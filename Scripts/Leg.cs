using Godot;
using System;
using System.Collections.Generic;

public class Leg : Spatial
{
    [Export]
    List<NodePath> hingePaths;

    public RigidBody hand;
    List<HingeJoint> hinges = new List<HingeJoint>();
    List<float> originalScales = new List<float>();

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (var hingePath in hingePaths)
        {
            var hinge = GetNodeOrNull<HingeJoint>(hingePath);
            if (hinge != null)
            {
                hinges.Add(hinge);
            }
        }
    }


    public void ScaleLeg(ref SceneTreeTween tween, float scale)
    {
        originalScales.Clear();
        GD.Print("Scaling leg " + Name + " to " + scale);
        for (int i = 0; i < hinges.Count; i++)
        {
            var hinge = hinges[i];
            float translationX = hinge.Translation.x;
            tween.Parallel().TweenProperty(hinge, "translation:x", translationX - scale * i, 1f);
            originalScales.Add(translationX);
        }
    }

    public void ResetLeg(ref SceneTreeTween tween)
    {
        GD.Print("Resetting leg " + Name);
        for (int i = 0; i < hinges.Count; i++)
        {
            var hinge = hinges[i];
            float translationX = originalScales[i];
            tween.Parallel().TweenProperty(hinge, "translation:x", translationX, 1f);
        }
    }
}
