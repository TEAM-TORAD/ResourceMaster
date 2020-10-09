using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private Animator anim;
    private Stamina stamina;
    public bool isBlocking;

    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        stamina = GetComponent<Stamina>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(stamina.stamina > 5) // Only possible to start blocking if the player has some stamina 
            {
                anim.SetBool("Block", true);
                stamina.usingStamina = true;
                isBlocking = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) || stamina.stamina <= 0)
        {
            anim.SetBool("Block", false);
            stamina.usingStamina = false;
            isBlocking = false;
        }
    }
}
