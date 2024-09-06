namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyIdleState : StateMachineStateBase
{
    protected override void EnterState()
    {
        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Idle));
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
