using Godot;
using System;

public class ServerConfig : Control
{
    public override void _Ready()
    {
        DebugOverlay debugOverlay = ((PackedScene)GD.Load("res://assets/ui/debugoverlay/DebugOverlay.tscn")).Instance() as DebugOverlay;
        AddChild(debugOverlay);
    }

    public void OnServerStartPressed()
    {
        AddChild(new Server());
    }
}
