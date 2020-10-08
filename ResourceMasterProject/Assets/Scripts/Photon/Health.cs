using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;

public class Health : MonoBehaviourPun
{
    public int health = 100;
    public int maxHealth = 100;
    public bool healthCriticalRunning = false, online = false;
    //public int lives;
    public GameObject healthImagePrefab;
    private Transform healthPanel;
    private Image healthImage, healthImageBG;
    private Color lerpedColor;
    public Color greyColor, redColor;

    void Start()
    {
        if(!online || photonView.IsMine)
        {
            if(transform.tag == "Player")
            {
                healthPanel = GameObject.FindGameObjectWithTag("HealthPanel").transform;
                GameObject healthImageObject = Instantiate(healthImagePrefab, healthPanel);
                healthImage = healthImageObject.transform.GetChild(1).GetComponent<Image>();
                healthImageBG = healthImageObject.transform.GetChild(0).GetComponent<Image>();
            }
            
        }
    }
    private void Update()
    {
        if (!online || photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                //Take Damage (testing purposes)
                TakeDamage(10);
            }
            if (healthCriticalRunning)
            {
                lerpedColor = Color.Lerp(greyColor, redColor, Mathf.PingPong(Time.time, 1.25f));
                healthImageBG.color = lerpedColor;
            }
        }
    }
    public void ResetHealth(int _health, int _maxHealth)
    {
        health = _health;
        maxHealth = _maxHealth;
        if(transform.tag == "Player")
        {
            healthImage.fillAmount = (float)health / maxHealth;
            if ((float)health / maxHealth < 0.2f) healthCriticalRunning = false;
            else healthCriticalRunning = true;
        }
    }
    
    public void TakeDamage(int value)
    {
        health -= value;
        if(health <= 0)
        {
            health = 0;
            // Death effect
            if(transform.tag == "ExplodingNPC")
            {
                transform.GetComponent<ExplodingNPCController>().Die();
            }
        }
        else
        {
            // Take damage
            Debug.Log(transform.name + " took " + value + " in damage.");
            if(transform.tag == "Player")
            {
                if ((float)health / maxHealth < 0.2f)
                {
                    healthCriticalRunning = true;
                }
            }
        }
        if(transform.tag == "Player")
        {
            healthImage.fillAmount = (float)health / maxHealth;
        }
    }
}
