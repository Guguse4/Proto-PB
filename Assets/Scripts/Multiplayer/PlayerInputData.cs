using Unity.Netcode;
using UnityEngine;

namespace Multiplayer
{
    public struct PlayerInputData: INetworkSerializable
    {
        public int Tick;
        public Vector2 MoveInput;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Tick);
            serializer.SerializeValue(ref MoveInput);
        }
    }

    public struct PlayerStateData : INetworkSerializable
    {
        public int Tick;
        public Vector3 Position;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Tick);
            serializer.SerializeValue(ref Position);
        }
    }
}