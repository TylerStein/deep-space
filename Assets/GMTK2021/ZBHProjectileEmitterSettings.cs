using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZBHProjectileEmitterSettings
{
    public ZBHProjectileEmitterMotion emitterMotionType = default;
    public ZBHProjectileTrajectoryType trajectoryType = default;

    [Range(0f, 1f)]
    public float originRadius = 1f;

    [Range(0.01f, 1f)]
    public float emitterSpawnDelay = 0.5f;

    [Range(0f, 1f)]
    public float emitterSpawnOffset = 0f;

    [Range(0.1f, 10f)]
    public float linearSpeed = 10f;

    [Range(1f, 720f)]
    public float turnSpeed = 60f;

    [Range(-1f, 60f)]
    public float toLinearTime = -1f;

    [Range(0, 360)]
    public float emitterAngleOffset = 0f;

    [Range(-1440, 1440)]
    public float emitterMotionSpeed = 10f;

    [Range(0, 100)]
    public float emitterMotionRange = 10f;
}
