using Godot;

namespace Udemy25dRpg.Scenes.Characters;

public abstract partial class CharacterBase : CharacterBody3D
{

    #region Exports (Required Nodes)
    [Export, ExportGroup("Required Nodes")] public StateMachine StateMachineNode { get; protected set; }

    [Export, ExportGroup("Required Nodes")] public AnimatedSprite3D AnimatedSprite3DNode { get; protected set; }

    #endregion

    #region Exports (AI Nodes)
    [Export, ExportGroup("AI Nodes")] public Path3D PathNode { get; private set; }

    [Export, ExportGroup("AI Nodes")] public NavigationAgent3D NavigationAgentNode { get; private set; }

    [Export, ExportGroup("AI Nodes")] public Area3D ChaseAreaNode { get; private set; }
    
    [Export, ExportGroup("AI Nodes")] public Area3D AttackAreaNode { get; private set; }
    
    #endregion

    public Vector2 Direction { get; protected set; } = Vector2.Zero;

    public void FlipSprite()
    {
        if (Velocity.X < 0)
        {
            AnimatedSprite3DNode.FlipH = true;
        }
        else if (Velocity.X > 0)
        {
            AnimatedSprite3DNode.FlipH = false;
        }
    }
}
