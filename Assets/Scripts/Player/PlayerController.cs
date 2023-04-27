using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Properties
    public float movementSpeed = 10f;
    public float jumpPower = 10f;
    [Range(0f, 1f)] public float jumpMovementFactor = 1f;

    // State Machine
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public Idle idleState;
    [HideInInspector] public Walking walkingState;
    [HideInInspector] public Jump jumpState;
    [HideInInspector] public Dead deadState;

    // Internal Properties
    [HideInInspector] public Vector2 movementVector;
    [HideInInspector] public bool hasJumpInput;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public Rigidbody thisRigidbody;
    [HideInInspector] public Collider thisCollider;

    [HideInInspector] public Animator thisAnimator;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>();
        thisAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        // State Machie and its states
        stateMachine = new StateMachine();
        idleState = new Idle(this);
        walkingState = new Walking(this);
        jumpState = new Jump(this);
        deadState = new Dead(this);
        stateMachine.ChangeState(idleState);
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {

            if (stateMachine.currentStateName != deadState.name)
            {
                stateMachine.ChangeState(deadState);
            }
        }
        // Create input Vector
        bool isUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        float inputX = isRight ? 1 : isLeft ? -1 : 0;
        float inputY = isUp ? 1 : isDown ? -1 : 0;
        movementVector = new Vector3(inputX, inputY);
        hasJumpInput = Input.GetKey(KeyCode.Space);

        //P Passar a velocidade de 0 a 1 pro Animator Controller
        float velocity = thisRigidbody.velocity.magnitude;
        float velocityRate = velocity / movementSpeed;
        thisAnimator.SetFloat("fVelocity", velocityRate);
        // Detect Ground
        DetectGround();


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
        if (movementVector.IsZero()) return;
        // Calculate Rotation
        Camera camera = Camera.main;
        Vector3 inputVector = new Vector3(movementVector.x, 0, movementVector.y);
        // Debug.Log(inputVector);
        Quaternion q1 = Quaternion.LookRotation(inputVector, Vector3.up);
        Quaternion q2 = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
        Quaternion toRotation = q1 * q2;
        Quaternion newRotation = Quaternion.LerpUnclamped(transform.rotation, toRotation, 0.15f);

        // Apply rotation
        thisRigidbody.MoveRotation(newRotation);
    }

    private void DetectGround()
    {
        // Reset flag
        isGrounded = false;

        // Detect ground
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        Bounds bounds = thisCollider.bounds;
        float radius = bounds.size.x * 0.33f;
        float maxDistance = bounds.size.y * 0.25f;
        if (Physics.SphereCast(origin, radius, direction, out var hitInfo, maxDistance))
        {
            GameObject hitObject = hitInfo.transform.gameObject;
            Debug.Log(hitObject.name);
            if (hitObject.CompareTag("Platform"))
            {
                isGrounded = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (thisCollider != null)
        {
            // Get values
            Vector3 origin = transform.position;
            Vector3 direction = Vector3.down;
            Bounds bounds = thisCollider.bounds;
            float radius = bounds.size.x * 0.33f;
            float maxDistance = bounds.size.y * 0.25f;

            //Draw Ray
            Gizmos.DrawRay(new Ray(origin, direction * maxDistance));

            // Draw Sphere
            Vector3 spherePosition = direction * maxDistance + origin;
            Gizmos.color = isGrounded ? Color.magenta : Color.cyan;
            Gizmos.DrawSphere(spherePosition, radius);
        }
    }

    // void OnGUI()
    // {
    //     Rect rect = new Rect(10, 10, 100, 50);
    //     string text = stateMachine.currentStateName;
    //     GUIStyle style = new GUIStyle
    //     {
    //         fontSize = (int)(50 * (Screen.width / 1920f))
    //     };
    //     GUI.color = Color.blue;
    //     GUI.Label(rect, text, style);
    //     GUI.Label(new Rect(20, 20, 100, 50), isGrounded.ToString(), style);
    // }

}
