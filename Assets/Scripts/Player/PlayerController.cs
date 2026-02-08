using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction shootAction;
    public Camera currentCamera; //Change this to change Raycast when phase switch
    public LayerMask groundLayer;


    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxDistanceRayCast = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        shootAction = playerInput.actions.FindAction("Fire");

        if (Mouse.current == null)
            return;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        Ray ray = currentCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceRayCast, groundLayer))
        {
            Vector3 targetPosition = hit.point;

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero) //direction.sqrMagnitude > 0.0001f
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
            }
        }
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;
    }

    void OnFire(InputValue value)
    {
        if(value.isPressed)
        {
            //Initialize bullet from pool
        }
    }
}
