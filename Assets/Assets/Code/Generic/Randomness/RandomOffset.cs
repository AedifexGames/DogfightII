using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOffset : MonoBehaviour
{
    public float xOffsetMaxRadius = .1f;

    public float yOffsetMaxRadius = .1f;

    public float zOffsetMaxRadius = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(Random.Range(-xOffsetMaxRadius, xOffsetMaxRadius), Random.Range(-yOffsetMaxRadius, yOffsetMaxRadius), Random.Range(-zOffsetMaxRadius, zOffsetMaxRadius));
    }
}
