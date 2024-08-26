using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FeverState{
    FeverCharging, FeverReady, FeverActive
}

public class Fever : MonoBehaviour
{
    public Button button;
    public Image feverIamge, circleImage;
    public Color startColor, endColor;
    public float blinkSpeed = 2f, chargingTime = 10f, duration = 5f;
    float countTime;
    FeverState feverState;
    IEnumerator blinkEffectIE = null;

    // Start is called before the first frame update
    void Start()
    {
        ChargeFever();
    }

    // Update is called once per frame
    void Update()
    {
        if(countTime < Mathf.Max(chargingTime, duration))
            countTime += Time.deltaTime;
    }

    void ChargeFever(){
        GameEvents.OnFeverEnd();
        if(blinkEffectIE != null) {
            StopCoroutine(blinkEffectIE);
            circleImage.color = startColor;
            feverIamge.color = new Color(255,255,255,32);
        }
        feverState = FeverState.FeverCharging;
        button.interactable = false;
        circleImage.fillAmount = 0;

        countTime = 0f;
        StartCoroutine(ChargeFeverCO());
    }

    IEnumerator ChargeFeverCO(){
        while(countTime < chargingTime){
            circleImage.fillAmount = countTime/chargingTime;
            yield return null;
        }
        circleImage.fillAmount = 1f;
        feverState = FeverState.FeverReady;
        button.interactable = true;
        //TODO: play some cool effects to notify that the fever is ready
        blinkEffectIE = BlinkEffectCO();
        StartCoroutine(blinkEffectIE);
    }

    IEnumerator BlinkEffectCO(){
        while(true){
            feverIamge.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
            circleImage.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
            yield return null;
        }
    }

    public void ActivateFever(){
        if(feverState == FeverState.FeverReady){
            feverState = FeverState.FeverActive;
            countTime = 0f;
            GameEvents.OnFeverBegin();
            StartCoroutine(ActivateFeverCO());
        }
    }

    IEnumerator ActivateFeverCO(){
        while(countTime < duration){
            circleImage.fillAmount = (duration - countTime)/duration;
            yield return null;
        }
        feverState = FeverState.FeverCharging;
        button.interactable = false;
        ChargeFever();
    }
}
