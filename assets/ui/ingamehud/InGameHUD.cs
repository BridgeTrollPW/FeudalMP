using FeudalMP.src.foundation;
using Godot;

public class InGameHUD : Node
{
    private Control control;
    public override void _Ready()
    {
        control = GetNode<Control>("Control");
        control.Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            control.Visible = !control.Visible;
            if (control.Visible)
            {
                //Disable processing on the player node as soon as this aws set to true
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
        NodeTreeManager.Instance.SceneLayer.Clear();
        NodeTreeManager.Instance.HUDLayer.RemoveChild(this);
        this.QueueFree();
        NodeTreeManager.Instance.GUILayer.ChangeScene<Node>("res://assets/ui/mainmenu/MainMenu.tscn");
    }

    public void OnResumePressed()
    {
        Input.SetMouseMode(Input.MouseMode.Captured);
        control.Visible = false;
    }
}
