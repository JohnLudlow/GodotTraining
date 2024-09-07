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

        _characterNode.AttackAreaNode.BodyEntered -= HandleAttackAreaBodyEntered;
        _characterNode.ChaseAreaNode.BodyExited -= HandleChaseAreaBodyExited;

        TimerNode.Timeout -= HandleTimerTimeout;
        TimerNode.Stop();
    }

    protected override void EnterState()
    {
        base.EnterState();

        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Move));
        
        _characterNode.AttackAreaNode.BodyEntered += HandleAttackAreaBodyEntered;
        _characterNode.ChaseAreaNode.BodyExited += HandleChaseAreaBodyExited;

        _target = _characterNode.ChaseAreaNode.GetOverlappingBodies().OfType<CharacterBody3D>().First();

        TimerNode.Timeout += HandleTimerTimeout;
        TimerNode.Start();
    }

    private void HandleChaseAreaBodyExited(Node3D body) => _characterNode.StateMachineNode.SwitchState<EnemyReturnState>();

    private void HandleAttackAreaBodyEntered(Node3D body) => _characterNode.StateMachineNode.SwitchState<EnemyAttackState>();


    private void HandleTimerTimeout()
    {
        _destination = _target.GlobalPosition;
        _characterNode.NavigationAgentNode.TargetPosition = _destination;        
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Move();
    }
}
