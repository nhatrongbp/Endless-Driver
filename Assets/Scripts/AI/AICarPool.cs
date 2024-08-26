using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarPool : MonoBehaviour
{
    public GameObject[] aiCarPrefabs;
    //[SerializeField] Transform _playerCarTransform;
    GameObject[] _aiCarPool;
    //WaitForSeconds _waitFor = new WaitForSeconds(.5f);
    float _timeLastCarSpawned = 0;
    float _lastPlayerPosition = 0;

    //overlapped check
    [SerializeField] LayerMask otherCarsLayerMask;
    Collider[] _overlappedCheckCollider = new Collider[1];

    // Start is called before the first frame update
    void Awake()
    {
        _aiCarPool = new GameObject[aiCarPrefabs.Length * 3];
        int prefabIndex = 0;
        for(int i = 0; i < _aiCarPool.Length; ++i){
            _aiCarPool[i] = Instantiate(aiCarPrefabs[prefabIndex++]);
            _aiCarPool[i].transform.parent = transform;
            _aiCarPool[i].SetActive(false);
            if(prefabIndex >= aiCarPrefabs.Length) prefabIndex = 0;
        }

        //StartCoroutine(UpdateLessOftenCO());
    }

    void OnDisable()
    {
        GameEvents.OnPlayerPositionZChanged -= CleanUpCarsBeyondView;
        GameEvents.OnPlayerPositionZChanged -= SpawnNewCars;
    }

    void OnEnable()
    {
        GameEvents.OnPlayerPositionZChanged += CleanUpCarsBeyondView;
        GameEvents.OnPlayerPositionZChanged += SpawnNewCars;
    }

    // IEnumerator UpdateLessOftenCO(){
    //     while(true){
    //         CleanUpCarsBeyondView();
    //         SpawnNewCars();
    //         yield return _waitFor;
    //     }
    // }

    void CleanUpCarsBeyondView(float z){
        //Debug.Log("CleanUpCarsBeyondView");
        foreach (var aiCar in _aiCarPool)
        {
            if(aiCar.activeInHierarchy){
                //too far ahead
                if(aiCar.transform.position.z - z > 400){
                    aiCar.SetActive(false);
                }

                //too far behind
                if(aiCar.transform.position.z - z < -50){
                    aiCar.SetActive(false);
                }
            }
        }
    }

    void SpawnNewCars(float z)
    {
        //if(Time.time - _timeLastCarSpawned < 1.5f) return;
        if(z - _lastPlayerPosition < 50) return;
        int maxTimesTry = 10;

        int numberOfCarsToSpawn = Random.Range(1, 4);
        while(numberOfCarsToSpawn > 0 && maxTimesTry > 0){
            maxTimesTry--;
            GameObject carToSpawn = GetRandomCarFromPool();

            //Debug.Log("SpawnNewCars");
            Vector3 spawnPosition = new Vector3(
                Utils.CarLanes[Random.Range(0, Utils.CarLanes.Length)], 0, z + 200);

            //check if the spawnPosition is valid:
            //generate a box with the double size of the car size, then check if this box overlap other car
            //this box was named "_overlappedCheckCollider"
            if(Physics.OverlapBoxNonAlloc(
                spawnPosition, new Vector3(1, 1, 4), //Vector3.one * 2, 
                _overlappedCheckCollider, Quaternion.identity, otherCarsLayerMask) > 0)
                    continue;

            carToSpawn.transform.position = spawnPosition;
            carToSpawn.SetActive(true);

            _timeLastCarSpawned = Time.time;
            _lastPlayerPosition = z;

            numberOfCarsToSpawn--;
        }
        if(numberOfCarsToSpawn > 0) Debug.Log("can not spawn more cars, maybe the game is over");
    }

    GameObject GetRandomCarFromPool(){
        int randomIndex = Random.Range(0, _aiCarPool.Length);
        while(_aiCarPool[randomIndex].activeInHierarchy){
            ++randomIndex;
            if(randomIndex >= _aiCarPool.Length) randomIndex = 0;
        }
        return _aiCarPool[randomIndex];
    }

}
