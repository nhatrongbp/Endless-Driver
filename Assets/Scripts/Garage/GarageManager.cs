using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GarageManager : MonoBehaviour
{
    [SerializeField] Transform cylinder;
    [SerializeField] Transform[] carList;
    [SerializeField] CarConfig[] carConfigs;
    public float rotateSpeed;
    int curCarID = 0;
    public Slider sliderAcc, sliderMaxSpeed, sliderSteer, sliderBrake, sliderFever;
    public TMP_Text carNameText, priceText, equipText;
    public GameObject buttonBuy, buttonEquip;

    // void OnEnable(){
    //     carList[curCarID].gameObject.SetActive(false);
    //     carList[curCarID].localPosition = new Vector3(0, 1.05f, 0);
    // }

    // Start is called before the first frame update
    void OnEnable()
    {
        carList[curCarID].gameObject.SetActive(false);
        //TODO: get curCarID from saved file
        curCarID = SaveSystem.instance.playerData.curCarID;
        carList[curCarID].localPosition = new Vector3(0, 1.05f, 0);
        carList[curCarID].gameObject.SetActive(true);
        UpdateCarStat();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cylinder.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void PrevCar(){
        carList[curCarID--].gameObject.SetActive(false);
        if(curCarID < 0) curCarID = carList.Length - 1;
        carList[curCarID].localPosition = new Vector3(0, 1.05f, 0);
        carList[curCarID].gameObject.SetActive(true);
        UpdateCarStat();
    }

    public void NextCar(){
        carList[curCarID++].gameObject.SetActive(false);
        if(curCarID >= carList.Length) curCarID = 0;
        carList[curCarID].localPosition = new Vector3(0, 1.05f, 0);
        carList[curCarID].gameObject.SetActive(true);
        UpdateCarStat();
    }

    void UpdateCarStat(){
        sliderAcc.value = carConfigs[curCarID].accelerationMultiplier;
        sliderBrake.value = carConfigs[curCarID].brakeMultipler;
        sliderFever.value = carConfigs[curCarID].feverDuration;
        sliderMaxSpeed.value = carConfigs[curCarID].maxSteerVelocity;
        sliderSteer.value = carConfigs[curCarID].steerMultiplier;
        carNameText.text = carConfigs[curCarID].carName;
        priceText.text = carConfigs[curCarID].price.ToString();

        if(SaveSystem.instance.playerData.curCarID == curCarID){
            buttonBuy.SetActive(false); 
            buttonEquip.SetActive(true);
            buttonEquip.GetComponent<Button>().interactable = false;
            equipText.text = "Equipped";
        } else {
            bool ownedThisCar = SaveSystem.instance.playerData.ownedCars.Contains(curCarID);
            buttonBuy.SetActive(!ownedThisCar); 
            buttonEquip.SetActive(ownedThisCar);
            buttonEquip.GetComponent<Button>().interactable = true;
            equipText.text = "Equip";
        }
    }

    public void BuyCar(){
        if(SaveSystem.instance.playerData.coins >= carConfigs[curCarID].price){
            SaveSystem.instance.AddCoins(- carConfigs[curCarID].price);
            SaveSystem.instance.playerData.ownedCars.Add(curCarID);
            buttonBuy.SetActive(false); 
            buttonEquip.SetActive(true);
            buttonEquip.GetComponent<Button>().interactable = true;
            equipText.text = "Equip";
        }
    }
    
    public void EquipCar(){
        SaveSystem.instance.playerData.curCarID = curCarID;
        buttonEquip.GetComponent<Button>().interactable = false;
        equipText.text = "Equipped";
    }
}
