using UnityEngine;

public class RandomColor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color color = Color.white;
        int rand = Random.Range(0, 10);
        switch(rand)
        {
            case 0:
                color = Color.white;
                break;
            case 1:
                color = Color.red;
                break;
            case 2:
                color = Color.green;
                break;
            case 3:
                color = Color.blue;
                break;
            case 4:
                color = Color.yellow;
                break;
            case 5:
                color = Color.purple;
                break;
            case 6:
                color = Color.orange;
                break;
            default:
                color = Color.white;
                break;
        }
        GetComponent<SpriteRenderer>().color = color;
    }
}
