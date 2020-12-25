using FeudalMP.src.foundation;
using FeudalMP.src.network.service;
using FeudalMP.src.util;
using Godot;

namespace FeudalMP.src.network.client
{
    public class Client : Node
    {
        private Logger log;
        private NetworkedMultiplayerENet networkedMultiplayerENet;
        private NetworkMessageDispatcher networkMessageDispatcher;
        public override void _Ready()
        {
            Name = "Client";
            log = new Logger(nameof(Client));
        }

        public void Connect(string ip, int port)
        {
            log.Info(string.Format("Starting client, connecting to ip={0},port={1} ...", ip, port));
            networkedMultiplayerENet = new NetworkedMultiplayerENet();
            networkedMultiplayerENet.CreateClient(ip, port);
            GetTree().NetworkPeer = networkedMultiplayerENet;

            log.Info("Client started");

            networkedMultiplayerENet.Connect("connection_failed", this, "OnConnectionFailure");
            networkedMultiplayerENet.Connect("connection_succeeded", this, "OnConnectionSuccess");
            networkedMultiplayerENet.Connect("server_disconnected", this, "OnServerDisconnected");

            networkMessageDispatcher = new NetworkMessageDispatcher();
            NodeTreeManager.Instance.ServiceLayer.AddChild(networkMessageDispatcher);
            networkMessageDispatcher.RegisterNetworkMessage(new Connect());

            GetTree().Multiplayer.Connect("network_peer_packet", this, "OnNetworkPeerPacket");
        }

        public void Terminate()
        {
            networkedMultiplayerENet.CloseConnection();
            networkedMultiplayerENet.Disconnect("connection_failed", this, "OnConnectionFailure");
            networkedMultiplayerENet.Disconnect("connection_succeeded", this, "OnConnectionSuccess");
            networkedMultiplayerENet.Disconnect("server_disconnected", this, "OnServerDisconnected");
            GetTree().Multiplayer.Disconnect("network_peer_packet", this, "OnNetworkPeerPacket");
            NodeTreeManager.Instance.ServiceLayer.RemoveChild(networkMessageDispatcher);
            networkMessageDispatcher.QueueFree();
            GetTree().NetworkPeer = null;
            QueueFree();
        }

        public void OnConnectionFailure()
        {
            log.Warn("Unable to connect to server");
        }
        public void OnConnectionSuccess()
        {
            log.Info("Successfully connected to server");
        }
        public void OnServerDisconnected()
        {
            log.Warn("Server disconnected this client");
        }

        public void OnNetworkPeerPacket(int id, byte[] packet)
        {
            networkMessageDispatcher.Process(id, packet);
        }
    }
}