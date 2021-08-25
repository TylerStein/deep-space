using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ZBHPlayerHealthController : MonoBehaviour
{
    public ZBHDirector director;
    public int healthPoints = 6;
    public int maxHealthPoints = 6;
    public UnityEvent hpLostEvent = new UnityEvent();
    public ZBHPlayerColliderController zbhPlayerCollider;
    public ZBHMaterialFlasher playerFlasher;
    public AudioSource playerHitAudio;

    // Start is called before the first frame update
    void Awake()
    {
        zbhPlayerCollider.hazardCollisionEvent.AddListener(onEvent_hazardCollision);
        healthPoints = maxHealthPoints;
    }

    private void OnDestroy() {
        zbhPlayerCollider.hazardCollisionEvent.RemoveListener(onEvent_hazardCollision);
    }

    void onEvent_hazardCollision(Collider2D collision) {
        if (!director.isPlaying) return;
        if (healthPoints <= 0) return;
        playerFlasher.Flash();
        healthPoints--;
        hpLostEvent.Invoke();
        playerHitAudio.Play();
        if (healthPoints <= 0) Kill();
    }

    void Kill() {
        Debug.Log("You Lose :^(");
        director.PlayLoseOutro();
    }
}
