using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class ServerEvents : MonoBehaviourPun
{
    public Transform serverMessagesContainer;
    public GameObject serverMessagePrefab;
    private string playerName;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            GetPlayerProfile();
        }
    }
    void GetPlayerProfile()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, result => 
        {
            playerName = result.AccountInfo.TitleInfo.DisplayName;
            PlayerJoinedServer(playerName);
        }, error => 
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }

    public void PlayerJoinedServer(string playerName)
    {

        GameObject newText = Instantiate(serverMessagePrefab, serverMessagesContainer);
        newText.GetComponent<TMP_Text>().text = playerName + " has joined the server.";
    }
}
