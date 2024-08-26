using System;
using TMPro;
using DG.Tweening;
using UnityEngine;

public class GameStatManager : MonoBehaviour
{
    public TMP_Text coinText, speedText, distanceText;
    public TMP_Text resultText, resultScoreText, highScoreText;
    public CanvasGroup panelGameOver;
    float distance = 0, maxSpeed = 0, coins = 0;
    // Start is called before the first frame update
    void OnDisable()
    {
        GameEvents.OnPlayerPositionZChanged -= UpdateDistanceText;
        GameEvents.OnSpeedChanged -= UpdateSpeedText;
        GameEvents.OnGameOver -= ShowGameOver;
    }

    // Update is called once per frame
    void OnEnable()
    {
        GameEvents.OnPlayerPositionZChanged += UpdateDistanceText;
        GameEvents.OnSpeedChanged += UpdateSpeedText;
        GameEvents.OnGameOver += ShowGameOver;
    }

    void UpdateDistanceText(float z){
        distance = z;
        distanceText.text = String.Format("{0:0.00} km", z/1000);
    }

    void UpdateSpeedText(float z){
        maxSpeed = Mathf.Max(maxSpeed, z);
        speedText.text = ((int)z).ToString() + " km/h";
        if(30 < z && z < 45) coins += Time.deltaTime;
        else if(45 < z && z < 60) coins += 2 * Time.deltaTime;
        else if(60 < z && z < 75) coins += 3 * Time.deltaTime;
        else if(75 < z && z < 90) coins += 4 * Time.deltaTime;
        else if(90 < z) coins += 5 * Time.deltaTime;
        coinText.text = ((int)coins).ToString();
    }

    void ShowGameOver(){
        if(!panelGameOver.gameObject.activeInHierarchy){
            AudioManager.instance.PlaySound("Explode");
            
            panelGameOver.gameObject.SetActive(true);
            panelGameOver.alpha = 0f;
            panelGameOver.DOFade(1f, 2f).SetEase(Ease.InSine).SetDelay(2f);

            resultScoreText.GetComponent<RectTransform>().localScale = Vector3.one * 512;
            resultScoreText.GetComponent<RectTransform>().DOScale(1f, .5f).SetDelay(4f);
            resultScoreText.alpha = 0f;
            resultScoreText.DOFade(1f, .5f).SetDelay(4f);
            resultScoreText.text = coinText.text;
            resultText.text = distanceText.text + "\n0\n" 
                + ((int)maxSpeed).ToString() + " km/h\n-\n-";

            SaveSystem.instance.AddCoins((int)coins);

            //update high score if needed
            if(coins > SaveSystem.instance.playerData.highestScore){
                SaveSystem.instance.playerData.highestScore = (int)coins;
                highScoreText.text = "new high score !!";
            } else {
                highScoreText.text = "high score: " + SaveSystem.instance.playerData.highestScore.ToString();
            }
        }
    }
}
