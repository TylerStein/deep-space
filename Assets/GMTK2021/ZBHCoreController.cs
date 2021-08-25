using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ZBHCoreController : MonoBehaviour
{
    public ZBHDirector zbhDirector;
    public ZBHBoiler zbhBoiler;
    public UnityEvent hpChangedEvent = new UnityEvent();

    public bool lastHitValue;
    public bool isBeingHit;
    public float hitTickTimeSeconds = 1f;
    public int healthPoints = 120;
    public int maxHealthPoints = 120;

    private float hitTickTimer = 0f;

    private void Start() {
        healthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (!zbhDirector.isPlaying) return;
        if (lastHitValue != isBeingHit) {
            ResetTimer();
        }

        if (isBeingHit) {
            zbhBoiler.BoilForSeconds(0.1f);
        } else {
            zbhBoiler.ResetBoil();
        }

        hitTickTimer -= Time.deltaTime;
        if (hitTickTimer <= 0) {
            ResetTimer();
            if (isBeingHit) {
                healthPoints--;
                hpChangedEvent.Invoke();
                if (healthPoints <= 0) Kill();
            } else {
                healthPoints++;
                if (healthPoints > maxHealthPoints) healthPoints = maxHealthPoints;
                hpChangedEvent.Invoke();
            }
        }

        lastHitValue = isBeingHit;
    }

    void Kill() {
        Debug.Log("You Win :^)");
        zbhDirector.PlayWinOutro();
    }

    void ResetTimer() {
        hitTickTimer = hitTickTimeSeconds;
    }
}
