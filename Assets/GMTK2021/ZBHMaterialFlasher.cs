using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZBHMaterialFlasher : MonoBehaviour
{
    public ZBHDirector director;
    public List<MeshRenderer> meshes = new List<MeshRenderer>();
    public List<Color> originalColors = new List<Color>();
    public Color flashColor;
    public string colorProperty = "_Color";
    public float flashDuration = 2f;
    public float flashRate = 0.5f;

    [SerializeField] private float totalRemainingTime = 0f;
    [SerializeField] private float stepRemainingTime = 0f;
    [SerializeField] private bool isFlashed = false;

    void Start() {
        originalColors.Clear();
        for (int i = 0; i < meshes.Count; i++) {
            Color color = meshes[i].material.GetColor(colorProperty);
            originalColors.Add(color);
        }
    }

    void Update() {
        if (!director.isPlaying) return;

        if (totalRemainingTime <= 0f) {
            if (isFlashed) SetOriginal();
            totalRemainingTime = 0f;
        } else if (stepRemainingTime <= 0f) {
            stepRemainingTime = flashRate;
            totalRemainingTime -= flashRate;
            if (isFlashed) SetOriginal();
            else SetFlashed();
        } else {
            stepRemainingTime -= Time.deltaTime;
        }
    }

    public void Flash() {
        totalRemainingTime = flashDuration;
        stepRemainingTime = flashRate;
        SetFlashed();
    }

    private void OnDestroy() {
        SetOriginal();
    }
    private void SetFlashed() {
        for (int i = 0; i < meshes.Count; i++) {
             meshes[i].material.SetColor(colorProperty, flashColor);
        }
        isFlashed = true;
    }

    private void SetOriginal() {
        for (int i = 0; i < meshes.Count; i++) {
            meshes[i].material.SetColor(colorProperty, originalColors[i]);
        }
        isFlashed = false;
    }
}
