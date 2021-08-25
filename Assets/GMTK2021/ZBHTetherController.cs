using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZBHTetherController : MonoBehaviour
{
    public ZBHDirector zbhDirector;
    public Transform zbhPoint;
    public ZBHCoreController zbhCore;

    public LineRenderer tetherLineRenderer;

    public ZBHShieldController sheildController;

    public int extraSegments = 12;

    public float activeWiggleAmplitude = 0.15f;
    public float activeWiggleFrequency = 0.15f;

    public Gradient shieldHitLineColor;
    public float shieldWiggleAmplitude = 0.15f;
    public float shieldWiggleFrequency = 0.15f;

    public Gradient bossHitLineColor;
    public float bossWiggleAmplitude = 0.15f;
    public float bossWiggleFrequency = 0.15f;

    public float perlinOffset = 0f;
    public float perlinYScale = 1f;

    public float baseCoreRadius = 0.1f;

    public AudioSource tetheredAudioSource;
    public AudioSource untetheredAudioSource;
    public float audioSourceVolume = 1f;

    // Update is called once per frame
    void Update()
    {
        if (!zbhDirector.isPlaying) return;

        perlinOffset += activeWiggleFrequency * Time.deltaTime;

        if (extraSegments < 0) extraSegments = 0;
        if (tetherLineRenderer.positionCount != (extraSegments + 2)) {
            tetherLineRenderer.positionCount = extraSegments + 2;
        }

        Vector3 hitTarget;
        ZBHArcRenderer arcRenderer;
        bool didHit = sheildController.IntersectsWithShield(zbhPoint.transform.position, out hitTarget, out arcRenderer);
        if (didHit) {
            tetherLineRenderer.colorGradient = shieldHitLineColor;
            activeWiggleAmplitude = shieldWiggleAmplitude;
            activeWiggleFrequency = shieldWiggleFrequency;
            tetheredAudioSource.volume = 0f;
            untetheredAudioSource.volume = audioSourceVolume;
        } else {
            hitTarget = zbhCore.transform.position + (zbhPoint.transform.position - zbhCore.transform.position).normalized * baseCoreRadius;
            tetherLineRenderer.colorGradient = bossHitLineColor;
            activeWiggleAmplitude = bossWiggleAmplitude;
            activeWiggleFrequency = bossWiggleFrequency;
            tetheredAudioSource.volume = audioSourceVolume;
            untetheredAudioSource.volume = 0f;
        }


        Vector3 diff = hitTarget - zbhPoint.transform.position;
        Vector3 dir = diff.normalized;
        Vector3 rotated = new Vector3(dir.y, -dir.x);

        float count = extraSegments + 2;
        for (int i = 0; i < count; i++) {

            float noise = Mathf.PerlinNoise(perlinOffset, i * perlinYScale) * activeWiggleAmplitude;
            noise -= (activeWiggleAmplitude / 2);

            float t = i / (count - 1);

            float dist = diff.magnitude;
            Vector3 point = zbhPoint.transform.position + (dir * (dist * t));

            if (i > 1 && i < (count - 1)) {
                point += rotated * noise; // Radom.Range(-wiggleAmplitude, wiggleAmplitude);
            }

            tetherLineRenderer.SetPosition(i, point);
        }

        zbhCore.isBeingHit = !didHit;
    }
}
