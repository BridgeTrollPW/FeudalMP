using FeudalMP.src.foundation;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.server;
using FeudalMP.src.network.service;
using Godot;

namespace FeudalMP.src.network.messages
{
    [System.Serializable]
    public class Disconnect : INetworkMessage
    {
        private int disconnectedPeer;

        public int DisconnectedPeer { get => disconnectedPeer; set => disconnectedPeer = value; }

        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return NetworkMessageSerializer.Deserialize<Disconnect>(byteArray);
        }

        public void ExecuteClient(int senderPeer)
        {
            //do not remove myself
            if (NodeTreeManager.Instance.SceneLayer.GetTree().GetNetworkUniqueId() == DisconnectedPeer)
            {
                return;
            }
            Node remotePeer = NodeTreeManager.Instance.SceneLayer.GetNodeOrNull(DisconnectedPeer.ToString());
            NodeTreeManager.Instance.SceneLayer.RemoveChild(remotePeer);
            remotePeer.QueueFree();
        }

        public void ExecuteServer(int senderPeer)
        {
            //Do not execute if the disconnect was send by the server itself
            if(senderPeer == NetworkedMultiplayerPeer.TargetPeerServer){
                return;
            }
            Server server = NodeTreeManager.Instance.ServiceLayer.GetNode<Server>("./Server");
            server.NetworkMessageDispatcher.Dispatch(new Disconnect()
            {
                DisconnectedPeer = senderPeer
            }, NetworkedMultiplayerPeer.TargetPeerBroadcast);
        }

        public NetworkMessageIdentifier GetNetworkMessageIdentifier()
        {
            return NetworkMessageIdentifier.DISCONNECT;
        }

        public byte[] Serialize()
        {
            return NetworkMessageSerializer.Serialize(this);
        }
    }
}