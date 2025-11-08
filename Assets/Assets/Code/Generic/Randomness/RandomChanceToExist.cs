using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChanceToExist : MonoBehaviour
{
    [Range(0f, 1f)]
    public float chanceToNotExist = .5f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0f, 1f) < chanceToNotExist)
        {
            gameObject.SetActive(false);
        }
    }
}
