using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve _lifetimeHorizontalOffset;
    [SerializeField] private float _horizontalOffsetIntensity = 1;
    [SerializeField] private AnimationCurve _lifetimeVelocity;
    [SerializeField] private float _velocityIntensity = 1;
    [SerializeField] private float _lifetime = 1;
    private float _currentTime;
    private Vector3 _position;
    [SerializeField] private UnityEvent _onDestroy;

    private void Start()
    {
        _position = transform.position;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _lifetime)
        {
            _onDestroy?.Invoke();
            Destroy(gameObject);
        }
        Vector3 offset = transform.right * _lifetimeHorizontalOffset.Evaluate(_currentTime / _lifetime) * _horizontalOffsetIntensity;
        _position += transform.up * _lifetimeVelocity.Evaluate(_currentTime / _lifetime) * Time.deltaTime * _velocityIntensity;
        transform.position = _position + offset;
    }
}