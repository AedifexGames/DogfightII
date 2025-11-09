using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadScene(string name)
    {
        if (SceneLoader.instance == null) return;
        SceneLoader.instance.ChangeScene(name);
    }

    public void LoadScene(int n)
    {
        if (SceneLoader.instance == null) return;
        SceneLoader.instance.ChangeScene(n);
    }
}
