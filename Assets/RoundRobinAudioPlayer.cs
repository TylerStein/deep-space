using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundRobinAudioPlayer : MonoBehaviour
{
    public int sourceIndex = 0;
    public float sourceVolume = 0.5f;
    public List<AudioSource> audioSources = new List<AudioSource>();

    public void Awake() {
        for (int i = 0; i < audioSources.Count; i++) {
            audioSources[i].volume = sourceVolume;
        }
    }

    public void PlayNext(bool force = true) {
        if (!force && audioSources[sourceIndex].isPlaying) return;
        audioSources[sourceIndex].Play();
        sourceIndex++;
        if (sourceIndex >= audioSources.Count) sourceIndex = 0;
    }
}
