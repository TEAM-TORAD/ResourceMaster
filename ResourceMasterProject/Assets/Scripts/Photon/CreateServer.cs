using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateServer : MonoBehaviourPunCallbacks
{
    public string[] maps;


    public TMP_Dropdown maxPlayers;
    public TMP_Dropdown mapsDropdown;
    private int[] maxPlayersReference;
    private string[] mapsReference;

    public TMP_InputField serverName;


    public int setServerLimit;
    private void Start()
    {
        maxPlayersReference = new int[setServerLimit];
        mapsReference = new string[maps.Length];

        for(int i = 1; i <= setServerLimit; ++i)
        {
            maxPlayers.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
            maxPlayersReference[i - 1] = i;
        }
        for(int i = 0; i < maps.Length; ++i)
        {
            mapsDropdown.options.Add(new TMP_Dropdown.OptionData(maps[i]));
            mapsReference[i] = maps[i];

        }
    }
    public void CreateServerCustom()
    {
        string thisServerName = "";
        if (serverName.text == "") thisServerName = "New Server";
        else thisServerName = serverName.text;


        PhotonNetwork.AutomaticallySyncScene = true;
        RoomOptions options = new RoomOptions();


        options.MaxPlayers = (byte) maxPlayersReference[maxPlayers.value];
        options.IsOpen = true;
        options.IsVisible = true;
        //Set a custom server-name
        //options.CustomRoomProperties.Add("SN", thisServerName);

        PhotonNetwork.CreateRoom(mapsReference[mapsDropdown.value], options);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(mapsReference[mapsDropdown.value]);
        //SceneManager.LoadScene(mapsReference[mapsDropdown.value]);
    }
    public override void OnConnectedToMaster()
    {
        //PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }

    public void CloseServerCreate()
    {
        SceneManager.UnloadSceneAsync("CreateServer");
        MPManager.multiplayerPanel.SetActive(true);
    }
}
