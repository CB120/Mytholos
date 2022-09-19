using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    public string sceneName = "ArenaEddie";
    
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
