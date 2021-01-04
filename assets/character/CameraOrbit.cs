using System;
using Godot;
using Godot.Collections;

public class CameraOrbit : Spatial
{
    [Signal]
    public delegate void RotationUpdate();
    private float lookSensitivity = 15f;
    private float minLookAngle = -20f;
    private float maxLookAngle = 75f;
    private Vector2 mouseDelta;
    private Spatial player;

    public override void _Ready()
    {
        player = GetParent<Spatial>();
        Input.SetMouseMode(Input.MouseMode.Captured);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion inputEventMouseMotion)
        {
            mouseDelta = inputEventMouseMotion.Relative;
            EmitSignal(nameof(RotationUpdate));
        }
    }

    public override void _Process(float delta)
    {
        Vector3 rot = new Vector3(mouseDelta.y, mouseDelta.x, 0) * lookSensitivity * delta;
        //Assignment to local variable because Parent has no write access
        Vector3 RotationDegreesLocal = RotationDegrees;
        RotationDegreesLocal.x += rot.x;
        RotationDegreesLocal.x = Mathf.Clamp(RotationDegreesLocal.x, minLookAngle, maxLookAngle);
        RotationDegrees = RotationDegreesLocal;

        Vector3 PlayerRotationDegreesLocal = player.RotationDegrees;
        PlayerRotationDegreesLocal.y -= rot.y;
        player.RotationDegrees = PlayerRotationDegreesLocal;

        mouseDelta = new Vector2();
    }
}
