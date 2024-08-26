using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Vector2 input = Vector2.zero;
    void Awake(){
        if(CompareTag("CarAI")){
            Destroy(this);
            return;
        }
    }

#if (UNITY_EDITOR)
//controll by 'WASD'
    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        //input.y = 1;
        GameEvents.OnPlayerInput(input.x, input.y);
    }
#endif

    public void OnSteerLeftPressedDown(){ 
        input.x = -1;
        GameEvents.OnPlayerInput(input.x, input.y);
    }

    public void OnSteerRightPressedDown(){ 
        input.x = 1;
        GameEvents.OnPlayerInput(input.x, input.y);
    }
    public void OnSteerPressedUp(){ 
        input.x = 0; 
        GameEvents.OnPlayerInput(input.x, input.y);
    }

    public void OnAcceleratePressedDown(){
        input.y = 1; 
        GameEvents.OnPlayerInput(input.x, input.y);
    }

    public void OnAcceleratePressedUp(){
        input.y = 0; 
        GameEvents.OnPlayerInput(input.x, input.y);
    }

    public void OnBrakePressedDown(){
        input.y = -1; 
        GameEvents.OnPlayerInput(input.x, input.y);
    }
}
