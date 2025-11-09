using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    #region Variables
    private bool alive = true;
    [SerializeField] private float maxShield;
    [SerializeField] private float curShield;
    private float toughness = 0;

    [SerializeField] private UnityEvent onHit;
    [SerializeField] private UnityEvent onDie;
    [SerializeField] private UnityEvent onHeal;

    [SerializeField] private LayerMask damageMask;
    [SerializeField] private LayerMask healMask;
    #endregion

    public bool isAlive { get { return alive; } }

    public void damage(float amt) 
    {
        if (curShield > 0)
        {
            onHit?.Invoke();
            updateShield(-amt);
        }
        else
        {
            if (amt >= toughness)
            {
                kill();
            }
        }
    }

    private void kill()
    {
        alive = false;
        onDie?.Invoke();
    }

    private void revive()
    {
        alive = true;
    }

    private void updateShield(float amt)
    {
        curShield += amt;
        Mathf.Clamp(curShield, 0, maxShield);
    }

    public void regenPartShield(float amt)
    {
        onHeal?.Invoke();
        updateShield(amt);
    }

    private void regenFullShield()
    {
        onHeal?.Invoke();
        curShield = maxShield;
    }

    private void destroyShield()
    {
        curShield = 0;
    }
    private void reset()
    {
        revive();
        regenFullShield();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageMask == (damageMask | (1 << collision.gameObject.layer)))
        {
            damage(1.0f);
        }
        else if (healMask == (healMask | (1 << collision.gameObject.layer)))
        {
            regenFullShield();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageMask == (damageMask | (1 << collision.gameObject.layer)))
        {
            damage(1.0f);
        }
        else if (healMask == (healMask | (1 << collision.gameObject.layer)))
        {
            regenFullShield();
        }
    }
}
