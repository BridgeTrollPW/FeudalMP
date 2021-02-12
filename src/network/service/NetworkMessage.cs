using System;
using FeudalMP.src.network.service;

namespace FeudalMP.src.network.entity
{
    public interface INetworkMessage
    {
        void ExecuteServer(int senderPeer);
        void ExecuteClient(int senderPeer);
        byte[] Serialize();
        INetworkMessage Deserialize(byte[] byteArray);
        NetworkMessageIdentifier GetNetworkMessageIdentifier();

        INetworkMessageData GetData();
        bool RequiresNodeInitialisation();
    }
}