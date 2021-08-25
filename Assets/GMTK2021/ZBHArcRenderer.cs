using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZBHArcRenderer : MonoBehaviour
{
    public Vector3 center;
    public LineRenderer lineRenderer;

    public float radius = 0.25f;
    public float stepSize = 12;

    [SerializeField] private float fromAngle = -45f;
    [SerializeField] private float toAngle = 45f;

    public float FromAngle => fromAngle;
    public float ToAngle => toAngle;

    public void SetSettings(ZBHShieldSettings settings) {
        stepSize = settings.stepSize;
        radius = settings.radius;
        float from = settings.angleOffset - (settings.size / 2f);
        float to = settings.angleOffset + (settings.size / 2f);
        SetAngles(from, to);
    }
    public void SetAngles(float from, float to) {
        fromAngle = from;
        toAngle = to;

        if (fromAngle < 0f) fromAngle += 360f;
        else if (fromAngle > 360f) fromAngle -= 360f;

        if (toAngle < 0f) toAngle += 360f;
        else if (toAngle > 360f) toAngle -= 360f;
    }

    public void UpdateLine() {
        //if (renderer.ToAngle < renderer.FromAngle) {
        //    hit = (angle > renderer.FromAngle || angle < renderer.ToAngle);
        //} else if (angle > renderer.FromAngle && angle < renderer.ToAngle) {
        //    hit = true;
        //}

        Vector2 selfPosition = transform.position;
        if (fromAngle < toAngle) {
            // eg. 0 to 10
            float length = toAngle - fromAngle;
            int count = Mathf.RoundToInt(length / stepSize);
            lineRenderer.positionCount = count;

            for (int i = 0; i < count; i++) {
                float lerpAngle = Mathf.Lerp(fromAngle, toAngle, (float)i / (count - 1f));
                //if (lerpAngle < 0) {
                //    lerpAngle += 360;
                //}

                Vector2 pos = selfPosition + GetAnglePosition(lerpAngle * Mathf.Deg2Rad, radius);
                lineRenderer.SetPosition(i, pos);
            }

        } else {
            // eg. 350 to 10
            float offset = (360 - fromAngle);
            float length = offset + toAngle;
            int count = Mathf.RoundToInt(length / stepSize);
            lineRenderer.positionCount = count;

            for (int i = 0; i < count; i++) {
                float lerpAngle = Mathf.Lerp(0f, toAngle + offset, (float)i / (count - 1f)) - offset;
                //if (lerpAngle < 0) {
                //    lerpAngle += 360;
                //}

                Vector2 pos = selfPosition + GetAnglePosition(lerpAngle * Mathf.Deg2Rad, radius);
                lineRenderer.SetPosition(i, pos);
            }
        }
    }

    public Vector2 GetAnglePosition(float theta, float radius) {
        return new Vector2(Mathf.Sin(theta) * radius, Mathf.Cos(theta) * radius);
    }
}
