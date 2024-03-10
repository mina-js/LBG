using Godot;
using System;
using System.Diagnostics;

public class Player : Spatial
{
    [Export]
    public NodePath headNode;

    [Export]
    public int Speed = 14;
    private Vector3 _targetVelocity = Vector3.Zero;
    private KinematicBody headKB;

    public override void _Ready()
    {
        headKB = GetNodeOrNull<KinematicBody>(headNode);
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 direction = Vector3.Zero;

        if (Input.IsActionPressed("move_forward"))
        {
            direction.z -= 1f;
        }

        if (Input.IsActionPressed("move_backward"))
        {
            direction.z += 1f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            LookAt(direction, Vector3.Up);
        }

        _targetVelocity.z = direction.z * Speed;
        headKB.MoveAndSlide(_targetVelocity);
    }
}