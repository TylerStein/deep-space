using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZBHShieldController : MonoBehaviour
{
    public ZBHDirector director;
    public Transform originTransform;
    public GameObject shieldPrefab;
    public ZBHShieldGroup activeShieldGroup;
    public List<ZBHArcRenderer> shields = new List<ZBHArcRenderer>();

    private void Start() {
        if (!originTransform) originTransform = transform;
        SetShieldGroup(activeShieldGroup);
    }

    private void Update() {
        if (!director.isPlaying) return;
        int idx = 0;
        for (int i = 0; i < shields.Count; i++) {
            float rotSpeed = activeShieldGroup.settings[idx].rotateSpeed;
            float change = rotSpeed * Time.deltaTime;
            shields[i].SetAngles(shields[i].FromAngle + change, shields[i].ToAngle + change);
            shields[i].UpdateLine();

            idx++;
        }
    }

    public bool IntersectsWithShield(Vector3 fromPoint, out Vector3 hitPoint, out ZBHArcRenderer hitRenderer) {
        Vector3 diff = originTransform.position - fromPoint;
        float dist = diff.magnitude;
        Vector3 dir = diff.normalized;
        float angle = Mathf.Atan2(-dir.x, -dir.y) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;

        foreach (var shield in shields) {
            if (dist < shield.radius) {
                continue;
            }

            float min = shield.FromAngle;
            float max = shield.ToAngle;

            bool hit = false;
            if (shield.ToAngle < shield.FromAngle) {
                hit = (angle > shield.FromAngle || angle < shield.ToAngle);
            } else if (angle > shield.FromAngle && angle < shield.ToAngle) {
                hit = true;
            }

            if (hit) {
                hitPoint = originTransform.position + (-dir * shield.radius);
                hitRenderer = shield;
                return true;
            }
        }

        hitRenderer = null;
        hitPoint = originTransform.position;
        return false;
    }

    public void SetShieldGroup(ZBHShieldGroup group) {
        for (int i = 0; i < shields.Count; i++) {
            Destroy(shields[i].gameObject);
        }
        shields.Clear();

        activeShieldGroup = group;
        for (int i = 0; i < activeShieldGroup.settings.Count; i++) {
            InstantiateShield(activeShieldGroup.settings[i]);
        }

        shields.Sort((b, a) => Mathf.RoundToInt(a.radius * 1000f) - Mathf.RoundToInt(b.radius * 1000f));

        foreach (var shield in shields) {
            shield.UpdateLine();
        }
    }
    public void InstantiateShield(ZBHShieldSettings settings) {
        GameObject shieldObject = Instantiate(shieldPrefab, transform);
        ZBHArcRenderer arcRenderer = shieldObject.GetComponent<ZBHArcRenderer>();
        arcRenderer.SetSettings(settings);
        shields.Add(arcRenderer);
    }

    public void DestroyShield(int index) {
        var shield = shields[index];
        shields.RemoveAt(index);
        Destroy(shield.gameObject);
    }
}
