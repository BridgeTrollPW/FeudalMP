using FeudalMP.assets.ui.mainmenu;
using FeudalMP.src.foundation;
using FeudalMP.src.network.server;
using Godot;

namespace FeudalMP.assets.ui.serverconfig
{
    public class ServerConfig : Control
    {
        private Button startServerButton;
        private Button stopServerButton;

        private MenuButton mapSelectionMenuButton;

        public override void _Ready()
        {

            startServerButton = GetNode<Button>("HSplitContainer/VBoxContainer/HBoxContainer2/StartServerButton");
            stopServerButton = GetNode<Button>("HSplitContainer/VBoxContainer/HBoxContainer4/StopServerButton");

            if (NodeTreeManager.Instance.ServiceLayer.HasNode("./Server"))
            {
                startServerButton.Disabled = true;
            }
            else
            {
                stopServerButton.Disabled = true;
            }
            mapSelectionMenuButton = GetNode<MenuButton>("HSplitContainer/VBoxContainer/MapSelectionHBoxContainer/MapSelectionMenuButton");
            Directory directory = new Directory();
            if (directory.Open("user://maps") == Error.Ok)
            {
                directory.ListDirBegin();
                string tempMapName = directory.GetNext();
                while (tempMapName != "")
                {
                    if (directory.CurrentIsDir())
                    {
                        mapSelectionMenuButton.GetPopup().AddItem(tempMapName);
                    }
                    tempMapName = directory.GetNext();
                }
            }

        }


        public void OnServerStartPressed()
        {
            Server server = new Server();
            NodeTreeManager.Instance.ServiceLayer.AddChild(server);
            server.Start();
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