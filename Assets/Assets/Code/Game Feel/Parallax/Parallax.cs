using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public float length;
    private float startPos;
    public float parallaxEffect;
    private GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
