using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField] private GameObject GameOverScreen;
    public void GameOver()
    {
        GameOverScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
