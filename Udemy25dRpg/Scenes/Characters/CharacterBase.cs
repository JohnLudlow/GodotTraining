using Godot;

namespace Udemy25dRpg.Scenes.Characters;

public abstract partial class CharacterBase : CharacterBody3D
{
    [Export, ExportGroup("Required Nodes")]
    public StateMachine StateMachineNode { get; protected set; }

    [Export, ExportGroup("Required Nodes")]
    public AnimatedSprite3D AnimatedSprite3DNode { get; protected set; }

    [Export, ExportGroup("AI Nodes")]
    public Path3D PathNode { get; private set; }

    [Export, ExportGroup("AI Nodes")]
    public NavigationAgent3D NavigationAgentNode { get; private set; }

    public Vector2 Direction { get; protected set; } = Vector2.Zero;

    public void FlipSprite()
    {
        if (Direction.X < 0)
        {
            AnimatedSprite3DNode.FlipH = true;
        }
        else if (Direction.X > 0)
        {
            AnimatedSprite3DNode.FlipH = false;
        }
    }
}
