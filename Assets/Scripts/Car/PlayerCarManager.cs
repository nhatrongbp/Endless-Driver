using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCarManager : MonoBehaviour
{
    public Transform[] playerCarPF;
    public CinemachineVirtualCamera virtualCamera;
    Transform playerCarInstance;
    WaitForSeconds _waitFor = new WaitForSeconds(.5f);

    // Start is called before the first frame update
    void Awake()
    {
        int carID = SaveSystem.instance.playerData.curCarID;
        playerCarInstance = Instantiate(playerCarPF[carID], transform);
        virtualCamera.Follow = playerCarInstance;
    }

    void Start(){
        StartCoroutine(UpdateLessOftenCO());
    }

    // Update is called once per frame
    IEnumerator UpdateLessOftenCO()
    {
        while(true){
            //GameEvents.OnTestObserver(2001);
            GameEvents.OnPlayerPositionZChanged(playerCarInstance.position.z);
            yield return _waitFor;
        }
    }
}
