using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    //public float speed = 10f;
    public float accelerationMultiplier = 30f, brakeMultipler = 150f;
    public float steerMultiplier = 25f, maxSteerVelocity = 50f;
    public float minForwardVelocity = 20f, maxForwardVelocity = 100f;
    public float rotationAmount = 1f;
    Vector2 _input = Vector2.zero;
    [SerializeField] Rigidbody _rb;
    [SerializeField] Transform _carModel;
    [SerializeField] ExplodeHandler _explodeHandler;
    bool _isExploded = false, _isFever = false;
    bool _isPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        _isPlayer = CompareTag("Player");
    }

    // Update is called once per frame
    void Update(){
        if(_isExploded) return;
        _carModel.rotation = Quaternion.Euler(0, _rb.velocity.x * rotationAmount, 0);
        if(_isPlayer) GameEvents.OnSpeedChanged(_rb.velocity.z);
    }
    void FixedUpdate()
    {
        if(_isExploded){
            //slow down the car into the range [1.5, 10]
            _rb.drag = _rb.velocity.z * .1f;
            _rb.drag = Mathf.Clamp(_rb.drag, 1.5f, 10);

            //slowly move "the parent of the car" to the center 
            //camera also moves to the center too, because it followed "the parent of the car"
            _rb.MovePosition(Vector3.Lerp(
                transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime * .5f));
            return;
        }

        if(_input.y > 0) Accelerate();
        else _rb.drag = .2f;

        if(_input.y < 0) Brake();

        Steer();

        if(_rb.velocity.z <= 0) _rb.velocity = Vector3.zero;

        //maintain a minimum velocity to prevend the player from standing stationary
        //if(_isPlayer){
            _rb.velocity = new Vector3(_rb.velocity.x, 0, 
                Mathf.Clamp(_rb.velocity.z, minForwardVelocity, maxForwardVelocity));
        //}
    }

    void Accelerate(){
        _rb.drag = 0;
        if(_rb.velocity.z >= maxForwardVelocity) return;
        _rb.AddForce(_rb.transform.forward * accelerationMultiplier * _input.y);
    }

    void Brake(){
        if(_rb.velocity.z <= 0) return;
        _rb.AddForce(_rb.transform.forward * brakeMultipler * _input.y);
    }

    void Steer(){
        if(Mathf.Abs(_input.x) > 0){
            //if the car is stationary then it can not steer (speedBaseSteerLimit = 0)
            float speedBaseSteerLimit = _rb.velocity.z / 5f;
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);
            _rb.AddForce(_rb.transform.right * steerMultiplier * _input.x * speedBaseSteerLimit);

            //Make sure we stay within the range [-maxSteerVelocity, maxSteerVelocity]
            float normalizedX = _rb.velocity.x / maxSteerVelocity;
            normalizedX = Mathf.Clamp(normalizedX, -1f, 1f);
            _rb.velocity = new Vector3(normalizedX * maxSteerVelocity, 0, _rb.velocity.z);
        } else {
            _rb.velocity = Vector3.Lerp(_rb.velocity, new Vector3(0, 0, _rb.velocity.z), Time.fixedDeltaTime * 3);
        }
    }

    void OnDisable(){
        if(_isPlayer){
            GameEvents.OnPlayerInput -= SetInput;
            // GameEvents.OnFeverBegin -= () => _isFever = true;
            // GameEvents.OnFeverEnd -= () => {
            //     _isFever = false; 
            //     _rb.velocity = new Vector3(_rb.velocity.x, 0, minForwardVelocity);
            // };
        }
    }

    void OnEnable(){
        if(_isPlayer){
            GameEvents.OnPlayerInput += SetInput;
            // GameEvents.OnFeverBegin += () => _isFever = true;
            // GameEvents.OnFeverEnd += () => {
            //     _isFever = false; 
            //     _rb.velocity = new Vector3(_rb.velocity.x, 0, minForwardVelocity);
            // };
        }
    }

    void SetInput(float x, float y){
        //Debug.Log($"After received, x = {x}");
        _input = new Vector2(x, y);
    }

    public void SetInput(Vector2 inputVector){
        //Debug.Log("SetInput(Vector2 inputVector){");
        //inputVector.Normalize();
        _input = inputVector;
        //if(inputVector != Vector2.zero) Debug.Log(inputVector.x + " " + inputVector.y);
    }

    void OnCollisionEnter(Collision collision){
        //if this car is AI car
        if(!_isPlayer){
            // if(collision.transform.root.CompareTag("Untagged")) {
            //     Debug.Log("Untagged " + collision.gameObject.name);
            //     return;
            // }
            if(collision.transform.root.CompareTag("CarAI")) {
                Debug.Log("CarAI");
                return;
            }
        }
        //TODO: check the tag if needed
        //Debug.Log($"Hit {collision.collider.name}");
        if(_isPlayer && _isFever){
            Debug.Log(gameObject.name);
            return;
        }
        
        //Debug.Log(gameObject.name + " isPLayer = " + _isPlayer);
        Vector3 velocity = _rb.velocity;
        _explodeHandler.Explode(velocity, _isFever);
        _isExploded = true;
        _rb.velocity = Vector3.zero;
    }
}
