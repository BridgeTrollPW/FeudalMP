using System.Collections.Generic;
using System.Linq;
using FeudalMP.src.foundation;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.server;
using FeudalMP.src.network.server.entity;
using FeudalMP.src.network.service;
using Godot;

namespace FeudalMP.src.network.messages
{
    [System.Serializable]
    public class Sync : INetworkMessage
    {
        private List<int> clientUids = new List<int>();

        public List<int> ClientUids { get => clientUids; set => clientUids = value; }

        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return NetworkMessageSerializer.Deserialize<Sync>(byteArray);
        }

        public void ExecuteClient(int senderPeer)
        {
            foreach (int peerId in ClientUids)
            {
                if (peerId == NodeTreeManager.Instance.SceneLayer.GetTree().GetNetworkUniqueId())
                {
                    continue;
                }
                Spatial CharacterRepresentation = AssetManager.Load<Spatial>(AssetManager.PATH_BASE + "/character/CharacterRepresentation.tscn");
                NodeTreeManager.Instance.SceneLayer.AddChild(CharacterRepresentation);
                CharacterRepresentation.Name = string.Format("{0}", peerId);
            }
        }

        public void ExecuteServer(int senderPeer)
        {
            Server server = NodeTreeManager.Instance.ServiceLayer.GetNode<Server>("./Server");
            server.Clients[senderPeer].State = GameClientState.SYNC;
            //get list of all connected clients and send to client requesting
            server.NetworkMessageDispatcher.Dispatch(new Sync()
            {
                ClientUids = new List<int>(server.GetActiveClientIds())
            }, senderPeer);

            //Send Sync message to all connected clients with only this single client
            server.NetworkMessageDispatcher.Dispatch(new Sync()
            {
                ClientUids = new List<int>() { senderPeer }
            }, NetworkedMultiplayerPeer.TargetPeerBroadcast);
            server.Clients[senderPeer].State = GameClientState.ACTIVE;
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