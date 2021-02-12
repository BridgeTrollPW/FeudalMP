using System;
using FeudalMP.src.foundation;
using FeudalMP.src.network.client;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.server;
using FeudalMP.src.network.server.entity;
using FeudalMP.src.network.service;
using Godot;

namespace FeudalMP.src.network.messages
{
    [System.Serializable]
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
            NodeTreeManager.Instance.GUILayer.Clear();
            NodeTreeManager.Instance.SceneLayer.ChangeScene<Node>(string.Format("res://assets/maps/{0}/{1}.tscn", mapName, mapName));
            Spatial Character = AssetManager.Load<Spatial>(AssetManager.PATH_BASE + "/character/Character.tscn");
            Character.Translation = new Vector3(0, 2, 0);
            NodeTreeManager.Instance.SceneLayer.AddChild(Character);
            Character.Name = NodeTreeManager.Instance.SceneLayer.GetTree().GetNetworkUniqueId().ToString();
            //Add ingame esc menu
            NodeTreeManager.Instance.HUDLayer.AddChild(AssetManager.Load<InGameHUD>(AssetManager.PATH_UI + "/ingamehud/InGameHUD.tscn"));
            //Send Sync Message
            Client client = NodeTreeManager.Instance.ServiceLayer.GetNode<Client>("./Client");
            client.NetworkMessageDispatcher.Dispatch(new Sync(), NetworkedMultiplayerPeer.TargetPeerServer);
        }

        public void ExecuteServer(int senderPeer)
        {
            Server server = NodeTreeManager.Instance.ServiceLayer.GetNode<Server>("./Server");
            server.Clients[senderPeer].State = GameClientState.INITIAL_SYNC;
            server.NetworkMessageDispatcher.Dispatch(new InitialSync()
            {
                MapName = server.Map
            }, senderPeer);

        }

        public NetworkMessageIdentifier GetNetworkMessageIdentifier()
        {
            return NetworkMessageIdentifier.INITIAL_SYNC;
        }

        public bool RequiresNodeInitialisation()
        {
            return false;
        }

        public byte[] Serialize()
        {
            return NetworkMessageSerializer.Serialize(this);
        }
    }
}