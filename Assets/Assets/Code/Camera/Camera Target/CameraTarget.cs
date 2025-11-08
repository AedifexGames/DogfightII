using UnityEngine;
public class CameraTarget : SingletonBehaviour<CameraTarget>
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offset;
    private const float _levelGenerateInterval = 5f;
    private float _currentTargetToLevelGen = 0f;

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
        transform.position = new Vector3(_target.position.x + _offset, transform.position.y, transform.position.z);
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
