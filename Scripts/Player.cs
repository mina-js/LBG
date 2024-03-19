using Godot;
using Godot.Collections;
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
    private KinematicBody headKB;
    private List<Leg> legs = new List<Leg>();

    public override void _Ready()
    {
        headKB = GetNodeOrNull<KinematicBody>(headNode);
        foreach (var legPath in legPaths)
        {
            legs.Add(GetNodeOrNull<Leg>(legPath));
        }
    }

    public override void _PhysicsProcess(float delta)
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

        headKB.Rotate(Vector3.Up, spin * rotationSpeed * delta);
    }

    void MakeMove(float delta, Vector2 goalVector)
    {
        GD.Print("STARTING MOVE----");
        GD.Print("goalVector: " + goalVector);

        var randomlyReorderedLegs = new List<Leg>();
        foreach (var leg in legs.GetRange(3, 5))
        {
            randomlyReorderedLegs.Add(leg);
        }
        Utils.Shuffle(randomlyReorderedLegs);

        SceneTreeTween allLegsTween = GetTree().CreateTween();
        allLegsTween.SetParallel();

        for (var i = 0; i < randomlyReorderedLegs.Count; i++)
        {
            randomlyReorderedLegs[i].ScaleLeg(ref allLegsTween, (1 + i) * .25f);
        }

        allLegsTween.Connect(
            "finished",
            this,
            nameof(OnTweenAllLegsFinished),
            new Godot.Collections.Array { goalVector, randomlyReorderedLegs });
        allLegsTween.Play();
    }

    public void OnTweenAllLegsFinished(Vector2 goalVector, List<Leg> legs)
    {
        //Just apply the impulse in the desired direction, simplest
        Vector3 impulse = new Vector3(goalVector.x, 0, goalVector.y) * impulseMultiplier;
        GD.Print("SIMPLE IMPULSE: " + impulse);

        SceneTreeTween allLegsTween = GetTree().CreateTween();
        allLegsTween.SetParallel();

        for (var i = 0; i < legs.Count; i++)
        {
            legs[i].ResetLeg(ref allLegsTween);
        }

        headKB.MoveAndSlide(
            impulse
        );
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