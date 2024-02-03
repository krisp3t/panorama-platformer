using Godot;

public partial class Player : CharacterBody3D
{
    [Signal]
    public delegate void DiedEventHandler();

    private float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    [Export] private float JumpVelocity = 4.5f;
    [Export] private float Speed = 5.0f;


    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        if (velocity.Y <= -30.0f)
        {
            EmitSignal(SignalName.Died);
            return;
        }

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        var inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            // Apply friction based on the physics material of the floor.
            if (IsOnFloor())
                for (var i = 0; i < GetSlideCollisionCount(); i++)
                {
                    var collision = GetSlideCollision(i);
                    if (collision.GetCollider() is not StaticBody3D staticBody) continue;
                    var friction = staticBody.PhysicsMaterialOverride?.Friction ?? 1;
                    velocity.X = Mathf.Lerp(Velocity.X, 0, (float)delta * (friction * 5));
                    velocity.Z = Mathf.Lerp(Velocity.Z, 0, (float)delta * (friction * 5));
                }
        }


        GD.Print(velocity, delta);
        Velocity = velocity;
        MoveAndSlide();
    }
}