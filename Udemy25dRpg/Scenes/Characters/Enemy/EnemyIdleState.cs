namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyIdleState : EnemyState
{
    protected override void ExitState()
    {
        base.ExitState();

        _characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;        
    }

    protected override void EnterState()
    {
        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Idle));
        _characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;        
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_characterNode.NavigationAgentNode.IsNodeReady())
        {
            _characterNode.StateMachineNode.SwitchState<EnemyReturnState>();
        }
    }
}
