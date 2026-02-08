using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class AttackController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction fireAction;

    public BulletPool _bulletPool;

    private float nextTimeToShoot;

    [SerializeField] private Transform muzzlePosition;

    [SerializeField] private float speedBullet = 50f;

    [SerializeField] private float cooldownWindow = 0.25f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions.FindAction("Fire");

        if (Mouse.current == null)
            return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Initialize bullet from pool
        if (fireAction.IsPressed() && nextTimeToShoot > cooldownWindow && _bulletPool != null)
        {
            Debug.Log("Enter if");
            Projectile bulletObject = _bulletPool.pool.Get();

            if (bulletObject == null)
            {
                return;
            }

            bulletObject.transform.SetPositionAndRotation(muzzlePosition.position, muzzlePosition.rotation);

            bulletObject.GetComponent<Rigidbody>().AddForce(bulletObject.transform.forward * speedBullet, ForceMode.Acceleration);

            bulletObject.Deactivate();

            nextTimeToShoot = 0;
        }
        nextTimeToShoot += Time.deltaTime;
    }
}
