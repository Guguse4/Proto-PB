using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Multiplayer
{
    public class NetworkedPlayerController: NetworkBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float maxSpeed = 5f;
        
        // Components
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private Camera _currentCamera;
        
        // Movement values
        private float _currentSpeed;
        private Vector3 _movement;
        private Rigidbody _rb;
        
        // Add these variables to your PlayerMovement class.
        private const int BUFFER_SIZE = 1024;
        private PlayerInputData[] _inputBuffer = new PlayerInputData[BUFFER_SIZE];
        private PlayerStateData[] _stateBuffer = new PlayerStateData[BUFFER_SIZE];
        private NetworkVariable<PlayerStateData> _serverState = new NetworkVariable<PlayerStateData>(writePerm: NetworkVariableWritePermission.Server);
        private int _currentTick;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rb = GetComponent<Rigidbody>();
            _currentCamera = FindAnyObjectByType<Camera>();

            if (_playerInput == null) return;
            
            _moveAction = _playerInput.actions.FindAction("Move");
        }

        private void Update()
        {
            if (!IsOwner) return;
            
            // Player rotation
            /*
            Ray ray = _currentCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane aimPlane = new Plane(Vector3.up, transform.position);
            if (aimPlane.Raycast(ray, out float hit))
            {
                Vector3 hitPoint = ray.GetPoint(hit);

                Vector3 lookDirection = hitPoint - transform.position;
                lookDirection.y = 0f;

                if (lookDirection != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
                }
            }
            */
            
            // Player movement
            Vector2 moveInput = _moveAction.ReadValue<Vector2>().normalized;
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

            _rb.MovePosition(_rb.position + moveDirection * maxSpeed * Time.fixedDeltaTime);
            
            PlayerInputData inputData = new PlayerInputData{MoveInput = moveInput, Tick = _currentTick++};
            MoveServerRpc(inputData);
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void MoveServerRpc(PlayerInputData input)
        {
            Vector3 moveDirection = new Vector3(input.MoveInput.x, 0f, input.MoveInput.y);
            _rb.MovePosition(_rb.position + moveDirection * maxSpeed * Time.fixedDeltaTime);

            PlayerStateData state = new PlayerStateData
            {
                Tick = input.Tick,
                Position = transform.position
            };

            ReconcileSClientRpc(state);
        }

        [ClientRpc]
        private void ReconcileSClientRpc(PlayerStateData serverState)
        {
            if(!IsOwner) return;
            
            int bufferIndex = serverState.Tick % BUFFER_SIZE;
            PlayerStateData localState = _stateBuffer[bufferIndex];
            
            float distance = Vector3.Distance(localState.Position, serverState.Position);
            if (distance > 0.01f)
            {
                transform.position = serverState.Position;
                int ticksToResimulate = _currentTick - serverState.Tick;
                for (int i = 0; i < ticksToResimulate; i++)
                {
                    int tickToProcess = serverState.Tick + i;
                    PlayerInputData inputToProcess = _inputBuffer[tickToProcess % BUFFER_SIZE];
                    
                    Vector3 moveDirection = new Vector3(inputToProcess.MoveInput.x, 0f, inputToProcess.MoveInput.y);
                    _rb.MovePosition(_rb.position + moveDirection * maxSpeed * Time.fixedDeltaTime);
                    
                    _stateBuffer[tickToProcess % BUFFER_SIZE] = new PlayerStateData{Tick = tickToProcess, Position = transform.position};
                }
            }
        }
    }
}