using UnityEngine;
public class CameraTarget : SingletonBehaviour<CameraTarget>
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offset;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_target.position.x + _offset, transform.position.y, transform.position.z);
    }

    public void SetOffset(float o) {  _offset = o; }
}
