using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZBHBoiler : MonoBehaviour
{
    public Vector3 originalPosition;
    public float frequency = 0.01f;
    public Vector3 amplitude = new Vector3(0.05f, 0.05f, 0f);

    public float boilTime = 0f;
    public float stepTime = 0f;

    private void Start() {
        originalPosition = transform.position;
    }

    public void BoilForSeconds(float seconds) {
        boilTime = seconds;
        stepTime = 0f;
    }

    // Update is called once per frame
    void Update() {
        if (boilTime > 0f) {
            boilTime -= Time.deltaTime;
            stepTime -= Time.deltaTime;
            if (stepTime <= 0f) {
                StepBoil();
                stepTime = frequency;
            }
        }

    }

    public void ResetBoil() {
        transform.position = originalPosition;
    }

    void StepBoil() {
        Vector3 nextBoil = new Vector3(
            Random.Range(-amplitude.x, amplitude.x),
            Random.Range(-amplitude.y, amplitude.y),
            Random.Range(-amplitude.z, amplitude.z)
        );
        transform.position = originalPosition + nextBoil;
    }
}
