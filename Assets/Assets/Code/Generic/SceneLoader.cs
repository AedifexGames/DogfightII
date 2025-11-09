using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    public void ChangeScene(int s)
    {
        SceneManager.LoadScene(s);
    }

    public void ChangeScene(string s)
    {
        SceneManager.LoadScene(s);
    }
}
