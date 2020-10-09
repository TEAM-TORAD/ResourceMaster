using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public int regularDamage = 15, powerDamage = 25;
    private int attackValue;
    // Start is called before the first frame update
    void Start()
    {
        attackValue = regularDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider c)
    {
        if(c.attachedRigidbody != null)
        {
            if (c.attachedRigidbody.tag == "ExplodingNPC")
            {
                Debug.Log("Player hit Exploding NPC");
                c.attachedRigidbody.GetComponent<ExplodingNPCController>().TakeDamage(attackValue);
                Debug.Log(c.attachedRigidbody.GetComponent<Health>().health);
            }
            else Debug.Log(c.attachedRigidbody.tag);
        }
        
    }
}
