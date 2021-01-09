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

        public void OnDevScenePressed()
        {
            NodeTreeManager.Instance.GUILayer.Clear();
            NodeTreeManager.Instance.HUDLayer.AddChild(AssetManager.Load<InGameHUD>(AssetManager.PATH_UI + "/ingamehud/InGameHUD.tscn"));
            NodeTreeManager.Instance.SceneLayer.ChangeScene<Node>("res://assets/maps/dev01/dev01.tscn");
            Spatial Character = AssetManager.Load<Spatial>(AssetManager.PATH_BASE + "/character/Character.tscn");
            Character.Translation = new Vector3(0, 2, 0);
            NodeTreeManager.Instance.SceneLayer.AddChild(Character);
            Character.Name = NodeTreeManager.Instance.SceneLayer.GetTree().GetNetworkUniqueId().ToString();
        }
        public void OnExitPressed()
        {
            GetTree().Quit();
        }
    }
}