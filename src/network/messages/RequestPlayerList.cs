using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FeudalMP.src.foundation;
using FeudalMP.src.network.client;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.server;
using FeudalMP.src.network.server.entity;
using FeudalMP.src.network.service;
using Godot;

namespace FeudalMP.src.network.messages
{
    [Serializable]
    public class RequestPlayerList : INetworkMessage
    {
        private Dictionary<int, string> clientIds;

        public Dictionary<int, string> ClientIds { get => clientIds; set => clientIds = value; }

        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return NetworkMessageSerializer.Deserialize<RequestPlayerList>(byteArray);
        }

        public void ExecuteClient(int senderPeer)
        {
            Client client = NodeTreeManager.Instance.ServiceLayer.GetNode<Client>("./Client");
            client.PlayerList = ClientIds;
        }

        public void ExecuteServer(int senderPeer)
        {
            Server server = NodeTreeManager.Instance.ServiceLayer.GetNode<Server>("./Server");
            Dictionary<int, string> playerlist = new Dictionary<int, string>();
            foreach (KeyValuePair<int, GameClient> entry in server.Clients)
            {
                playerlist.Add(entry.Key, entry.Value.Name);
            }
            server.NetworkMessageDispatcher.Dispatch(new RequestPlayerList()
            {
                ClientIds = playerlist
            }, senderPeer);
        }

        public NetworkMessageIdentifier GetNetworkMessageIdentifier()
        {
            return NetworkMessageIdentifier.REQUEST_PLAYER_LIST;
        }

        public byte[] Serialize()
        {
            return NetworkMessageSerializer.Serialize(this);
        }
    }
}