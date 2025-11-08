using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = new Sprite[0];
    // Start is called before the first frame update
    private void Start()
    {
        if (sprites.Length == 0) return;

        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

}
