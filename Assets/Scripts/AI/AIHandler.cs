using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    public float minSpeed = 4, maxSpeed = 8;
    [SerializeField] CarHandler carHandler;

    //check if there is a car ahead of this car, then steer left/right/stop
    [SerializeField] LayerMask otherCarsLayerMask;
    [SerializeField] BoxCollider boxCollider;
    RaycastHit[] raycastHits = new  RaycastHit[1];
    bool isCarAhead = false;
    //int laneIndex;
    WaitForSeconds waitFor = new WaitForSeconds(.2f);

    void Awake(){
        if(CompareTag("Player")){
            Destroy(this);
            return;
        }
    }
    void OnEnable(){
        carHandler.maxForwardVelocity = Random.Range(minSpeed, maxSpeed);
        //laneIndex = Random.Range(0, Utils.CarLanes.Length);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateLessOftenCO());
    }

    // Update is called once per frame
    void Update()
    {
        float steerInput = 0;
        // float desiredPositionX = Utils.CarLanes[laneIndex];
        // float diff = desiredPositionX - transform.position.x;
        // if(Mathf.Abs(diff) > 0.05f) steerInput = diff;
        // steerInput = Mathf.Clamp(steerInput, -1f, 1f);

        if(isCarAhead) carHandler.SetInput(new Vector2(steerInput, -1));
        carHandler.SetInput(new Vector2(steerInput, 1));
    }

    IEnumerator UpdateLessOftenCO(){
        while(true){
            isCarAhead = CheckIfOtherCarIsAhead();
            yield return waitFor;
        }
    }

    //check if there is a car ahead of this car, then steer left/right/stop
    bool CheckIfOtherCarIsAhead(){
        boxCollider.enabled = false;
        //generate a small box of this car and check if it collides other cars
        int numberOsHits = Physics.BoxCastNonAlloc(
            transform.position, Vector3.one, transform.forward,
            raycastHits, Quaternion.identity, 4, otherCarsLayerMask
        );
        boxCollider.enabled = true;
        return numberOsHits > 0;
    }
}
