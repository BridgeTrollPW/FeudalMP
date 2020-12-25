using FeudalMP.assets.ui.serverconfig;
using FeudalMP.src.foundation;
using Godot;
using System;

namespace FeudalMP.assets.ui.mainmenu
{
    public class MainMenu : Control
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        public void OnStartServerPressed()
        {
            NodeTreeManager.Instance.GUILayer.ChangeScene<ServerConfig>("res://assets/ui/serverconfig/ServerConfig.tscn");
        }

        public void OnStartClientPressed()
        {
            NodeTreeManager.Instance.GUILayer.ChangeScene<ClientConfig>("res://assets/ui/clientconfig/ClientConfig.tscn");
        }
    }
}