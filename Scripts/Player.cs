using Godot;
using System;
using System.Diagnostics;

public class Player : Spatial
{
    [Export]
    public NodePath headNode;

    [Export]
    public int rotationSpeed = 5;

    [Export]
    public int movementSpeed = 100;

    private Vector3 _targetVelocity = Vector3.Zero;
    private KinematicBody headKB;

    public override void _Ready()
    {
        headKB = GetNodeOrNull<KinematicBody>(headNode);
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 direction = headKB.Transform.basis.z;

        if (Input.IsActionPressed("move_forward"))
        {
            direction *= movementSpeed;
        }

        if (Input.IsActionPressed("move_backward"))
        {
            direction *= -1 * movementSpeed;
        }

        float spin = 0;

        if (Input.IsActionPressed("rotate_clockwise"))
        {
            spin += 1f;
        }
        else if (Input.IsActionPressed("rotate_counter_clockwise"))
        {
            spin -= 1f;
        }

        headKB.Rotate(Vector3.Up, spin * rotationSpeed * delta);
        _targetVelocity = direction * delta;

        headKB.MoveAndSlide(_targetVelocity);
    }
}