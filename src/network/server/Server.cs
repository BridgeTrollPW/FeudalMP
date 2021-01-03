using FeudalMP.src.foundation;
using FeudalMP.src.network.server.entity;
using FeudalMP.src.network.service;
using FeudalMP.src.util;
using Godot;
using System.Collections.Generic;

namespace FeudalMP.src.network.server
{
    public class Server : Node
    {
        private int port;
        private Logger log;
        private NetworkedMultiplayerENet networkedMultiplayerENet;
        private NetworkMessageDispatcher networkMessageDispatcher;

        private Dictionary<int, GameClient> clients;
        public int Port { get => port; set => port = value; }
        public Dictionary<int, GameClient> Clients { get => clients; set => clients = value; }
        public NetworkMessageDispatcher NetworkMessageDispatcher { get => networkMessageDispatcher; set => networkMessageDispatcher = value; }

        public override void _Ready()
        {
            Name = "Server";
            port = (int)ProjectSettings.GetSetting("FeudalMP/server/port");
            log = new Logger(nameof(Server));
            clients = new Dictionary<int, GameClient>();
        }

        public void Start()
        {
            log.Info("Starting server on port=" + port);
            networkedMultiplayerENet = new NetworkedMultiplayerENet();
            networkedMultiplayerENet.CreateServer(port);
            GetTree().NetworkPeer = networkedMultiplayerENet;

            log.Info("Server started");

            networkedMultiplayerENet.Connect("peer_connected", this, "OnPeerConnected");
            networkedMultiplayerENet.Connect("peer_disconnected", this, "OnPeerDisconnected");

            networkMessageDispatcher = new NetworkMessageDispatcher();
            NodeTreeManager.Instance.ServiceLayer.AddChild(networkMessageDispatcher);

            InternalRegisterNetworkMessages();

            GetTree().Multiplayer.Connect("network_peer_packet", this, "OnNetworkPeerPacket");
        }
        public void Stop()
        {
            networkedMultiplayerENet.CloseConnection();
            networkedMultiplayerENet.Disconnect("peer_connected", this, "OnPeerConnected");
            networkedMultiplayerENet.Disconnect("peer_disconnected", this, "OnPeerDisconnected");
            GetTree().Multiplayer.Disconnect("network_peer_packet", this, "OnNetworkPeerPacket");
            NodeTreeManager.Instance.ServiceLayer.RemoveChild(networkMessageDispatcher);
            networkMessageDispatcher.QueueFree();
            GetTree().NetworkPeer = null;
            QueueFree();
            log.Info("Server stopped");
        }
        //
        // Signals
        //
        public void OnPeerConnected(int id)
        {
            log.Info("Peer connected id=" + id);
            clients.Add(id, new GameClient()
            {
                Id = id
            });
        }
        public void OnPeerDisconnected(int id)
        {
            log.Info("Peer disconnected id=" + id);
            clients.Remove(id);
        }

        public void OnNetworkPeerPacket(int senderPeer, byte[] message)
        {
            networkMessageDispatcher.Process(senderPeer, message);
        }

        public void DisconnectClient(int peerId)
        {
            networkedMultiplayerENet.DisconnectPeer(peerId);
        }

        private void InternalRegisterNetworkMessages()
        {
            networkMessageDispatcher.RegisterNetworkMessage(new Connect());
            networkMessageDispatcher.RegisterNetworkMessage(new ErrorMessage());
        }
    }
}