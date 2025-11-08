using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnTimer : MonoBehaviour
{
    [SerializeField] private UnityEvent _onTimer;
    [SerializeField] private float _initialTimerDuration;
    [SerializeField] private float _currentTimer;
    private bool _isRunning = true;

    public void ResetTimer() {  _currentTimer = _initialTimerDuration; }

    private void Awake()
    {
        _onTimer.AddListener(ResetTimer);
    }

    private void FixedUpdate()
    {
        if(!_isRunning) return;
        _currentTimer -= Time.deltaTime;
        if (_currentTimer < 0)
        {
            _onTimer?.Invoke();
            ResetTimer();
        }
    }

    public void Pause()
    {
        _isRunning = false;
    }

    public void Unpause()
    {
        _isRunning = true;
    }
}
