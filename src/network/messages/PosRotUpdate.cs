using System;
using FeudalMP.src.foundation;
using FeudalMP.src.network.entity;
using FeudalMP.src.network.server;
using FeudalMP.src.network.service;
using FeudalMP.src.util;
using Godot;

namespace FeudalMP.src.network.messages
{
    [Serializable]
    public class PosRotUpdate : INetworkMessage
    {
        private int peerId;
        private Vector3 translation;
        private Vector3 rotationDegrees;

        public int PeerId { get => peerId; set => peerId = value; }
        public Vector3 Translation { get => translation; set => translation = value; }
        public Vector3 RotationDegrees { get => rotationDegrees; set => rotationDegrees = value; }

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
            server.Clients[senderPeer].Translation = translation;
            server.Clients[senderPeer].Rotation = rotationDegrees;
            server.NetworkMessageDispatcher.Dispatch(new PosRotUpdate()
            {
                PeerId = senderPeer,
                Translation = translation,
                RotationDegrees = rotationDegrees
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
    }
}