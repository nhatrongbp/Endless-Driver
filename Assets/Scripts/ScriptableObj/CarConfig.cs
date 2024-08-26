using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarConfig", menuName = "CarConfig")]
public class CarConfig : ScriptableObject
{
    public string carName;
    public int price;
    public float accelerationMultiplier, brakeMultipler;
    public float steerMultiplier, maxSteerVelocity;
    //public float minForwardVelocity, maxForwardVelocity;
    public float rotationAmount;
    public float feverChargingTime, feverDuration;
}
