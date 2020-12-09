using FeudalMP.src.network.server.entity;
using FeudalMP.src.util;
using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;

namespace FeudalMP.src.network.server
{
    public class Server : Node
    {
        private int port;
        private Logger log;
        private NetworkedMultiplayerENet networkedMultiplayerENet;

        private Dictionary<int, GameClient> clients;
        public int Port { get => port; set => port = value; }

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
        }
        public void Stop()
        {
            networkedMultiplayerENet.CloseConnection();
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
    }
}