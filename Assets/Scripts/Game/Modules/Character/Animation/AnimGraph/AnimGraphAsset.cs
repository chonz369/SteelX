using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;

public abstract class AnimGraphAsset : ScriptableObject
{
    public abstract IAnimGraphInstance Instantiate(EntityManager entityManager, Entity owner, PlayableGraph graph,
        Entity animStateOwner);
}

