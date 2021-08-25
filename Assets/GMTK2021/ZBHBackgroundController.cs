using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZBHBackgroundElement
{
    public Transform transform;
    public float moveRate = 1f;
}

public class ZBHBackgroundController : MonoBehaviour
{
    public Vector2 min = -1 * Vector2.one;
    public Vector2 max = Vector2.one;

    public Vector2 lastValue;

    public List<ZBHBackgroundElement> elements = new List<ZBHBackgroundElement>();
    
    public void SetView(float x, float y) {
        lastValue = new Vector2(x, y);
        for (int i = 0; i < elements.Count; i++) {
            Vector3 pos = elements[i].transform.position;
            pos.x = elements[i].moveRate * Mathf.Lerp(min.x, max.x, x);
            pos.y = elements[i].moveRate * Mathf.Lerp(min.y, max.y, y);
            elements[i].transform.position = pos;
        }
    }
}
