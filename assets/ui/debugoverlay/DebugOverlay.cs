using Godot;
using System;

public class DebugOverlay : Control
{
    private CheckButton checkButton;
    public override void _Ready()
    {
        checkButton = GetNode<CheckButton>("./ServerRunning");
    }

    public override void _Process(float delta)
    {
        if (GetTree().NetworkPeer != null && GetTree().IsNetworkServer())
        {
            checkButton.Pressed = true;
        }
    }

}
