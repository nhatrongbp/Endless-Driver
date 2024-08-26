using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;
    public GameObject panelPause;

    public void TogglePauseScreen(){
        panelPause.SetActive(!panelPause.activeInHierarchy);
        if(panelPause.activeInHierarchy){
            Time.timeScale = 0;
        }
        Time.timeScale = panelPause.activeInHierarchy ? 0 : 1;
    }

    public void LoadScene(string str){
        Time.timeScale = 1;
        // SceneManager.LoadScene(str);
        levelLoader.LoadLevel(str);
    }
}
