using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnTimer : MonoBehaviour
{
    [SerializeField] private UnityEvent _onTimer;
    [SerializeField] private float _initialTimerDuration;
    [SerializeField] private float _currentTimer;

    public void ResetTimer() {  _currentTimer = _initialTimerDuration; }

    private void Awake()
    {
        _onTimer.AddListener(ResetTimer);
    }

    private void FixedUpdate()
    {
        _currentTimer -= Time.deltaTime;
        if (_currentTimer < 0)
        {
            _onTimer?.Invoke();
        }
    }
}
