using Godot;

public class Programm : Node
{
    public override void _Ready()
    {
        GetTree().ChangeScene("res://assets/ui/mainmenu/MainMenu.tscn");
    }
}
