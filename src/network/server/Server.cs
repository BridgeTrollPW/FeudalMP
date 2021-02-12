using FeudalMP.src.foundation;
using FeudalMP.src.network.messages;
using FeudalMP.src.network.server.entity;
using FeudalMP.src.network.service;
using FeudalMP.src.util;
using Godot;
using System.Collections.Generic;
using System.Linq;

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
        public string Map { get => map; set => map = value; }

        private string map;

        public override void _Ready()
        {
            Name = "Server";
            port = (int)ProjectSettings.GetSetting("FeudalMP/server/port");
            log = new Logger(nameof(Server));
            clients = new Dictionary<int, GameClient>();
            Map = "dev01";
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
            new DatabaseHandler();
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
        public void OnPeerDisconnected(int peerId)
        {
            NetworkMessageDispatcher.Dispatch(new Disconnect()
            {
                DisconnectedPeer = peerId
            }, NetworkedMultiplayerPeer.TargetPeerBroadcast);
            log.Info("Peer disconnected id=" + peerId);
            clients.Remove(peerId);
        }

        public void OnNetworkPeerPacket(int senderPeer, byte[] message)
        {
            networkMessageDispatcher.Process(senderPeer, message);
        }

        public void DisconnectClient(int peerId)
        {
            NetworkMessageDispatcher.Dispatch(new Disconnect()
            {
                DisconnectedPeer = peerId
            }, NetworkedMultiplayerPeer.TargetPeerBroadcast);
            networkedMultiplayerENet.DisconnectPeer(peerId);
        }

        private void InternalRegisterNetworkMessages()
        {
            networkMessageDispatcher.RegisterNetworkMessage(new ConnectClient());
            networkMessageDispatcher.RegisterNetworkMessage(new ErrorMessage());
            networkMessageDispatcher.RegisterNetworkMessage(new InitialSync());
            networkMessageDispatcher.RegisterNetworkMessage(new Sync());
            networkMessageDispatcher.RegisterNetworkMessage(new PosRotUpdate());
            networkMessageDispatcher.RegisterNetworkMessage(new Disconnect());
            networkMessageDispatcher.RegisterNetworkMessage(new RequestPlayerList());
        }

        public List<int> GetActiveClientIds()
        {
            return Clients.Where(entry => GameClientState.ACTIVE.Equals(entry.Value.State)).Select(entry => entry.Key).ToList();
        }
    }
}