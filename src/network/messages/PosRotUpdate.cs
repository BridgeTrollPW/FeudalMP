using System;
using FeudalMP.src.foundation;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.server;
using FeudalMP.src.network.service;
using FeudalMP.src.util;
using Godot;

namespace FeudalMP.src.network.messages
{
    
    public class PosRotUpdate : Node, INetworkMessage
    {
        [Serializable]
        private class Data : INetworkMessageData
        {
            public int peerId;
            public Vector3 translation;
            public Vector3 rotationDegrees;
        }
        private Data data;
        public int PeerId { get => data.peerId; set => data.peerId = value; }
        public Vector3 Translation { get => data.translation; set => data.translation = value; }
        public Vector3 RotationDegrees { get => data.rotationDegrees; set => data.rotationDegrees = value; }

        public INetworkMessage Deserialize(byte[] byteArray)
        {
            return NetworkMessageSerializer.Deserialize<PosRotUpdate>(byteArray);
        }

        public void ExecuteClient(int senderPeer)
        {
            //Don't update own position for now
            //Todo later check with server position
            if (PeerId == NodeTreeManager.Instance.SceneLayer.GetTree().GetNetworkUniqueId())
            {
                return;
            }
            Logger log = new Logger(nameof(PosRotUpdate));
            Spatial CharacterRepresentation = NodeTreeManager.Instance.SceneLayer.GetNodeOrNull<Spatial>(string.Format("{0}", PeerId));
            if (CharacterRepresentation == null)
            {

                log.Warn(string.Format("Incoming request for peer position and rotation update could not be designated to an existing/synced client peer {0}", PeerId));
                return;
            }
            log.Info(string.Format("Translation={0},RotationDegrees={1}", Translation, RotationDegrees));

            CharacterRepresentation.Translation = Translation;
            CharacterRepresentation.RotationDegrees = RotationDegrees;
        }

        public void ExecuteServer(int senderPeer)
        {
            Server server = NodeTreeManager.Instance.ServiceLayer.GetNode<Server>("./Server");
            server.Clients[senderPeer].Translation = data.translation;
            server.Clients[senderPeer].Rotation = data.rotationDegrees;
            server.NetworkMessageDispatcher.Dispatch(new PosRotUpdate()
            {
                PeerId = senderPeer,
                Translation = data.translation,
                RotationDegrees = data.rotationDegrees
            }, NetworkedMultiplayerPeer.TargetPeerBroadcast);
        }

        public NetworkMessageIdentifier GetNetworkMessageIdentifier()
        {
            return NetworkMessageIdentifier.POSITION_ROTATION_UPDATE;
        }

        public byte[] Serialize()
        {
            return NetworkMessageSerializer.Serialize(this);
        }

        bool INetworkMessage.RequiresNodeInitialisation()
        {
            return true;
        }

        public INetworkMessageData GetData()
        {
            return data;
        }
    }
}