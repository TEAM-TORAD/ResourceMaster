using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Alerts newAlert = new Alerts();
        StartCoroutine(newAlert.LoadSceneAsync("Testing Title", "Testing Message"));
    }
}
