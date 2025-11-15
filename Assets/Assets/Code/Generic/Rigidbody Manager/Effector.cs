using Physics;
using UnityEngine;

public class Effector : MonoBehaviour
{
    [SerializeField] private LayerMask _effectMask;
    [SerializeField] private string _effectName;
    [SerializeField] private Vector2 _force;
    [SerializeField] private float _angularForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_effectMask == (_effectMask | (1 << collision.gameObject.layer)))
        {
            ApplyForce(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_effectMask == (_effectMask | (1 << collision.gameObject.layer)))
        {
            RemoveForce(collision.gameObject);
        }
    }

    private void ApplyForce(GameObject obj)
    {
        Rigidbody2DManager rb = obj.GetComponent<Rigidbody2DManager>();
        if (rb == null) return;
        rb.AddForce(_effectName, _force, _angularForce, 0);
    }

    private void RemoveForce(GameObject obj)
    {
        Rigidbody2DManager rb = obj.GetComponent<Rigidbody2DManager>();
        if (rb == null) return;
        rb.RemoveForce(_effectName);
    }
}
