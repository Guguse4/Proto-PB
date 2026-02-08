using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;
using System;

public class AttackController : MonoBehaviour
{
    private ObjectPool<GameObject> pool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: CreateBullet,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyBullet,
            collectionCheck: true,
            defaultCapacity: 20
            );
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject CreateBullet()
    {
        //GameObject gameObject = 
        return gameObject;
    }

    private void OnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    private void OnRelease(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroyBullet(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator ReturnAfter(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // Give it back to the pool.
        pool.Release(gameObject);
    }
}
