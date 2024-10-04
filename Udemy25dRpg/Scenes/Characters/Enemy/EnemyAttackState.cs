using System;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyAttackState : EnemyState
{
    protected override void ExitState()
    {
        base.ExitState();

        CharacterNode.AnimatedSprite3DNode.AnimationFinished -= HandleAnimationFinished;
    }

    protected override void EnterState()
    {        
        base.EnterState();

        CharacterNode.AnimatedSprite3DNode.AnimationFinished += HandleAnimationFinished;
        CharacterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Attack));
    }

    private void HandleAnimationFinished()
    {
        CharacterNode.StateMachineNode.SwitchState<EnemyChaseState>();
    }

}
