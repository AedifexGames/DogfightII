using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlip : MonoBehaviour
{
    public bool x;
    public bool y;
    // Start is called before the first frame update
    void Start()
    {
        if (x && FlipCoin())
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (y && FlipCoin())
        {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }

    public bool FlipCoin()
    {
        int flip = Random.Range(0, 2);
        if (flip == 0) return false;
        return true;
    }
}
