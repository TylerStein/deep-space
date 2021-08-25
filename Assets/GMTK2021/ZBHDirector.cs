using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class ZBHDirector : MonoBehaviour
{
    public List<AudioSource> cutsceneMutedSources;

    public bool playIntro = true;
    public bool animationMode = false;

    public bool isPlaying => !isLoading && !isInCutscene && !isPaused;

    public Color vingetteColor = Color.black;
    public float vingetteSmoothness = 0.38f;
    public float vingetteIntensity = 0.32f;
    public float vingetteRoundness = 1f;
    public float cameraOrthographicSize = 1.8f;
    public Vector3 playerPosition;
    public Vector3 corePosition;

    public PostProcessVolume postProcessVolume;
    public Vignette vingette;
    public Camera zbhCamera;
    public Transform playerTransform;
    public Transform coreTransform;

    public bool isLoading = false;
    public bool isInCutscene = false;
    public bool isPaused = false;

    public new Animation animation;

    private void Start() {
        isLoading = false;
        if (playIntro) PlayIntro();
        postProcessVolume.profile.TryGetSettings(out vingette);
        if (!zbhCamera) zbhCamera = Camera.main;
        Cursor.visible = false;
    }

    private void Update() {
        if (animationMode) {
            vingette.color.value = vingetteColor;
            vingette.smoothness.value = vingetteSmoothness;
            vingette.intensity.value = vingetteIntensity;
            vingette.roundness.value = vingetteRoundness;
            zbhCamera.orthographicSize = cameraOrthographicSize;
            playerTransform.position = playerPosition;
            coreTransform.position = corePosition;
        }
    }

    public void PlayIntro() {
        isInCutscene = true;
        animationMode = true;
        var clip = animation.GetClip("Director_Intro");
        animation.Play(clip.name, PlayMode.StopAll);
        SetSourcesMuted(true);
    }

    public void PlayLoseOutro() {
        isInCutscene = true;
        animationMode = true;
        var clip = animation.GetClip("Director_Outro");
        animation.Play(clip.name, PlayMode.StopAll);
        SetSourcesMuted(true);
    }

    public void PlayWinOutro() {
        isInCutscene = true;
        animationMode = true;
        var clip = animation.GetClip("Director_Outro_Win");
        animation.Play(clip.name, PlayMode.StopAll);
        SetSourcesMuted(true);
    }

    public void OnOutroComplete() {
        isInCutscene = false;
        animationMode = false;
        isLoading = true;
        SceneManager.LoadScene(0);
    }

    public void OnOutroWinComplete() {
        isInCutscene = false;
        animationMode = false;
        isLoading = true;
        SceneManager.LoadScene(0);
    }

    public void OnIntroComplete() {
        isInCutscene = false;
        animationMode = false;
        SetSourcesMuted(false);
    }

    public void SetSourcesMuted(bool muted) {
        foreach (var src in cutsceneMutedSources) {
            src.mute = muted;
        }
    }
}
