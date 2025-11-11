using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AXGlobal : MonoBehaviour
{
    private bool existed;
    // Start is called before the first frame update
    private void Awake()
    {
        AXGlobal[] globals = FindObjectsByType<AXGlobal>(FindObjectsSortMode.None);
        for (int i = 0; i < globals.Length; i++)
        {
            if (globals[i].gameObject.name == gameObject.name && globals[i] != this)
            {
                if (!existed)
                { Destroy(gameObject); return;  }
            }
        }
        existed = true;
        DontDestroyOnLoad(gameObject);
    }

}