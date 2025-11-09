using UnityEngine;

public class RotateWithTransform : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _targetZRotVariance;
    [SerializeField] private float _targetYRotVariance;
    [SerializeField] private Vector3 _originalRot;

    private void Update()
    {
        float percent = (_target.rotation.eulerAngles.z - 270) / _targetZRotVariance;
        transform.localRotation = Quaternion.Euler(_originalRot.x + percent * _targetYRotVariance, _originalRot.y, _originalRot.z);
    }
}
