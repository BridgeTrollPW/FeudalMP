using System;
using System.Collections.Generic;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.service;

namespace FeudalMP.src.network.messages
{
    [Serializable]
    public class Sync : INetworkMessage
    {
        List<int> clientUids = new List<int>();
        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return NetworkMessageSerializer.Deserialize<Sync>(byteArray);
        }

        public void ExecuteClient(int senderPeer)
        {
            //for each clientUid create new client representation and add uuid as name
        }

        public void ExecuteServer(int senderPeer)
        {
            //get list of all connected clients and send to client requesting
            //Send Sync message to all connected clients with only this single client
        }

        public NetworkMessageIdentifier GetNetworkMessageIdentifier()
        {
            return NetworkMessageIdentifier.SYNC;
        }

        public byte[] Serialize()
        {
            return NetworkMessageSerializer.Serialize(this);
        }
    }
}