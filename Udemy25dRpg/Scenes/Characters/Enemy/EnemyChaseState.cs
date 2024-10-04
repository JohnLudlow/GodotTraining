using Godot;
using System;
using System.Linq;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyChaseState : EnemyState
{
    private CharacterBody3D _target;

    [Export] public Timer TimerNode { get; set; }

    protected override void ExitState()
    {
        base.ExitState();

        CharacterNode.AttackAreaNode.BodyEntered -= HandleAttackAreaBodyEntered;
        CharacterNode.ChaseAreaNode.BodyExited -= HandleChaseAreaBodyExited;

        TimerNode.Timeout -= HandleTimerTimeout;
        TimerNode.Stop();
    }

    protected override void EnterState()
    {
        base.EnterState();

        CharacterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Move));
        
        CharacterNode.AttackAreaNode.BodyEntered += HandleAttackAreaBodyEntered;
        CharacterNode.ChaseAreaNode.BodyExited += HandleChaseAreaBodyExited;

        _target = CharacterNode.ChaseAreaNode.GetOverlappingBodies().OfType<CharacterBody3D>().First();

        TimerNode.Timeout += HandleTimerTimeout;
        TimerNode.Start();
    }

    private void HandleChaseAreaBodyExited(Node3D body) => CharacterNode.StateMachineNode.SwitchState<EnemyReturnState>();

    private void HandleAttackAreaBodyEntered(Node3D body) => CharacterNode.StateMachineNode.SwitchState<EnemyAttackState>();


    private void HandleTimerTimeout()
    {
        Destination = _target.GlobalPosition;
        CharacterNode.NavigationAgentNode.TargetPosition = Destination;        
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Move();
    }
}
