using FeudalMP.src.foundation;
using FeudalMP.src.network.client;
using Godot;

public class InGameHUD : Node
{
    private Control control;
    private PlayerList playerList;
    public override void _Ready()
    {
        control = GetNode<Control>("Control");
        control.Visible = false;

        playerList = AssetManager.Load<PlayerList>(AssetManager.PATH_UI + "/ingamehud/PlayerList.tscn");
        NodeTreeManager.Instance.HUDLayer.AddChild(playerList);
        playerList.Connect("ClosePlayerListUI", this, "OnClosePlayerListUIPressed");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            control.Visible = !control.Visible;
            if (control.Visible)
            {
                //Disable processing on the player node as soon as this aws set to true
                int localId = NodeTreeManager.Instance.SceneLayer.GetTree().GetNetworkUniqueId();
                Node node = NodeTreeManager.Instance.SceneLayer.GetNode(localId.ToString());
                node.SetPhysicsProcess(false);
                node.GetNode(nameof(CameraOrbit)).SetProcess(false);
                Input.SetMouseMode(Input.MouseMode.Visible);
            }
            else
            {
                OnResumePressed();
            }
        }
    }

    public void OnMainMenuPressed()
    {
        if (GetTree().NetworkPeer != null)
        {
            NodeTreeManager.Instance.HUDLayer.Clear();
            Client client = NodeTreeManager.Instance.ServiceLayer.GetNode<Client>("./Client");
            client.Terminate();
        }
        else
        {
            NodeTreeManager.Instance.HUDLayer.Clear();
            NodeTreeManager.Instance.SceneLayer.Clear();
            this.QueueFree();
            NodeTreeManager.Instance.GUILayer.ChangeScene<Node>("res://assets/ui/mainmenu/MainMenu.tscn");
        }
    }

    public void OnResumePressed()
    {
        Input.SetMouseMode(Input.MouseMode.Captured);
        control.Visible = false;
        int localId = NodeTreeManager.Instance.SceneLayer.GetTree().GetNetworkUniqueId();
        Node node = NodeTreeManager.Instance.SceneLayer.GetNode(localId.ToString());
        node.SetPhysicsProcess(true);
        node.GetNode(nameof(CameraOrbit)).SetProcess(true);
    }

    public void OnPlayerListPressed()
    {
        control.Visible = false;
        playerList.Visible = true;
    }

    public void OnClosePlayerListUIPressed()
    {
        control.Visible = true;
        playerList.Visible = false;
    }
}
