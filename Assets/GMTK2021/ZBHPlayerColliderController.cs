using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityCollider2DEvent : UnityEvent<Collider2D> { }

[RequireComponent(typeof(Collider2D))]
public class ZBHPlayerColliderController : MonoBehaviour
{
    public UnityCollider2DEvent hazardCollisionEvent;
    public LayerMask hazardLayer;
    public LayerMask coreLayer;
    public new Collider2D collider;

    // Start is called before the first frame update
    void Awake()
    {
        if (!collider) collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (hazardLayer == (hazardLayer | (1 << collision.gameObject.layer))) {
            hazardCollisionEvent.Invoke(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (coreLayer == (coreLayer | (1 << collision.gameObject.layer))) {
            hazardCollisionEvent.Invoke(collision);
        }
    }
}
