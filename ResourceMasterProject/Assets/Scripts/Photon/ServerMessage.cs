using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServerMessage : MonoBehaviour
{
    public float timeUntilRemove = 4.0f;
    public float fadeAfter = 3.0f;
    private float timer = 0.0f;
    private float startAplha;
    private TMP_Text thisText;
    // Start is called before the first frame update
    void Start()
    {
        thisText = GetComponent<TMP_Text>();
        startAplha = thisText.alpha;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= fadeAfter)
        {
            float lerpedAlpha = thisText.alpha - ((timeUntilRemove - fadeAfter) * startAplha * Time.deltaTime);
            if (lerpedAlpha < 0) lerpedAlpha = 0;
            thisText.alpha = lerpedAlpha;
        }
        if(timer >= timeUntilRemove)
        {
            Destroy(this.gameObject);
        }
    }
}
