using UnityEngine;

public class AutoDestroyTimer : MonoBehaviour
{
    private float _lifetime = 0;

    private void FixedUpdate()
    {
        _lifetime -= Time.fixedDeltaTime;
        if (_lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetLifetime(float lifetime) { _lifetime = lifetime; }
}
