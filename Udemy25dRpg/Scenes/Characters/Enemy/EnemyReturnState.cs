using Godot;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyReturnState : EnemyState
{
    public override void _Ready()
    {
        base._Ready();

        Destination = 
            CharacterNode.PathNode.Curve.GetPointPosition(0) +
            CharacterNode.PathNode.GlobalPosition;
    }

    protected override void ExitState()
    {
        base.ExitState();

        CharacterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;        
    }

    protected override void EnterState()
    {
        base.EnterState();

        CharacterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;        

        CharacterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Move));
        CharacterNode.NavigationAgentNode.TargetPosition = Destination;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CharacterNode.NavigationAgentNode.IsNavigationFinished())
        {
            CharacterNode.StateMachineNode.SwitchState<EnemyPatrolState>();
            return;
        }

        CharacterNode.Velocity = CharacterNode.GlobalPosition.DirectionTo(Destination);
        CharacterNode.MoveAndSlide();
    }
}
