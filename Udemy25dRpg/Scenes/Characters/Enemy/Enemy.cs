using Godot;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class Enemy : CharacterBase
{
    public enum EnemyAnimations
    {
        Idle,
        Move,
        Attack,
        Death,
        TakeHit
    }

    [Export, ExportGroup("Required Nodes")] public Area3D HurtboxAreaNode { get; protected set; }

    public override void _Ready()
    {
        base._Ready();

        HurtboxAreaNode.AreaEntered += static area =>
        {
            GD.Print($"Hello, {area.Name}");
            GD.Print(string.Format("Hello, {0}", area.Name));
            GD.Print("Hello, " + area.Name);
            GD.Print("Ouch");
        };

        StateMachineNode.SwitchState<EnemyIdleState>();
    }

}
