using System;
using System.Linq;

using Godot;

using Udemy25dRpg.Resources;

namespace Udemy25dRpg.Scenes.Characters;

public abstract partial class CharacterBase : CharacterBody3D
{
  [Export] private StatsResource[] Stats { get; set; } = [];

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

  [Export, ExportGroup("Required Nodes")] public Area3D HurtboxAreaNode { get; protected set; }
  [Export, ExportGroup("Required Nodes")] public Area3D HitboxAreaNode { get; protected set; }
  [Export, ExportGroup("Required Nodes")] public CollisionShape3D HitboxShapeNode { get; protected set; }

  public override void _Ready()
  {
    HurtboxAreaNode.AreaEntered += HurtboxAreaNode_AreaEntered;
  }

  private void HurtboxAreaNode_AreaEntered(Area3D area)
  {
    var health = Stats.FirstOrDefault(st => st.StatType == StatType.Health);
    var player = area.GetOwner<CharacterBase>();
    health.StatValue -= player.Stats.FirstOrDefault(s => s.StatType == StatType.Strength)?.StatValue ?? 0;
  }

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
