using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MPManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1.4";
    public static GameObject multiplayerPanel;
    public GameObject[] enableObjectsOnConnect;
    public GameObject[] disableObjectsOnConnect;
    string serverName = "";
    // Start is called before the first frame update
    void Start()
    {
        if(multiplayerPanel == null) multiplayerPanel = GameObject.FindGameObjectWithTag("MultiplayerPanel");
        multiplayerPanel.SetActive(false);
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        foreach (GameObject o in enableObjectsOnConnect)
        {
            o.SetActive(true);
        }
        foreach (GameObject o in disableObjectsOnConnect)
        {
            o.SetActive(false);
        }
        Debug.Log("We are now connected to Photon.");
        //PhotonNetwork.ConnectToRegion("au");
        //PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void JoinNinjaVillageServer()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + "\n" + message);
        CreateNinjaVillageServer();
    }
    /*
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(serverName);
    }*/
    public void CreateNinjaVillageServer()
    {
        //Set the name of the server to be loaded once the player has joined a room
        serverName = "VillageGreyBox";
        Debug.Log("Creating server...");
        PhotonNetwork.AutomaticallySyncScene = true;
        RoomOptions RO = new RoomOptions { MaxPlayers = 10, IsOpen = true, IsVisible = true};
        PhotonNetwork.CreateRoom("NinjaVillage", RO, TypedLobby.Default);

    }

    // New system
    public void OpenServersList()
    {
        multiplayerPanel.SetActive(false);
        SceneManager.LoadSceneAsync("ServersList", LoadSceneMode.Additive);
    }
    public void CreateServer()
    {
        multiplayerPanel.SetActive(false);
        SceneManager.LoadSceneAsync("CreateServer", LoadSceneMode.Additive);
    }
}
