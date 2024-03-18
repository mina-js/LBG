using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Player : Spatial
{
    [Export]
    public NodePath headNode;
    [Export]
    List<NodePath> legPaths;

    [Export]
    public int rotationSpeed = 5;
    [Export]
    public int impulseMultiplier = 2500;

    private Vector3 _targetVelocity = Vector3.Zero;
    private RigidBody headRB;
    private List<Leg> legs = new List<Leg>();

    public override void _Ready()
    {
        headRB = GetNodeOrNull<RigidBody>(headNode);
        foreach (var legPath in legPaths)
        {
            legs.Add(GetNodeOrNull<Leg>(legPath));
        }
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("move_forward"))
        {
            MakeMove(delta, new Vector2(1, 1));
        }
        else if (Input.IsActionJustPressed("move_backward"))
        {
            MakeMove(delta, new Vector2(-1, -1));
        }

        float spin = 0;

        if (Input.IsActionPressed("rotate_clockwise"))
        {
            spin -= 1f;
        }
        else if (Input.IsActionPressed("rotate_counter_clockwise"))
        {
            spin += 1f;
        }

        headRB.Rotate(Vector3.Up, spin * rotationSpeed * delta);
    }

    void MakeMove(float delta, Vector2 goalVector)
    {
        GD.Print("STARTING MOVE----");
        GD.Print("goalVector: " + goalVector);

        var randomlyReorderedLegs = new List<Leg>(legs.GetRange(3, 5));
        Utils.Shuffle(randomlyReorderedLegs);

        for (var i = 0; i < randomlyReorderedLegs.Count; i++)
        {
            randomlyReorderedLegs[i].ScaleLeg(1f + (i * 2f));
        }

        //Just apply the impulse in the desired direction, simplest
        Vector3 impulse = new Vector3(goalVector.x, 0, goalVector.y) * impulseMultiplier * delta;
        GD.Print("SIMPLE IMPULSE: " + impulse);
        headRB.ApplyImpulse(
            new Vector3(0, -1, 0),
            impulse
        );


        // var collidingBodies = headRB.GetCollidingBodies();
        // GD.Print("Colliding with: " + collidingBodies.Count);
        // StaticBody floor = collidingBodies[0] as StaticBody;
        // floor

        //Apply the movement via KB
        // headKB.MoveAndSlide(
        //     new Vector3(goalVector.x, 0, goalVector.y) * movementSpeed * delta
        // );

    }

    // public override void _PhysicsProcess(float delta)
    // {
    //     Vector3 direction = headKB.Transform.basis.z;

    //     if (Input.IsActionPressed("move_forward"))
    //     {
    //         direction *= movementSpeed;
    //     }

    //     if (Input.IsActionPressed("move_backward"))
    //     {
    //         direction *= -1 * movementSpeed;
    //     }

    //     float spin = 0;

    //     if (Input.IsActionPressed("rotate_clockwise"))
    //     {
    //         spin -= 1f;
    //     }
    //     else if (Input.IsActionPressed("rotate_counter_clockwise"))
    //     {
    //         spin += 1f;
    //     }

    //     headKB.Rotate(Vector3.Up, spin * rotationSpeed * delta);
    //     _targetVelocity = direction * delta;

    //     headKB.MoveAndSlide(_targetVelocity);
    // }
}