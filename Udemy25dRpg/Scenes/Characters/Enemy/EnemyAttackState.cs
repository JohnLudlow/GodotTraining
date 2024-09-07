using System;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyAttackState : EnemyState
{
    protected override void ExitState()
    {
        base.ExitState();

        _characterNode.AnimatedSprite3DNode.AnimationFinished -= HandleAnimationFinished;
    }

    protected override void EnterState()
    {        
        base.EnterState();

        _characterNode.AnimatedSprite3DNode.AnimationFinished += HandleAnimationFinished;
        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Attack));
    }

    private void HandleAnimationFinished()
    {
        _characterNode.StateMachineNode.SwitchState<EnemyChaseState>();
    }

}
