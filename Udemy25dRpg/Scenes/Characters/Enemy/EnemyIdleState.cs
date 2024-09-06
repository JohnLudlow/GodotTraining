using Godot;
using System;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyIdleState : StateMachineStateBase
{
    protected override void EnterState()
    {
        _characterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Idle));
    }
}
