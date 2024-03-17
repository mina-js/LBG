using Godot;
using System;

public class Leg : HingeJoint
{
    [Export]
    public NodePath handPath;
    [Export]
    private float liftForce = 10;

    public RigidBody hand;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        hand = GetNode<RigidBody>(handPath);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("lift"))
        {
            GD.Print("Lift");
            hand.AddForce(Vector3.Up * liftForce, hand.Transform.origin);
        }
    }
}
