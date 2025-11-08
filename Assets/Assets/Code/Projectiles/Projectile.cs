using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve _lifetimeHorizontalOffset;
    [SerializeField] private AnimationCurve _lifetimeVelocity;
    [SerializeField] private float _lifetime = 1;
    private float _currentTime;
    private Vector3 _position;
    [SerializeField] private UnityEvent _onDestroy;

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _lifetime)
        {
            _onDestroy?.Invoke();
            Destroy(gameObject);
        }
        Vector3 offset = transform.right * _lifetimeHorizontalOffset.Evaluate(_currentTime / _lifetime);
        _position += transform.up * _lifetimeVelocity.Evaluate(_currentTime / _lifetime) * Time.deltaTime;
        transform.position = _position + offset;
    }
}