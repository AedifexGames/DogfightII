using UnityEngine;

public class Autoscroll : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * (_moveSpeed * Time.deltaTime);
    }
}
