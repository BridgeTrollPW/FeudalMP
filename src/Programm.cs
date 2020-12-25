using System.Linq;
using FeudalMP.assets.ui.debugoverlay;
using FeudalMP.assets.ui.mainmenu;
using FeudalMP.src.foundation;
using FeudalMP.src.network.server;
using FeudalMP.src.util;
using Godot;

namespace FeudalMP.src
{
    public class Programm : Node
    {
        public override async void _Ready()
        {
            Logger log = new Logger(nameof(Programm));
            log.Info("Waiting for rootnode to complete setup");
            await ToSignal(GetTree().Root, "ready");
            GetTree().Root.AddChild(NodeTreeManager.Instance);

            if (OS.GetCmdlineArgs().Contains("--server"))
            {

                Server server = new Server
                {
                    Port = (int)ProjectSettings.GetSetting("FeudalMP/server/port")
                };
                NodeTreeManager.Instance.ServiceLayer.AddChild(server);
                server.Start();

            }
            else
            {
                log.Info("Starting client application");
                FadeTransition fadeTransition = AssetManager.Load<FadeTransition>(AssetManager.PATH_UI + "/transitions/FadeTransition.tscn");
                NodeTreeManager.Instance.GUILayer.AddChild(fadeTransition);
                fadeTransition.FadeIn();
                NodeTreeManager.Instance.HUDLayer.AddChild(AssetManager.Load<DebugOverlay>(AssetManager.PATH_UI + "/debugoverlay/DebugOverlay.tscn"));
                NodeTreeManager.Instance.GUILayer.AddChild(AssetManager.Load<MainMenu>(AssetManager.PATH_UI + "/mainmenu/MainMenu.tscn"));
            }
        }
    }
}