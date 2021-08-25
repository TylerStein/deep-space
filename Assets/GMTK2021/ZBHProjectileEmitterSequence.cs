using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Emitter Sequence", menuName = "Emitters/Sequence", order = 1)]
public class ZBHProjectileEmitterSequence : ScriptableObject
{
    public List<ZBHProjectileEmitterSequenceEntry> entries = new List<ZBHProjectileEmitterSequenceEntry>();
}

[System.Serializable]
public class ZBHProjectileEmitterSequenceEntry
{
    public ZBHProjectileEmitterGroup emitterGroup;
    public float minDurationSeconds = 15f;
    public float maxDurationSeconds = 30f;
}