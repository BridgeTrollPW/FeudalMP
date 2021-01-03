using Godot;

public class Character : KinematicBody
{
    private float moveSpeed = 5f;
    private float sprintSpeed = 15f;
    private float jumpForce = 10f;
    private float gravity = 15f;
    private Vector3 velocity;
    private Camera camera;
    public override void _Ready()
    {
        velocity = new Vector3();
        camera = GetNode<Camera>("CameraOrbit/Camera");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    public override void _PhysicsProcess(float delta)
    {
        float currentSpeed = moveSpeed;
        velocity.x = 0;
        velocity.z = 0;

        Vector3 input = new Vector3();
        if (Input.IsActionPressed("move_forward"))
        {
            input.z += 1;
        }
        if (Input.IsActionPressed("move_backward"))
        {
            input.z -= 1;
        }
        if (Input.IsActionPressed("move_left"))
        {
            input.x += 1;
        }
        if (Input.IsActionPressed("move_right"))
        {
            input.x -= 1;
        }
        if (Input.IsActionPressed("move_sprint"))
        {
            currentSpeed = sprintSpeed;
        }

        input = input.Normalized();

        Vector3 direction = Transform.basis.z * input.z + Transform.basis.x * input.x;

        velocity.x = direction.x * currentSpeed;
        velocity.z = direction.z * currentSpeed;


        velocity.y -= gravity * delta;
        if (Input.IsActionPressed("move_jump") && IsOnFloor())
        {
            velocity.y = jumpForce;
        }
        velocity = MoveAndSlide(velocity, Vector3.Up);
    }
}
