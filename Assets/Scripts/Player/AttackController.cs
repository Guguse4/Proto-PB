using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : NetworkBehaviour
{
    PlayerInput playerInput;
    InputAction fireAction;

    // public BulletPool _bulletPool;
    public GameObject projectilePrefab;

    private float nextTimeToShoot;

    [SerializeField] private Transform muzzlePosition;

    [SerializeField] private float speedBullet = 50f;

    [SerializeField] private float cooldownWindow = 0.25f;

    private GameObject _currentBullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions.FindAction("Fire");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) return;
        
        //Initialize bullet from pool
        if (fireAction.IsPressed() && nextTimeToShoot > cooldownWindow)
        {
            PredictLocalBullet();
            SpawnBulletServerRpc(muzzlePosition.position, muzzlePosition.rotation, (float)NetworkManager.ServerTime.Time);
            
            nextTimeToShoot = 0;
        }
        
        nextTimeToShoot += Time.fixedDeltaTime;
    }

    private void PredictLocalBullet()
    {
        _currentBullet = Instantiate(projectilePrefab, muzzlePosition.position, muzzlePosition.rotation);
        Destroy(_currentBullet.GetComponent<NetworkObject>());
    }

    [ServerRpc]
    private void SpawnBulletServerRpc(Vector3 position, Quaternion rotation, float clientTime)
    {
        float serverTime = (float)NetworkManager.ServerTime.Time;
        float lag = serverTime - clientTime;

        float distance = projectilePrefab.GetComponent<Projectile>().Speed * lag * 2;
        Vector3 compensatedPosition = position + rotation * Vector3.forward * distance;
        
        GameObject bulletObject = Instantiate(projectilePrefab, compensatedPosition, rotation);
        bulletObject.GetComponent<Projectile>().Deactivate();
        bulletObject.GetComponent<NetworkObject>().Spawn();
        ConfirmSpawnClientRpc();
    }

    [ClientRpc]
    private void ConfirmSpawnClientRpc()
    {
        Destroy(_currentBullet);
    }
}
