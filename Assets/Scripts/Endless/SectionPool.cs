using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionPool : MonoBehaviour
{
    public GameObject[] sectionPrefabs;
    GameObject[] _sectionPool = new GameObject[20];
    GameObject[] _sections = new GameObject[10]; //current active sections
    //[SerializeField] Transform _playerCarTransform;
    //WaitForSeconds _waitFor = new WaitForSeconds(.1f);
    const float sectionLength = 120;
    // Start is called before the first frame update
    void Awake()
    {
        //playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform; // dont use Find, use Observer instead
        int prefabIndex = 0;
        for(int i = 0; i < _sectionPool.Length; ++i){
            _sectionPool[i] = Instantiate(sectionPrefabs[prefabIndex++]);
            _sectionPool[i].transform.parent = transform;
            _sectionPool[i].SetActive(false);
            if(prefabIndex >= sectionPrefabs.Length) prefabIndex = 0;
        }

        //init the first sections to the road
        for(int i = 0; i < _sections.Length; ++i){
            GameObject randomSection = GetRandomSectionFromPool();
            randomSection.transform.position = new Vector3(_sectionPool[i].transform.position.x, 0, i*sectionLength);
            randomSection.SetActive(true);

            _sections[i] = randomSection;
        }

        //StartCoroutine(UpdateLessOftenCO());
    }

    // Update is called once per frame
    void OnDisable()
    {
        GameEvents.OnPlayerPositionZChanged -= UpdateSectionPosition;
        // GameEvents.OnTestObserver -= (val) => Debug.Log("received OnTestObserver " + val);
    }

    void OnEnable()
    {
        GameEvents.OnPlayerPositionZChanged += UpdateSectionPosition;
        // GameEvents.OnTestObserver += (val) => Debug.Log("received OnTestObserver " + val);
    }

    // IEnumerator UpdateLessOftenCO(){
    //     while(true){
    //         UpdateSectionPosition(0);
    //         yield return _waitFor;
    //     }
    // }

    void UpdateSectionPosition(float z){
        for(int i = 0; i < _sections.Length; ++i){
            //Check if section is too far behind
            if(_sections[i].transform.position.z - z < -sectionLength){
                Vector3 lastSectionPosition = _sections[i].transform.position;
                _sections[i].SetActive(false);
                _sections[i] = GetRandomSectionFromPool();
                _sections[i].transform.position = new Vector3(
                    lastSectionPosition.x, 0, lastSectionPosition.z + sectionLength * _sections.Length);
                _sections[i].SetActive(true);
            }
        }
    }

    GameObject GetRandomSectionFromPool(){
        int randomIndex = Random.Range(0, _sectionPool.Length);
        while(_sectionPool[randomIndex].activeInHierarchy){
            ++randomIndex;
            if(randomIndex >= _sectionPool.Length) randomIndex = 0;
        }
        return _sectionPool[randomIndex];
    }
}
