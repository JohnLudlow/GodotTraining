using Godot;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyReturnState : EnemyState
{
    public override void _Ready()
    {
        base._Ready();

        _destination = 
            _characterNode.PathNode.Curve.GetPointPosition(0) +
            _characterNode.PathNode.GlobalPosition;
    }

    protected override void ExitState()
    {
        base.ExitState();

        _characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;        
    }

    protected override void EnterState()
    {
        base.EnterState();

        _characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;        

        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Move));
        _characterNode.NavigationAgentNode.TargetPosition = _destination;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_characterNode.NavigationAgentNode.IsNavigationFinished())
        {
            _characterNode.StateMachineNode.SwitchState<EnemyPatrolState>();
            return;
        }

        _characterNode.Velocity = _characterNode.GlobalPosition.DirectionTo(_destination);
        _characterNode.MoveAndSlide();
    }
}
