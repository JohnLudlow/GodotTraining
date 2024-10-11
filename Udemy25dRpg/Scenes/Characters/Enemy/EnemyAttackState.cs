using System.Linq;

using Godot;

namespace Udemy25dRpg.Scenes.Characters.Enemy;

public partial class EnemyAttackState : EnemyState
{

  private Vector3 _targetPosition;

  protected override void ExitState()
  {
    base.ExitState();

    CharacterNode.AnimatedSprite3DNode.AnimationFinished -= HandleAnimationFinished;
    CharacterNode.AnimatedSprite3DNode.FrameChanged -= HandleAnimationFrameChanged;

  }

  private void HandleAnimationFrameChanged()
  {
    switch (CharacterNode.AnimatedSprite3DNode.Animation)
    {
      case "Attack" when CharacterNode.AnimatedSprite3DNode.Frame == 3:
        PerformHit();
        break;
    }
  }

  protected override void EnterState()
  {
    base.EnterState();

    _targetPosition = CharacterNode
      .AttackAreaNode
      .GetOverlappingBodies()
      .First()
      .GlobalPosition;

    CharacterNode.AnimatedSprite3DNode.AnimationFinished += HandleAnimationFinished;
    CharacterNode.AnimatedSprite3DNode.FrameChanged += HandleAnimationFrameChanged;

    CharacterNode.HitboxShapeNode.Disabled = true;

    CharacterNode.AnimatedSprite3DNode.Play(nameof(Enemy.EnemyAnimations.Attack));
  }

  private void HandleAnimationFinished()
  {
    if (CharacterNode
      .AttackAreaNode
      .GetOverlappingBodies()
      .OfType<Player.Player>()
      .Any())
    {
      EnterState();
    }
    else
    {      
      CharacterNode.StateMachineNode.SwitchState<EnemyChaseState>();
    }
  }

  private void PerformHit()
  {
    CharacterNode.HitboxAreaNode.GlobalPosition = _targetPosition;
    CharacterNode.HitboxShapeNode.Disabled = false;
  }
}
