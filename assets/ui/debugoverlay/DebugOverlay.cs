using FeudalMP.src.foundation;
using Godot;
using System;

namespace FeudalMP.assets.ui.debugoverlay
{
    public class DebugOverlay : Control
    {
        private bool showSceneTree = false;
        private CheckButton checkButton;
        private Label debugSceneTree;
        public override void _Ready()
        {
            checkButton = GetNode<CheckButton>("./ServerRunning");
            debugSceneTree = GetNode<Label>("./DebugSceneTree");
        }

        public override void _Process(float delta)
        {

            checkButton.Pressed = (GetTree().NetworkPeer != null && GetTree().IsNetworkServer());


            if (showSceneTree)
            {
                string debugText = "";
                debugText += "GUILayer\n";
                foreach (Node n in NodeTreeManager.Instance.GUILayer.GetChildren())
                {
                    debugText += "--" + n.Name + "\n";
                }
                debugText += "\n\n";
                debugText += "HUDLayer\n";
                foreach (Node n in NodeTreeManager.Instance.HUDLayer.GetChildren())
                {
                    debugText += "--" + n.Name + "\n";
                }
                debugText += "\n\n";
                debugText += "SceneLayer\n";
                foreach (Node n in NodeTreeManager.Instance.SceneLayer.GetChildren())
                {
                    debugText += "--" + n.Name + "\n";
                }
                debugText += "\n\n";
                debugText += "ServiceLayer\n";
                foreach (Node n in NodeTreeManager.Instance.ServiceLayer.GetChildren())
                {
                    debugText += "--" + n.Name + "\n";
                }
                debugSceneTree.Text = debugText;
            }
        }

        public override void _Input(InputEvent inputEvent)
        {
            if (inputEvent.IsActionPressed("debug_scenetree"))
            {
                showSceneTree = !showSceneTree;
                debugSceneTree.Visible = showSceneTree;
            }
        }
    }
}