using UnityEngine;
public class CameraTarget : SingletonBehaviour<CameraTarget>
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offset;
    private const float _levelGenerateInterval = 5f;
    private float _currentTargetToLevelGen = 0f;
    [SerializeField] private float _lerpSpeed = 3f;

    private void Start()
    {
        UpdateOffset();
        _currentTargetToLevelGen = transform.position.x + _levelGenerateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOffset();
        UpdateLevelGen();
    }

    private void UpdateOffset()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(_target.position.x + _offset, transform.position.y, transform.position.z), Time.deltaTime * _lerpSpeed);
    }

    private void UpdateLevelGen()
    {
        if (transform.position.x >  _currentTargetToLevelGen)
        {
            _currentTargetToLevelGen += _levelGenerateInterval;
            LevelGenerator.instance?.GenerateColumn();
        }
    }

    public void SetOffset(float o) {  _offset = o; }
}
