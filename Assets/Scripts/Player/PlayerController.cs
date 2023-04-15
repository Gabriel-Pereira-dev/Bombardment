using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Properties
    public float movementSpeed = 10f;

    // State Machine
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public Idle idleState;
    [HideInInspector] public Walking walkingState;

    // Internal Properties
    [HideInInspector] public Vector2 movementVector;
    [HideInInspector] public Rigidbody thisRigidbody;

    [HideInInspector] public Animator thisAnimator;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        thisAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        // State Machie and its states
        stateMachine = new StateMachine();
        idleState = new Idle(this);
        walkingState = new Walking(this);
        stateMachine.ChangeState(idleState);
    }

    void Update()
    {
        // Create input Vector
        bool isUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        float inputX = isRight ? 1 : isLeft ? -1 : 0;
        float inputY = isUp ? 1 : isDown ? -1 : 0;
        movementVector = new Vector3(inputX, inputY);

        //P Passar a velocidade de 0 a 1 pro Animator Controller
        float velocity = thisRigidbody.velocity.magnitude;
        float velocityRate = velocity / movementSpeed;
        thisAnimator.SetFloat("fVelocity", velocityRate);

        stateMachine.Update();
    }


    void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public Quaternion GetFoward()
    {
        Camera camera = Camera.main;
        float eulerY = camera.transform.eulerAngles.y;
        return Quaternion.Euler(0, eulerY, 0);
    }

    public void RotateBodyToFaceInput()
    {
        // if (movementVector.IsZero()) return;

        // Calculate Rotation
        Camera camera = Camera.main;
        Vector3 inputVector = new Vector3(movementVector.x, 0, movementVector.y);
        Debug.Log(inputVector);
        Quaternion q1 = Quaternion.LookRotation(inputVector, Vector3.up);
        Quaternion q2 = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
        Quaternion toRotation = q1 * q2;
        Quaternion newRotation = Quaternion.LerpUnclamped(transform.rotation, toRotation, 0.15f);

        // Apply rotation
        thisRigidbody.MoveRotation(newRotation);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 200, 50), stateMachine.currentStateName);
    }

}
