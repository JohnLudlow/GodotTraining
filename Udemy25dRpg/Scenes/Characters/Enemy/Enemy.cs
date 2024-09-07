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

    public override void _Ready()
    {
        StateMachineNode.SwitchState<EnemyIdleState>();
    }

}
