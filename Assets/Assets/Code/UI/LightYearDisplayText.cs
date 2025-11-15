using TMPro;
using UnityEngine;

public class LightYearDisplayText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private Transform _scoreKeeper;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = "" + (int)_scoreKeeper.position.x;
    }
}
