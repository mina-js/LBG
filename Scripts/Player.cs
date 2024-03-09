using Godot;
using System;
using System.Diagnostics;

public class Player : KinematicBody
{
    [Export]
    public int Speed = 14;
    private Vector3 _targetVelocity = Vector3.Zero;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 direction = Vector3.Zero;

        if (Input.IsActionPressed("move_forward"))
        {
            Debug.WriteLine("Move Forward");
            direction.z -= 1f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            LookAt(direction, Vector3.Up);
        }

        _targetVelocity.z = direction.z * Speed;
        MoveAndSlide(_targetVelocity);
    }
}