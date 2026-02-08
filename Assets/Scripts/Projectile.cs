using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float timeoutDelay = 3f;

    private ObjectPool<Projectile> objectPool;

    public ObjectPool<Projectile> ObjectPool { set => objectPool = value; }

    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }

    IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = new Vector3(0f, 0f, 0f);

        objectPool.Release(this);
    }
}
