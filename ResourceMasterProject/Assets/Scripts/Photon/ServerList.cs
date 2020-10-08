using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ServerList : MonoBehaviourPunCallbacks
{
    public GameObject serverButtonPrefab;
    public Transform serversHolder;
    private string serverName = "";

    public List<RoomInfo> roomList;
    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }
    
    void ServerStatus()
    {
        Debug.Log("In lobby: " + PhotonNetwork.InLobby + " In Room: " + PhotonNetwork.InRoom);

    }
    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        Debug.Log("Roomlist updated");
        roomList = _roomList;
        ClearRoomList();

        foreach(RoomInfo RI in roomList)
        {
            GameObject newButton = Instantiate(serverButtonPrefab, serversHolder) as GameObject;
            serverName = RI.Name;

            newButton.transform.Find("ServerName").GetComponent<TMP_Text>().text = serverName;
            newButton.transform.Find("Players").GetComponent<TMP_Text>().text = RI.PlayerCount.ToString() + "/" + RI.MaxPlayers.ToString();

            newButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(serverName); });
        }

        base.OnRoomListUpdate(roomList);
    }
    public override void OnConnectedToMaster()
    {
        //When joining lobby the OnRoomListUpdate will fire
        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }

    public void ClearRoomList()
    {
        foreach (Transform t in serversHolder) Destroy(t.gameObject);
    }
    public void JoinRoom(string serverName)
    {
        PhotonNetwork.JoinRoom(serverName);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(serverName);
        base.OnJoinedRoom();
    }

    public void CloseServersList()
    {
        SceneManager.UnloadSceneAsync("ServersList");
        MPManager.multiplayerPanel.SetActive(true);
    }
}
