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
        private List<GameClient> connectedClients = new List<GameClient>();

        public List<GameClient> ClientUids { get => connectedClients; set => connectedClients = value; }

        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return NetworkMessageSerializer.Deserialize<Sync>(byteArray);
        }

        public void ExecuteClient(int senderPeer)
        {
            foreach (GameClient peer in ClientUids)
            {
                if (peer.Id == NodeTreeManager.Instance.SceneLayer.GetTree().GetNetworkUniqueId())
                {
                    continue;
                }
                Spatial CharacterRepresentation = AssetManager.Load<Spatial>(AssetManager.PATH_BASE + "/character/CharacterRepresentation.tscn");
                NodeTreeManager.Instance.SceneLayer.AddChild(CharacterRepresentation);
                CharacterRepresentation.Name = string.Format("{0}", peer.Id);
                Label billboard = CharacterRepresentation.GetNode<Label>("Billboard/Viewport/Label");
                billboard.Text = peer.Name + "\n" + peer.Id;
                CharacterRepresentation.Translation = peer.Translation;
                CharacterRepresentation.RotationDegrees = peer.Rotation;

            }
        }

        public void ExecuteServer(int senderPeer)
        {
            Server server = NodeTreeManager.Instance.ServiceLayer.GetNode<Server>("./Server");
            server.Clients[senderPeer].State = GameClientState.SYNC;
            //get list of all connected clients and send to client requesting
            server.NetworkMessageDispatcher.Dispatch(new Sync()
            {
                ClientUids = new List<GameClient>(server.Clients.Values)
            }, senderPeer);

            //Send Sync message to all connected clients with only this single client
            server.NetworkMessageDispatcher.Dispatch(new Sync()
            {
                ClientUids = new List<GameClient>() { server.Clients[senderPeer] }
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

        bool INetworkMessage.RequiresNodeInitialisation()
        {
            return false;
        }
    }
}