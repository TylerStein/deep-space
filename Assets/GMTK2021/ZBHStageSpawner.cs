using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZBHStageSpawner : MonoBehaviour
{
    public RoundRobinAudioPlayer projectileAudio;
    public ZBHCoreController zbhCore;
    public ZBHDirector director;
    public ZBHShieldController zbhShield;
    public Transform playerTransform;
    public Bounds worldBounds = new Bounds() {
        center = new Vector3(0, 0, 0),
        size = new Vector3(1f, 1f, 1f),
    };

    public int maxHazards = 256;
    public int currentStage = 0;

    public float lowestHealthPct = 1f;

    public GameObject projectilePrefab;
    public GameObject emitterPrefab;

    public List<ZBHStageSequence> stages = new List<ZBHStageSequence>();

    public ZBHStageSequence activeStage;
    public ZBHProjectileEmitterGroup activeEmitterGroup;
    public ZBHProjectileEmitterSequence activeEmitterSequence;

    public float activeEmitterSequenceCountdown = 0f;
    public float timeScale = 1f;

    public List<ZBHProjectileEmitter> emitters = new List<ZBHProjectileEmitter>();
    List<ZBHProjectile> activeHazards = new List<ZBHProjectile>();
    List<ZBHProjectile> idleHazards = new List<ZBHProjectile>();


    private void Start() {
        zbhCore.hpChangedEvent.AddListener(UpdateStage);
        for (int i = 0; i < emitters.Count; i++) {
            emitters[i].spawner = this;
        }

        for (int i = 0; i < maxHazards; i++) {
            InstantiateIdleHazard();
        }

        stages.Sort((a, b) => Mathf.RoundToInt(a.healthPercentage * 1000) - Mathf.RoundToInt(b.healthPercentage * 1000));
        UpdateStage();
    }

    private void OnDestroy() {
        zbhCore.hpChangedEvent.RemoveListener(UpdateStage);
    }

    private void Update() {
        if (!director.isPlaying) return;

        if (activeEmitterSequenceCountdown < 0) {
            NextEmitterGroup();
        } else {
            activeEmitterSequenceCountdown -= Time.deltaTime * timeScale;
        }
    }


    void UpdateStage() {
        float pct = (float)zbhCore.healthPoints / zbhCore.maxHealthPoints;
        if (pct > lowestHealthPct) pct = lowestHealthPct;
        else lowestHealthPct = pct;

        int idx = -1;
        for (int i = 0; i < stages.Count; i++) {
            float stagePct = stages[i].healthPercentage;
            if (pct <= stagePct) {
                idx = i;
                break;
            }
        }

        if (idx == -1 && stages.Count > 0) {
            idx = 0;
        }

        if (stages[idx] != activeStage) {
            SetStage(idx);
        }
    }

    void InstantiateIdleHazard() {
        GameObject obj = Instantiate(projectilePrefab, transform);
        ZBHProjectile projectile = obj.GetComponent<ZBHProjectile>();
        projectile.transform.position = transform.position;
        projectile.spawner = this;
        idleHazards.Add(projectile);
        obj.SetActive(false);
    }

    public ZBHProjectile SpawnHazard() {
        if (idleHazards.Count == 0) {
            Debug.Log("Failed to spawn Hazard: Idle list is empy");
        }

        ZBHProjectile projectile = idleHazards[0];
        idleHazards.RemoveAt(0);
        activeHazards.Add(projectile);
        projectile.gameObject.SetActive(true);
        projectile.transform.position = transform.position;
        PlayProjectileAudio();
        return projectile;
    }

    public void DespawnHazard(ZBHProjectile projectile) {
        int index = activeHazards.IndexOf(projectile);
        if (index == -1) return;

        idleHazards.Add(activeHazards[index]);
        activeHazards.RemoveAt(index);
        projectile.gameObject.SetActive(false);
    }

    public void SetEmitterGroup(ZBHProjectileEmitterGroup group) {
        for (int i = 0; i < emitters.Count; i++) {
            Destroy(emitters[i]);
        }
        emitters.Clear();

        activeEmitterGroup = group;
        for (int i = 0; i < group.emitters.Count; i++) {
            InstantiateEmitter(group.emitters[i]);
        }
    }

    public void InstantiateEmitter(ZBHProjectileEmitterSettings settings) {
        GameObject emitterObject = Instantiate(emitterPrefab, transform);
        ZBHProjectileEmitter emitter = emitterObject.GetComponent<ZBHProjectileEmitter>();
        emitters.Add(emitter);
        emitter.emitterOrigin = transform;
        emitter.spawner = this;
        emitter.SetSettings(settings);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
    }

    public void SetStage(int index) {
        activeStage = stages[index];
        activeEmitterSequence = activeStage.sequence;
        zbhShield.SetShieldGroup(activeStage.shieldGroup);
        NextEmitterGroup();
    }

    public void NextEmitterGroup() {
        if (activeEmitterSequence == null) return;
        int count = activeEmitterSequence.entries.Count;
        ZBHProjectileEmitterSequenceEntry entry;
        if (count == 0) {
            // nothing to use
            return;
        } else if (count == 1) {
            entry = activeEmitterSequence.entries[0];
        } else {
            entry = activeEmitterSequence.entries.Where((x) => x.emitterGroup != activeEmitterGroup).ElementAt(Random.Range(0, count - 1));
        }

        activeEmitterSequenceCountdown = Random.Range(entry.minDurationSeconds, entry.maxDurationSeconds);
        SetEmitterGroup(entry.emitterGroup);
    }

    public void PlayProjectileAudio() {
        projectileAudio.PlayNext(true);
    }
}
