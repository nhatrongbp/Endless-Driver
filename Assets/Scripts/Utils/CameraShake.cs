using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration = .6f, magnitude = .6f;
    Vector3 originalPos;
    IEnumerator IEShakeCo;
    // Start is called before the first frame update
    void Start()
    {
        IEShakeCo = null;
        originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShake(){
        IEShakeCo = ShakeCo();
        StartCoroutine(IEShakeCo);
    }

    public void StopShake(){
        if(IEShakeCo != null){
            StopCoroutine(IEShakeCo);
            transform.localPosition = originalPos;
        }
    }

    IEnumerator ShakeCo(){
        float elapsed = 0f;
        while(elapsed < duration){
            transform.localPosition = new Vector3(
                Random.Range(-1f, 1f) * magnitude + originalPos.x,
                Random.Range(-1f, 1f) * magnitude + originalPos.y,
                originalPos.z
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    public Vector3 GetDeltaPosition(){
        return transform.localPosition - originalPos;
    }
}
