using TMPro;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private TextMeshProUGUI ScoreText;
    public void GameOver(float score)
    {
        GameOverScreen.SetActive(true);
        ScoreText.text = "You made it " + (int)score + " light years";
        Time.timeScale = 0.0f;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
    }
}
