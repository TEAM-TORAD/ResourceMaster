using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseAlertsScene : MonoBehaviour
{
    public void CloseScene()
    {
        SceneManager.UnloadSceneAsync("Alerts");
    }
}
