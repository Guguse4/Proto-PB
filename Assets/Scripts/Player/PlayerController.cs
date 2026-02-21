using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementMode
{
    TopDown,
    Side2D
}

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction switchCameraAction;
    public Camera currentCamera; //Change this to change Raycast when phase switch
    public MovementMode currentMovementMode;
    public CameraManager camaraManager;


    [Header("Movement")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float decceleration = 1f;
    private float speed;
    private Vector3 movement;
    private Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        moveAction = playerInput.actions.FindAction("Move");
        switchCameraAction = playerInput.actions.FindAction("CameraSwitch");
        currentCamera = FindAnyObjectByType<Camera>();

        if (Mouse.current == null)
            return;
    }

    // Update is called once per frame
    void Update()
    {
        if (switchCameraAction.WasPressedThisFrame())
        {
            if (currentMovementMode == MovementMode.TopDown)
            {
                camaraManager.SwitchToSide2D();
                currentMovementMode = MovementMode.Side2D;
            }
            else
            {
                camaraManager.SwitchToTopDown();
                currentMovementMode = MovementMode.TopDown;
                //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            }
        }

        if (Mouse.current == null)
        {
            return;
        }

        Ray ray = currentCamera.ScreenPointToRay(Mouse.current.position.ReadValue());


        Plane aimPlane = new Plane(Vector3.zero, transform.position);

        switch(currentMovementMode)
        {
            case MovementMode.TopDown:
                aimPlane = new Plane(Vector3.up, transform.position);
                break;

            case MovementMode.Side2D:
                aimPlane = new Plane(Vector3.right, transform.position);
                break;
        }

        if (aimPlane.Raycast(ray, out float hit))
        {
            Vector3 hitPoint = ray.GetPoint(hit);

            Vector3 direction = hitPoint - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero) //direction.sqrMagnitude > 0.0001f
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 directionInput = moveAction.ReadValue<Vector2>().normalized;
        Vector3 direction = Vector3.zero;

        if(currentMovementMode == MovementMode.TopDown)
        {
            //WASD -> X / Z
            direction = new Vector3(directionInput.x, 0f, directionInput.y);
        }

        else if(currentMovementMode == MovementMode.Side2D)
        {
            // WASD -> Y / Z
            direction = new Vector3(0f, directionInput.x, directionInput.y);
        }

        if (directionInput != Vector2.zero)
        {
            movement = direction;
            speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            speed = Mathf.MoveTowards(speed, 0f, decceleration * Time.deltaTime);
        }

        //transform.position += movement * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    public void SetMovementNode(MovementMode mode)
    {
        currentMovementMode = mode;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit: " + other.gameObject.name);
    }
}
