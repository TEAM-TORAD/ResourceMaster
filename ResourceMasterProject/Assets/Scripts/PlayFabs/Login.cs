using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    public GameObject loginPanel;
    public TMP_InputField username;
    public TMP_InputField password;
    public Button submissionButton;
    public string levelToLoad;

    private void Start()
    {
        username.ActivateInputField();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(username.isFocused)
            {
                password.ActivateInputField();
            }
            else
            {
                username.ActivateInputField();
            }
        }
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) && submissionButton.IsActive() ) 
        {
            LoginUser();
        }
    }
    public void LoginUser()
    {
        submissionButton.interactable = false;
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
        request.Username = username.text;
        request.Password = password.text;

        PlayFabClientAPI.LoginWithPlayFab(request, result => {
            GetVerificationUserData(result.PlayFabId);
            loginPanel.SetActive(false);
        }, error => {
            submissionButton.interactable = true;
            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Login Error", error.ErrorMessage));
        });
    }
    public void OpenResetPasswordPanel()
    {
        loginPanel.SetActive(false);
        SceneManager.UnloadSceneAsync("Login");
        SceneManager.LoadSceneAsync("ResetPassword", LoadSceneMode.Additive);
    }
    public void CloseLoginPanel()
    {
        UIManager.startPanel.SetActive(true);
        SceneManager.UnloadSceneAsync("Login");
    }

    void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void GetVerificationUserData(string myPlayFabeId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabeId,
            Keys = null
        }, result => {
            if (result.Data == null || !result.Data.ContainsKey("verified")) 
            {
                UIManager.startPanel.SetActive(true);
                Alerts a = new Alerts();
                StartCoroutine(a.LoadSceneAsync("Error!", "No player verification data on server! Please contact admin at team.torad@gmail.com"));
                SceneManager.UnloadSceneAsync("Login");
                PlayerPrefs.DeleteAll();
            }
            else
            {
                // Send unverified user to verification scene
                if(result.Data["verified"].Value == "false")
                {
                    SceneManager.UnloadSceneAsync("Login");
                    SceneManager.LoadSceneAsync("VerifyAccount", LoadSceneMode.Additive);
                }
                else
                {
                    SceneManager.UnloadSceneAsync("Login");
                    SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
                }
            }
        }, (error) => {

            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Error checking account verification!", error.ErrorMessage));
        });
    }

    void OnGetStatistics(GetPlayerStatisticsResult result) // Not used, kept for reference on using player stats
    {
        foreach (var eachStat in result.Statistics)
        {
            if(eachStat.StatisticName == "verified")
            {
                if(eachStat.Value == 0)
                {
                    SceneManager.UnloadSceneAsync("Login");
                    SceneManager.LoadSceneAsync("VerifyAccount", LoadSceneMode.Additive);
                }
                else if(eachStat.Value == 1)
                {
                    SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
                }
            }
        }
    }

}
