using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    public Vector2 minScale;
    public Vector2 maxScale;
    public bool maintainRatio;
    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(minScale.x, maxScale.x);
        float y = Random.Range(minScale.y, maxScale.y);
        if (maintainRatio)
        {
            float ratio = transform.localScale.x / transform.localScale.y;
            x = y * ratio;
        }
        transform.localScale = new Vector3(x, y);
    }
}
