using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Sequence", menuName = "Emitters/Stage", order = 3)]
public class ZBHStageSequence : ScriptableObject
{
    public ZBHProjectileEmitterSequence sequence;
    public ZBHShieldGroup shieldGroup;

    [Range(0f, 1f)]
    public float healthPercentage = 0.5f;
}
