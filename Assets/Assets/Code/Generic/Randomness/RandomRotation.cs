using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Quaternion rot = transform.rotation;
        rot.eulerAngles = new Vector3(0, 0, Random.Range(0, 359));
        transform.rotation = rot;
    }

}
