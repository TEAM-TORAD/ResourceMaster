using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static GameObject startPanel;
    private void Awake()
    {
        if(startPanel == null)
        {
            startPanel = GameObject.FindGameObjectWithTag("StartPanel");
        }
        else
        {
            Destroy(this);
        }
    }
    public void Create()
    {
        startPanel.SetActive(false);
        SceneManager.LoadScene("Register", LoadSceneMode.Additive);
    }
    public void Login()
    {
        startPanel.SetActive(false);
        SceneManager.LoadSceneAsync("Login", LoadSceneMode.Additive);
    }
}
