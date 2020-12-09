using FeudalMP.assets.ui.mainmenu;
using FeudalMP.src.foundation;
using FeudalMP.src.network.server;
using Godot;

namespace FeudalMP.assets.ui.serverconfig
{
    public class ServerConfig : Control
    {
        Button startServerButton;
        Button stopServerButton;
        
        public override void _Ready()
        {

            startServerButton = GetNode<Button>("VBoxContainer/HBoxContainer2/StartServerButton");
            stopServerButton = GetNode<Button>("VBoxContainer/HBoxContainer4/StopServerButton");
            if (NodeTreeManager.Instance.ServiceLayer.HasNode("./Server"))
            {
                startServerButton.Disabled = true;
            } else {
                stopServerButton.Disabled = true;
            }
        }


        public void OnServerStartPressed()
        {
            NodeTreeManager.Instance.ServiceLayer.AddChild(new Server());
            startServerButton.Disabled = true;
            stopServerButton.Disabled = false;
        }
        public void OnStopServerButtonPressed()
        {
            NodeTreeManager.Instance.ServiceLayer.GetNode<Server>("./Server").Stop();
            startServerButton.Disabled = false;
            stopServerButton.Disabled = true;
        }
        public void OnBackPressed()
        {
            NodeTreeManager.Instance.GUILayer.ChangeScene<MainMenu>("res://assets/ui/mainmenu/MainMenu.tscn");
        }
    }
}