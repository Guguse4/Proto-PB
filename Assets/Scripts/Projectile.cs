using System.Collections;
using Entity.Boss;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float timeoutDelay = 3f;
    [SerializeField] private float speed = 3f;

    // private BulletPool objectPool;
    // public BulletPool ObjectPool { set => objectPool = value; }
    
    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }

    IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        // objectPool.pool.Release(gameObject);
        GetComponent<NetworkObject>().Despawn(true);
    }

    private void OnTriggerEnter(Collider collider)
    {
        collider.gameObject.GetComponent<Boss>().TakeDamage(1);
        // objectPool.pool.Release(gameObject);
        GetComponent<NetworkObject>().Despawn(true);
    }
}
