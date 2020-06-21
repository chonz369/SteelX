using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Profiling;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[CreateAssetMenu(fileName = "InAir", menuName = "SteelX/Animation/AnimGraph/InAir")]
public class AnimGraph_InAir : AnimGraphAsset
{
    public List<BlendSpaceNode> inAirBlendSpaceNodes;
    public List<BlendSpaceNode> landAnticBlendSpaceNodes;

    public float landAnticStartHeight = 0.3f;
    public float blendDuration = 0.1f;

    public float damping = 0.1f;
    public float maxStep = 15f;

    [Range(0f, 1f)]
    [Tooltip("The max. time between exiting ground move and re-entering, before a state reset is triggered")]
    public float stateResetWindow;

    public AnimationClip animAimDownToUp;

    public ActionAnimationDefinition[] actionAnimations;

    public override IAnimGraphInstance Instantiate(EntityManager entityManager, Entity owner, PlayableGraph graph,
        Entity animStateOwner) {
        return new Instance(entityManager, owner, graph, animStateOwner, this);
    }

    class Instance : IAnimGraphInstance, IGraphState
    {
        public Instance(EntityManager entityManager, Entity owner, PlayableGraph graph, Entity animStateOwner, AnimGraph_InAir settings) {
            m_settings = settings;
            m_EntityManager = entityManager;
            m_Owner = owner;
            m_AnimStateOwner = animStateOwner;

            GameDebug.Assert(entityManager.HasComponent<Character>(m_AnimStateOwner), "Owner has no Character component");
            m_character = entityManager.GetComponentObject<Character>(m_AnimStateOwner);

            m_mainMixer = AnimationMixerPlayable.Create(graph, 2);

            m_inAirBlendTree = new BlendTree2dSimpleDirectional(graph, settings.inAirBlendSpaceNodes);
            m_LandAnticBlendTree = new BlendTree2dSimpleDirectional(graph, settings.landAnticBlendSpaceNodes);
            
            graph.Connect(m_inAirBlendTree.GetRootPlayable(), 0, m_mainMixer, 0);
            graph.Connect(m_LandAnticBlendTree.GetRootPlayable(), 0, m_mainMixer, 1);

            m_mainMixer.SetInputWeight(0, 1.0f);
            m_mainMixer.SetInputWeight(1, 0);

            m_layerMixer = AnimationLayerMixerPlayable.Create(graph);
            var port = m_layerMixer.AddInput(m_mainMixer, 0);
            m_layerMixer.SetInputWeight(port, 1);
            
            // Aim
            //if (settings.animAimDownToUp != null)
            //    m_aimHandler = new AimVerticalHandler(m_layerMixer, settings.animAimDownToUp);

            // Actions
            m_actionAnimationHandler = new ActionAnimationHandler(m_layerMixer, settings.actionAnimations);
        }

        public void Shutdown() {
        }

        public void SetPlayableInput(int index, Playable playable, int playablePort) {
        }

        public void GetPlayableOutput(int index, ref Playable playable, ref int playablePort) {
            playable = m_layerMixer;
            playablePort = 0;
        }

