using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeHandler : MonoBehaviour
{
    public float explodeForce = 200f;
    [SerializeField] GameObject[] hideWhenExploding;
    [SerializeField] Rigidbody[] rigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        //Explode(Vector3.forward);
    }

    // Update is called once per frame
    public void Explode(Vector3 externalForce, bool canBeTriggered)
    {
        foreach (var item in hideWhenExploding) item.SetActive(false);
        foreach (var rb in rigidbodies)
        {
            rb.transform.parent = null;
            rb.GetComponent<MeshCollider>().enabled = true;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.AddForce(Vector3.up * explodeForce + externalForce * .1f); //, ForceMode.Force);

            //provide a random vector3 inside the sphere with .5f radius
            rb.AddTorque(Random.insideUnitSphere * .5f, ForceMode.Impulse);

            //Change the tag so other objects can explode after being hit by a CarPart
            if(!canBeTriggered) rb.gameObject.tag = "CarPart";
            else rb.gameObject.layer = 4;
        }
        if(CompareTag("Player")) GameEvents.OnGameOver();

        //AudioManager.instance.PlaySound("Explode");
    }
}
