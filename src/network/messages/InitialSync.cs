using System;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.service;

namespace FeudalMP.src.network.messages
{
    [Serializable]
    public class InitialSync : INetworkMessage
    {
        //Map name
        private string mapName;

        public string MapName { get => mapName; set => mapName = value; }

        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return NetworkMessageSerializer.Deserialize<InitialSync>(byteArray);
        }

        public void ExecuteClient(int senderPeer)
        {
           //load map name and add client to the map
           //Send Sync Message
        }

        public void ExecuteServer(int senderPeer)
        {
            //Send map name to client
        }

        public NetworkMessageIdentifier GetNetworkMessageIdentifier()
        {
            return NetworkMessageIdentifier.INITIAL_SYNC;
        }

        public byte[] Serialize()
        {
            return NetworkMessageSerializer.Serialize(this);
        }
    }
}