        public void UpdatePresentationState(bool firstUpdate, GameTime time, float deltaTime) {
            Profiler.BeginSample("InAir.Update");

            var animState = m_EntityManager.GetComponentData<CharacterInterpolatedData>(m_AnimStateOwner);
            var charState = m_EntityManager.GetComponentData<CharacterPredictedData>(m_AnimStateOwner);

            if (firstUpdate) {
                animState.inAirTime = 0;

                animState.moveAngleLocal = CalculateMoveAngleLocal(animState.rotation, animState.moveYaw);
                animState.locomotionVector = AngleToPosition(animState.moveAngleLocal);
            } else {
                animState.inAirTime += deltaTime;
            }

            animState.rotation = animState.aimYaw;
            animState.moveAngleLocal = CalculateMoveAngleLocal(animState.rotation, animState.moveYaw);

            // Blend in land anticipation when close to ground // TODO only do this test when moving downwards
            var nearGround = m_character.altitude < m_settings.landAnticStartHeight;
            var deltaWeight = deltaTime / m_settings.blendDuration;
            animState.landAnticWeight += nearGround ? deltaWeight : -deltaWeight;
            animState.landAnticWeight = Mathf.Clamp(animState.landAnticWeight, 0, 1);

            var targetBlend = AngleToPosition(animState.moveAngleLocal);
            animState.locomotionVector = Vector2.SmoothDamp(animState.locomotionVector, targetBlend, ref m_CurrentVelocity, m_settings.damping, m_settings.maxStep, deltaTime); ;

            m_DoUpdateBlendPositions = false;
            m_LandAnticBlendTree.SetBlendPosition(animState.locomotionVector, false);
            m_inAirBlendTree.SetBlendPosition(animState.locomotionVector, false);

            m_EntityManager.SetComponentData(m_AnimStateOwner, animState);

            Profiler.EndSample();
        }

        public void ApplyPresentationState(GameTime time, float deltaTime) {
            Profiler.BeginSample("InAir.Apply");

            var animState = m_EntityManager.GetComponentData<CharacterInterpolatedData>(m_AnimStateOwner);

            if (m_DoUpdateBlendPositions) {
                m_inAirBlendTree.SetBlendPosition(animState.locomotionVector, false);
                m_LandAnticBlendTree.SetBlendPosition(animState.locomotionVector, false);
            }

            var landAnticWeight = animState.landAnticWeight;

            m_mainMixer.SetInputWeight(0, 1.0f - landAnticWeight);
            m_mainMixer.SetInputWeight(1, landAnticWeight);
            
            m_inAirBlendTree.UpdateGraph();
            m_LandAnticBlendTree.UpdateGraph();

            m_inAirBlendTree.SetPhase(animState.inAirTime);
            m_LandAnticBlendTree.SetPhase(animState.inAirTime);

            var characterActionDuration = time.DurationSinceTick(animState.charActionTick);
            m_actionAnimationHandler.UpdateAction(animState.charAction, characterActionDuration);
            //if (m_aimHandler != null)
            //    m_aimHandler.SetAngle(animState.aimPitch);

            Profiler.EndSample();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float CalculateMoveAngleLocal(float rotation, float moveYaw) {
            // Get new local move angle
            var moveAngleLocal = Mathf.DeltaAngle(rotation, moveYaw);

            // We cant blend running sideways and running backwards so in range 90->135 we snap to either sideways or backwards
            var absMoveAngle = Mathf.Abs(moveAngleLocal);
            if (absMoveAngle > 90 && absMoveAngle < 135) {
                var sign = Mathf.Sign(moveAngleLocal);
                moveAngleLocal = absMoveAngle > 112.5f ? sign * 135.0f : sign * 90.0f;
            }
            return moveAngleLocal;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector2 AngleToPosition(float _angle) {
            var dir3D = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
            return new Vector2(dir3D.x, dir3D.z);
        }

        AnimGraph_InAir m_settings;
        EntityManager m_EntityManager;
        Entity m_Owner;
        Entity m_AnimStateOwner;

        Character m_character;
        float m_playSpeed;

        BlendTree2dSimpleDirectional m_inAirBlendTree;
        BlendTree2dSimpleDirectional m_LandAnticBlendTree;

        AnimationLayerMixerPlayable m_inAirMixer, m_landAnticMixer;
        AnimationMixerPlayable m_mainMixer;

        AnimationClipPlayable m_animInAir;
        AnimationClipPlayable m_animLandAntic;

        int inAirPort;
        int landAnticPort;
        bool m_DoUpdateBlendPositions = true;
        Vector2 m_CurrentVelocity;

        AnimationLayerMixerPlayable m_layerMixer;

        ActionAnimationHandler m_actionAnimationHandler;
        //AimVerticalHandler m_aimHandler;
    }
}
