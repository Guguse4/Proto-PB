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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // _bulletPool = FindFirstObjectByType<BulletPool>();
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions.FindAction("Fire");

        if (Mouse.current == null)
            return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Initialize bullet from pool
        if (fireAction.IsPressed() && nextTimeToShoot > cooldownWindow /*&& _bulletPool != null*/)
        {
            // GameObject bulletObject = _bulletPool.pool.Get();
            
            SpawnBullet(projectilePrefab, muzzlePosition.position, muzzlePosition.rotation);
            
            // bulletObject.transform.SetPositionAndRotation(muzzlePosition.position, muzzlePosition.rotation);
            
            nextTimeToShoot = 0;
        }
        nextTimeToShoot += Time.deltaTime;
    }
    
    private void SpawnBullet(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (IsServer)
        {
            GameObject bulletObject = Instantiate(prefab, position, rotation);
            if (bulletObject == null) 
            { 
                return;
            }

            bulletObject.gameObject.SetActive(true);
            bulletObject.GetComponent<Projectile>().Deactivate();
            bulletObject.GetComponent<NetworkObject>().Spawn();
        }
        else
        {
            SpawnBulletServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnBulletServerRpc()
    {
        GameObject bulletObject = Instantiate(projectilePrefab, muzzlePosition.position, muzzlePosition.rotation);
        if (bulletObject == null) 
        { 
            return;
        }

        bulletObject.gameObject.SetActive(true);
        bulletObject.GetComponent<Projectile>().Deactivate();
        bulletObject.GetComponent<NetworkObject>().Spawn();
    }
}
