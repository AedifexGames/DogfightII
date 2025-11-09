using UnityEngine;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour
{
    public UnityEvent onDestroy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            onDestroy?.Invoke();
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.GetComponent<Player>().Dashing)
            {
                onDestroy?.Invoke();
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            onDestroy?.Invoke();
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.collider.GetComponent<Player>().Dashing)
            {
                onDestroy?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
