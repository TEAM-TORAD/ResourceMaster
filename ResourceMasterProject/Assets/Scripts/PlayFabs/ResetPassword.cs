using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResetPassword : MonoBehaviour
{
    public TMP_InputField email;
    public Button submissionButton;

    private void Start()
    {
        email.ActivateInputField();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) && submissionButton.IsActive())
        {
            RequestPassword();
        }
    }
    public void CloseResetPasswordPanel()
    {
        UIManager.startPanel.SetActive(true);
        SceneManager.UnloadSceneAsync("ResetPassword");
    }
    public void RequestPassword()
    {
        submissionButton.interactable = false;
        if (email.text != "")
        {
            var request = new SendAccountRecoveryEmailRequest();
            request.TitleId = "B7021";
            request.Email = email.text;
            PlayFabClientAPI.SendAccountRecoveryEmail(request, success =>
            {
                Alerts alert = new Alerts();
                StartCoroutine(alert.LoadSceneAsync("Email sent!", "An email with instructions on how to reset your password has been sent to " + email.text));
                UIManager.startPanel.SetActive(true);
                SceneManager.UnloadSceneAsync("ResetPassword");
            }, error =>
            {
                Alerts alert = new Alerts();
                StartCoroutine(alert.LoadSceneAsync("Error!", error.ErrorMessage));
                submissionButton.interactable = true;
            });
        }
        else
        {
            Alerts alert = new Alerts();
            StartCoroutine(alert.LoadSceneAsync("Error!", "You need to enter your email!"));
            submissionButton.interactable = true;
        }
    }
}
