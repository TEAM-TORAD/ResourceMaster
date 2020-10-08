using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class WeaponManager : MonoBehaviourPun
{
    public GameObject ninjato;
    public KeyCode primary;

    public bool online = true;

    public bool primaryEquiped;
    // Start is called before the first frame update
    void Start()
    {
        if(!online || photonView.IsMine)
        {
            primaryEquiped = false;
            ninjato.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!online || photonView.IsMine)
        {
            if (Input.GetKeyDown(primary))
            {
                if (primaryEquiped)
                {
                    PrimaryDeactivated();
                }

                else
                {
                    PrimaryActive();
                }

            }
        }
            

    }

    void PrimaryActive()
    {
        ninjato.SetActive(true);
        primaryEquiped = true;
    }
    void PrimaryDeactivated()
    {
        ninjato.SetActive(false);
        primaryEquiped = false;    
    }
}
