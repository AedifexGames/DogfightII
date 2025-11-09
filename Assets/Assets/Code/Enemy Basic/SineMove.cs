using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SineMove : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private float amplitude;
    [SerializeField] private float freq;
    [SerializeField] private float speed;
    [SerializeField] private float rotMagnitude;
    private float initialHeight;
    private void Start()
    {
        initialHeight = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position + (Vector3.right * speed * Time.deltaTime);
        newPos.y = initialHeight;
        newPos.y += amplitude * Mathf.Sin(freq * newPos.x + offset);
        transform.position = newPos;
        //rotate to tangent
        transform.rotation = Quaternion.Euler(0,0,rotMagnitude * Mathf.Cos(newPos.x + offset) / Mathf.Sqrt(1 + (Mathf.Pow(Mathf.Cos(newPos.x + offset),2f))));
    }
}
