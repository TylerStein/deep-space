using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ZBHProjectile : MonoBehaviour
{
    public LayerMask playerLayer;
    public ZBHStageSpawner spawner;
    public ZBHProjectileTrajectoryType rule;
    public Vector3 trajectory;
    public float linearSpeed;
    public new Collider2D collider;
    public Animator animator;
    public bool despawning = false;
    public float despawnTime = 0.15f;
    public Transform seekTarget;
    public float turnSpeed;
    public float timer = 0f;
    public float toLinearTime = -1f;

    private void Awake() {
        if (!collider) collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawner.director.isPlaying) return;
        if (despawning) return;

        switch (rule) {
            case ZBHProjectileTrajectoryType.seeking:
                StepSeeking();
                break;
            case ZBHProjectileTrajectoryType.linear:
            default:
                StepLinear();
                break;
        }

        if (spawner.worldBounds.Contains(transform.position) == false) {
            // despawn on out of bounds w/o animation
            Despawn(false);
        }
    }

    void StepLinear() {
        transform.Translate(trajectory * linearSpeed * Time.deltaTime);
    }

    void StepSeeking() {
        var targetTrajectory = (seekTarget.position - transform.position).normalized;
        trajectory = Vector3.RotateTowards(trajectory, targetTrajectory, turnSpeed * Mathf.Deg2Rad * Time.deltaTime, 1f);
        StepLinear();

        if (toLinearTime >= 0f) {
            timer += Time.deltaTime;
            if (timer > toLinearTime) {
                timer = 0;
                rule = ZBHProjectileTrajectoryType.linear;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (playerLayer == (playerLayer | (1 << collision.gameObject.layer))) {
            // despawn on player collision w/ animation
            Despawn(true);
        }
    }

    void Despawn(bool animate) {
        if (animate) {
            despawning = true;
            collider.enabled = false;
            animator.SetBool("pop", true);
            StartCoroutine(DespawnRoutine());
        } else {
            despawning = false;
            collider.enabled = true;
            spawner.DespawnHazard(this);
        }
    }

    IEnumerator DespawnRoutine() {
        yield return new WaitForSeconds(despawnTime);
        Despawn(false);
        yield return null;
    }
}
