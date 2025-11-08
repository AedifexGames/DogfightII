using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_target.position.x + _offset.x, transform.position.y, transform.position.z);
    }
}
