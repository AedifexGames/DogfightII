using UnityEngine;

public class Health
{

    #region Variables
    private bool alive = true;
    [SerializeField] private float maxShield;
    [SerializeField] private float curShield;
    private float toughness = 0;
    #endregion

    public bool isAlive { get { return alive; } }

    private void damage(float amt) 
    {
        if (curShield > 0)
        {
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

    private void regenPartShield(float amt)
    {
        updateShield(amt);
    }

    private void regenFullShield()
    {
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
}
