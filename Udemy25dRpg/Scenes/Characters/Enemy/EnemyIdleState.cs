namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyIdleState : EnemyState
{
    protected override void ExitState()
    {
        base.ExitState();

        CharacterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;        
    }

    protected override void EnterState()
    {
        CharacterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Idle));
        CharacterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (CharacterNode.NavigationAgentNode.IsNodeReady())
        {
            CharacterNode.StateMachineNode.SwitchState<EnemyReturnState>();
        }
    }
}
