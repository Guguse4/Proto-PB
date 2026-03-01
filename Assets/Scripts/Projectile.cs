using System.Collections;
using Entity.Boss;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float timeoutDelay = 3f;
    [SerializeField] private float speed = 3f;

    private BulletPool objectPool;

    public BulletPool ObjectPool { set => objectPool = value; }

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

        objectPool.pool.Release(this);
    }

    private void OnTriggerEnter(Collider collider)
    {
        collider.gameObject.GetComponent<Boss>().TakeDamage(1);
        objectPool.pool.Release(this);
    }
}
