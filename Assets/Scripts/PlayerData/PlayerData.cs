using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coins, highestScore;
    public int curCarID;
    public List<int> ownedCars;

    public PlayerData(){
        coins = 0;
        highestScore = 0;
        curCarID = 0;
        ownedCars = new List<int> { 0 };
    }
}
