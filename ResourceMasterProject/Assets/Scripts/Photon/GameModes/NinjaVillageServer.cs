using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class NinjaVillageServer : MonoBehaviourPun, IPunObservable
{

    private float timer;
    bool hasPlayerSpawned = false;
    public float spawnTime = 3;
    public Transform spawnPoint;
    private ServerEvents serverEvents;


    public GameObject lobbyCamera;
    public TMP_Text spawnCountdown;
    // Start is called before the first frame update
    void Start()
    {
        serverEvents = GetComponent<ServerEvents>();
        if(photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            lobbyCamera.SetActive(true);
            timer = spawnTime;
            hasPlayerSpawned = false;
        }
    }
    private void ChangeCursorVisibility()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeCursorVisibility();
            }
        }
        // If the player hasn't spawned, count down and spawn the player when the timer reaches 0.
        if(!hasPlayerSpawned)
        {
            timer -= Time.deltaTime;
            spawnCountdown.text = "Spawning in " + (Mathf.FloorToInt(timer) + 1).ToString();
        }
        if (timer <= 0 && !hasPlayerSpawned)
        {
            //instantiate a prefab from the Resourses folder
            PhotonNetwork.Instantiate("PlayerOrig", spawnPoint.position, Quaternion.identity, 0);
            timer = 0;
            lobbyCamera.SetActive(false);
            spawnCountdown.gameObject.SetActive(false);
            hasPlayerSpawned = true;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {

        }
        else if(stream.IsReading)
        {

        }
    }
}
