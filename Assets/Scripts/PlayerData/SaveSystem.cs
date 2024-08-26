using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public PlayerData playerData;
    public bool isSaved;
    string path;

    void Awake(){
        isSaved = false;
        // instance = this;
        if(instance != null && instance != this) Destroy(gameObject);
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
            path = Application.persistentDataPath + "/player.fun";
            LoadPlayer();
        }
    }

    public void ResetAndPlayFromScratch(){
        playerData = new PlayerData();
        SavePlayer(); LoadPlayer();
    }

    public void LoadPlayer(){
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            //load from file if existed
            playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
        } else {
            Debug.Log("save file not found, this is the first time the app is run");
            //create a new player data if not existed
            playerData = new PlayerData();
            //SavePlayer();
        }
    }

    public void SavePlayer(){
        isSaved = true;
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    void OnApplicationQuit()
    {
        SavePlayer();
    }

    public void AddCoins(int c){
        playerData.coins += c;
        if(SceneManager.GetActiveScene().name == "Menu")
            GameEvents.OnCoinsChanged(playerData.coins);
        //SavePlayer();
    }

    public void UpdateHighestScore(int hs){
        if(playerData.highestScore < hs)
            playerData.highestScore = hs;
        //SavePlayer();
    }

    public void EquipCar(int car){
        playerData.curCarID = car;
        //SavePlayer();
    }
}
