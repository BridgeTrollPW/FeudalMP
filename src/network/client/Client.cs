using FeudalMP.assets.ui.mainmenu;
using FeudalMP.src.foundation;
using FeudalMP.src.network.messages;
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

        public NetworkMessageDispatcher NetworkMessageDispatcher { get => networkMessageDispatcher; set => networkMessageDispatcher = value; }

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

            InternalRegisterNetworkMessages();

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
            NodeTreeManager.Instance.ServiceLayer.Clear();
            NodeTreeManager.Instance.SceneLayer.Clear();
            NodeTreeManager.Instance.GUILayer.ChangeScene<MainMenu>("res://assets/ui/mainmenu/MainMenu.tscn");
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
            Terminate();
        }

        public void OnNetworkPeerPacket(int id, byte[] packet)
        {
            networkMessageDispatcher.Process(id, packet);
        }

        private void InternalRegisterNetworkMessages()
        {
            networkMessageDispatcher.RegisterNetworkMessage(new ConnectClient());
            networkMessageDispatcher.RegisterNetworkMessage(new ErrorMessage());
            networkMessageDispatcher.RegisterNetworkMessage(new InitialSync());
            networkMessageDispatcher.RegisterNetworkMessage(new Sync());
            networkMessageDispatcher.RegisterNetworkMessage(new PosRotUpdate());
            networkMessageDispatcher.RegisterNetworkMessage(new Disconnect());
        }
    }
}