using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement
{
    public void LoadMainScene() 
    {
        LoadingUI.LoadScene("SampleScene");
    }

    public void LoadBossScene() 
    {
        LoadingUI.LoadScene("Boss");
    }

    public void LoadEndScene() 
    {
        LoadingUI.LoadScene("End");
    }

    public string GetScene() 
    {
        return SceneManager.GetActiveScene().name;
    }
}
