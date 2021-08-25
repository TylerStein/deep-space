using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Emitter Group", menuName = "Emitters/Group", order = 0)]
public class ZBHProjectileEmitterGroup : ScriptableObject
{
    public List<ZBHProjectileEmitterSettings> emitters = new List<ZBHProjectileEmitterSettings>();
}