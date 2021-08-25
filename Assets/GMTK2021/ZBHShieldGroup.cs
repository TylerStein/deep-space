using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield Group", menuName = "Shields/Group", order = 0)]
public class ZBHShieldGroup : ScriptableObject
{
    public List<ZBHShieldSettings> settings = new List<ZBHShieldSettings>();
}

[System.Serializable]
public class ZBHShieldSettings
{
    public float radius = 0.25f;
    public float stepSize = 12;
    public float rotateSpeed = 10f;
    public float angleOffset = 0f;
    public float size = 90f;
}
