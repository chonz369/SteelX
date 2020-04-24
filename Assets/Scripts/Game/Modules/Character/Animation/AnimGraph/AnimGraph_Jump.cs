using Unity.Entities;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Profiling;

[CreateAssetMenu(fileName = "Jump", menuName = "SteelX/Animation/AnimGraph/Jump")]
public class AnimGraph_Jump : AnimGraphAsset
{
    public AnimationClip[] animJumps = new AnimationClip[3];

    [Tooltip("The previous jump phase finished transition after the ratio part of the next phase has played.")]
    public float PhaseTransitionDurationRatio = 0.75f;
    [Tooltip("Jump height in animation. NOT actual ingame jump height")]
    public float jumpHeight = 1.7f; // Jump height of character in last frame of animation
    //public AnimationClip animAimDownToUp;

    public override IAnimGraphInstance Instantiate(EntityManager entityManager, Entity owner, PlayableGraph graph,
        Entity animStateOwner) {
        var animState = new Instance(entityManager, owner, graph, animStateOwner, this);
        return animState;
    }

    class Instance : IAnimGraphInstance, IGraphState
    {
        public Instance(EntityManager entityManager, Entity owner, PlayableGraph graph, Entity animStateOwner, AnimGraph_Jump settings) {
            m_EntityManager = entityManager;
            m_Owner = owner;
            m_AnimStateOwner = animStateOwner;
            phaseTransitionDurationRatio = settings.PhaseTransitionDurationRatio;
            m_additiveMixer = AnimationLayerMixerPlayable.Create(graph);

            mainMixer = AnimationMixerPlayable.Create(graph, settings.animJumps.Length);

            animJumpPlayables = new AnimationClipPlayable[settings.animJumps.Length];
            animJumpLengths = new float[settings.animJumps.Length];

            float totalJumpLength = 0;
            for (int i = 0; i < settings.animJumps.Length; i++) {
                var animJump = AnimationClipPlayable.Create(graph, settings.animJumps[i]);
                animJump.SetApplyFootIK(false);
                animJump.SetDuration(settings.animJumps[i].length);
                animJump.Pause();
                animJumpPlayables[i] = animJump;
                totalJumpLength += settings.animJumps[i].length;
                animJumpLengths[i] = settings.animJumps[i].length;
                mainMixer.SetInputWeight(i, i == 0 ? 1 : 0);

                graph.Connect(animJump, 0, mainMixer, i);
            }

            int port = m_additiveMixer.AddInput(mainMixer, 0);
            m_additiveMixer.SetLayerAdditive((uint)port, false);
            m_additiveMixer.SetInputWeight(port, 1);

            // Adjust play speed so vertical velocity in animation is matched with character velocity (so feet doesnt penetrate ground)
            var animJumpVel = settings.jumpHeight / totalJumpLength;
            var characterJumpVel = Game.config != null ? Game.config.jumpAscentHeight / Game.config.jumpAscentDuration : animJumpVel;
            playSpeed = characterJumpVel / animJumpVel;


            // Aim
            //m_aimHandler = new AimVerticalHandler(m_additiveMixer, settings.animAimDownToUp);
        }

        public void Shutdown() {
        }

        public void SetPlayableInput(int index, Playable playable, int playablePort) {
        }

        public void GetPlayableOutput(int index, ref Playable playable, ref int playablePort) {
            playable = m_additiveMixer;
            playablePort = 0;
        }

        public void UpdatePresentationState(bool firstUpdate, GameTime time, float deltaTime) {
            Profiler.BeginSample("Jump.Apply");

            var animState = m_EntityManager.GetComponentData<CharacterInterpolatedData>(m_AnimStateOwner);
            animState.rotation = animState.aimYaw;

            if (firstUpdate) {
                animState.jumpTime = 0;
                totalPlayedClipLength = 0;
                currentJumpPhase = 0;

                //transform jump time to the right jump phase
                for (int i = 0;i< animJumpLengths.Length; i++) {
                    if(animState.jumpTime > totalPlayedClipLength + animJumpLengths[i]) {
                        totalPlayedClipLength += animJumpLengths[i];
                        if(currentJumpPhase < animJumpLengths.Length - 1) {
                            currentJumpPhase++;
                        }
                    }
                }

                for (int i = 0; i < animJumpPlayables.Length; i++) {
                    mainMixer.SetInputWeight(i, i==currentJumpPhase ? 1 : 0);
                }
                mainMixer.SetTime(animState.jumpTime - totalPlayedClipLength);
            }
            else
                animState.jumpTime += playSpeed * deltaTime;

            if (currentJumpPhase < animJumpPlayables.Length - 1) {
                if (animState.jumpTime > totalPlayedClipLength + animJumpLengths[currentJumpPhase]) {
                    totalPlayedClipLength += animJumpLengths[currentJumpPhase];
                    currentJumpPhase++;
                }
            }

            m_EntityManager.SetComponentData(m_AnimStateOwner, animState);

            Profiler.EndSample();
        }

        public void ApplyPresentationState(GameTime time, float deltaTime) {
            Profiler.BeginSample("Jump.Apply");

            var animState = m_EntityManager.GetComponentData<CharacterInterpolatedData>(m_AnimStateOwner);
            //m_aimHandler.SetAngle(animState.aimPitch);            

            var t = Mathf.Min((animState.jumpTime - totalPlayedClipLength) / (animJumpLengths[currentJumpPhase] * phaseTransitionDurationRatio), 1);

            if((animState.jumpTime - totalPlayedClipLength) < animJumpLengths[currentJumpPhase] * phaseTransitionDurationRatio) {
                if (currentJumpPhase > 0) {
                    mainMixer.SetInputWeight(currentJumpPhase - 1, 1 - t);
                    animJumpPlayables[currentJumpPhase-1].SetTime(animJumpLengths[currentJumpPhase - 1] * (1- phaseTransitionDurationRatio) 
                        + animJumpLengths[currentJumpPhase - 1] * phaseTransitionDurationRatio * t);
                }
                mainMixer.SetInputWeight(currentJumpPhase, t);
            }

            animJumpPlayables[currentJumpPhase].SetTime(animState.jumpTime - totalPlayedClipLength);
            Profiler.EndSample();
        }

        EntityManager m_EntityManager;
        Entity m_Owner;
        Entity m_AnimStateOwner;
        AnimationLayerMixerPlayable m_additiveMixer;
        AnimationMixerPlayable mainMixer;
        AnimationClipPlayable[] animJumpPlayables;
        float[] animJumpLengths;
        //AimVerticalHandler m_aimHandler;
        float phaseTransitionDurationRatio;
        float totalPlayedClipLength;
        int currentJumpPhase;

        float playSpeed;
    }
}
