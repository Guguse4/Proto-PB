using Unity.Netcode;
using UnityEngine;

namespace Combat
{
    public class NetworkBulletHandler: INetworkPrefabInstanceHandler
    {
        private BulletPool pool;
        
        public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
        {
            GameObject bullet = pool.pool.Get();
            bullet.SetActive(true);
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            return bullet.GetComponent<NetworkObject>();
        }

        public void Destroy(NetworkObject networkObject)
        {
            networkObject.gameObject.SetActive(false);
        }

        public void SetPool(BulletPool in_pool)
        {
            pool = in_pool;
        }
    }
}