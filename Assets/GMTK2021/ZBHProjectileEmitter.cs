using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZBHProjectileTrajectoryType
{
    linear = 0,
    accelerating = 1,
    curve = 2,
    seeking = 3
}

public enum ZBHProjectileEmitterMotion
{
    stationary = 0,
    rotating = 1,
    sine = 2,
    pingpong = 4
}

[ExecuteAlways]
public class ZBHProjectileEmitter : MonoBehaviour
{
    public bool updateMotionInEditMode = false;
    public ZBHStageSpawner spawner;

    public Transform emitterTransform;
    public Transform emitterOrigin;

    [SerializeField] private ZBHProjectileEmitterSettings settings;

    [SerializeField] private float emitterRotationAngle = 0f;
    [SerializeField] private Vector3 emitterPosition = Vector3.zero;
    [SerializeField] private Vector3 emitterDirection = Vector3.zero;
    [SerializeField] private float spawnTimer = 0f;
    [SerializeField] private float emitterProgress = 0f;

    public void SetSettings(ZBHProjectileEmitterSettings settings) {
        this.settings = settings;
        spawnTimer = settings.emitterSpawnOffset;
        emitterProgress = 0f;
    }

    public void Start() {
        if (!emitterTransform) emitterTransform = transform;
        emitterRotationAngle = settings.emitterAngleOffset;
    }

    public void Update() {
        if (!spawner.director.isPlaying) return;
        if (Application.isPlaying || updateMotionInEditMode) {
            UpdateMotion();
        }

        UpdateValues();

        if (Application.isPlaying) {
            UpdateSpawning();
        }

    }

    public void UpdateSpawning() {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f) {
            spawnTimer = settings.emitterSpawnDelay;
            EmitProjectile(spawner.SpawnHazard());
        }
    }

    public void UpdateMotion() {
        switch (settings.emitterMotionType) {
            case ZBHProjectileEmitterMotion.rotating:
                emitterRotationAngle += settings.emitterMotionSpeed * Time.deltaTime;
                break;
            case ZBHProjectileEmitterMotion.sine:
                emitterProgress += Time.deltaTime * settings.emitterMotionSpeed;
                emitterRotationAngle = settings.emitterAngleOffset + (Mathf.Sin(emitterProgress) * settings.emitterMotionRange);
                break;
            default: return;
        }

        if (emitterRotationAngle > 360) emitterRotationAngle -= 360;
        else if (emitterRotationAngle < 0) emitterRotationAngle += 360;
    }

    public void UpdateValues() {

        var theta = Mathf.Deg2Rad * emitterRotationAngle;
        Vector3 point = new Vector3(Mathf.Sin(theta), Mathf.Cos(theta), 0);
        emitterPosition = emitterTransform.position + (point * settings.originRadius);
        emitterDirection = (point - emitterOrigin.position).normalized;
    }

    public void EmitProjectile(ZBHProjectile obj) {
        if (!obj) return;
        obj.transform.position = emitterPosition;
        obj.trajectory = emitterDirection;
        obj.rule = settings.trajectoryType;
        obj.linearSpeed = settings.linearSpeed;
        obj.turnSpeed = settings.turnSpeed;
        obj.seekTarget = spawner.playerTransform;
        obj.toLinearTime = settings.toLinearTime;
        obj.timer = 0f;
    }

    public void OnDrawGizmosSelected() {
        if (emitterOrigin) {
            Gizmos.color = Color.white;
            Vector3 edgePosition = emitterOrigin.position + (emitterDirection * settings.originRadius);
            Gizmos.DrawLine(emitterOrigin.position, edgePosition);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(edgePosition, edgePosition + (emitterDirection * 10f));
        }
    }
}
