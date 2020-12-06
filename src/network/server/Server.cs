using Godot;
using System;

public class Server : Node
{
    NetworkedMultiplayerENet networkedMultiplayerENet;
    public override void _Ready()
    {
        networkedMultiplayerENet = new NetworkedMultiplayerENet();
        networkedMultiplayerENet.CreateServer(9913);
        GetTree().NetworkPeer = networkedMultiplayerENet;
    }
}
