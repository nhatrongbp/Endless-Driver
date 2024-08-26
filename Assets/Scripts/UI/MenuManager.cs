using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Text textCoin, context;
    public GameObject panelGarage, panelMenu, panelEndless, panelSetting;
    [SerializeField] LevelLoader levelLoader;
    // Start is called before the first frame update
    void Start()
    {
        textCoin.text = SaveSystem.instance.playerData.coins.ToString();
    }

    // Update is called once per frame
    void OnDisable()
    {
        GameEvents.OnCoinsChanged -= UpdateTextCoin;
    }
    void OnEnable()
    {
        GameEvents.OnCoinsChanged += UpdateTextCoin;
    }

    public void AddCoins(int coins=50){
        SaveSystem.instance.AddCoins(coins);
    }

    void UpdateTextCoin(int coins){
        textCoin.text = coins.ToString();
    }

    void HideAllPanels(){
        panelGarage.SetActive(false);
        panelMenu.SetActive(false);
        panelEndless.SetActive(false);
    }

    public void OnBackButtonPressed(){
        HideAllPanels();
        panelMenu.SetActive(true);
        context.text = "MENU";
    }

    public void OnGarageButtonPressed(){
        HideAllPanels();
        panelGarage.SetActive(true);
        context.text = "GARAGE";
    }

    public void OnEndlessButtonPressed(){
        HideAllPanels();
        panelEndless.SetActive(true);
        context.text = "ENDLESS";
    }

    public void LoadScene(string str){
        // SceneManager.LoadScene(str);
        levelLoader.LoadLevel(str);
    }

    public void TogglePanelSetting(){
        panelSetting.SetActive(!panelSetting.activeInHierarchy);
    }
}
