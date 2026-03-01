using Unity.Netcode;
using UnityEngine;

namespace Combat
{
    public class NetworkBulletHandler: INetworkPrefabInstanceHandler
    {
        public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
        {
            throw new System.NotImplementedException();
        }

        public void Destroy(NetworkObject networkObject)
        {
            throw new System.NotImplementedException();
        }
    }
}