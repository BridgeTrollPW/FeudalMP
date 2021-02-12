using System;
using FeudalMP.src.foundation;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.server;
using FeudalMP.src.network.service;
using Godot;

namespace FeudalMP.src.network.messages
{
    [Serializable]
    class SpawnObjectMessage : INetworkMessage
    {
        private string resourcePath;
        private Vector3 rotation;
        private Vector3 translation;

        public string ResourcePath { get => resourcePath; set => resourcePath = value; }
        public Vector3 Rotation { get => rotation; set => rotation = value; }
        public Vector3 Translation { get => translation; set => translation = value; }

        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return NetworkMessageSerializer.Deserialize<SpawnObjectMessage>(byteArray);
        }

        public void ExecuteClient(int senderPeer)
        {
            Spatial spatial = AssetManager.Load<Spatial>(ResourcePath);
            NodeTreeManager.Instance.SceneLayer.AddChild(spatial);
            spatial.Rotation = Rotation;
            spatial.Translation = Translation;
        }

        public void ExecuteServer(int senderPeer)
        {
            Server server = NodeTreeManager.Instance.ServiceLayer.GetNode<Server>("./Server");
            server.NetworkMessageDispatcher.Dispatch(this, 0);
        }

        public NetworkMessageIdentifier GetNetworkMessageIdentifier()
        {
            return NetworkMessageIdentifier.SPAWN_OBJECT_MESSAGE;
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