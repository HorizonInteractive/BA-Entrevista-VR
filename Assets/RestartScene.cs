using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    private bool restarting = false;

    public void ReloadScene()
    {
        if(Time.timeSinceLevelLoad > 3f && !restarting)
        {
            restarting = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
