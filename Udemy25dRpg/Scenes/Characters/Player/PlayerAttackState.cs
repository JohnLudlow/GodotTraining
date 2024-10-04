using Godot;

using System;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerAttackState : StateMachineStateBase
{
  private int _comboCounter = 1;
  private int _maxComboCount = 2;

  [Export] public Timer ComboTimerNode { get; set; }

  public override void _Ready()
  {
    base._Ready();
    ComboTimerNode.Timeout += () => _comboCounter = 1;
  }

  protected override void EnterState()
  {
    CharacterNode.AnimatedSprite3DNode.AnimationFinished += HandleAnimationFinished;
    CharacterNode.AnimatedSprite3DNode.FrameChanged += HandleAnimationFrameChanged;

    if (_comboCounter >= _maxComboCount)
    {
      _comboCounter = 1;
      CharacterNode.AnimatedSprite3DNode.Play($"{nameof(Player.PlayerAnimations.Slash)}", customSpeed: 1.5f);
    }
    else
    {
      _comboCounter++;
      CharacterNode.AnimatedSprite3DNode.Play($"{nameof(Player.PlayerAnimations.Kick)}", customSpeed: 1.5f);
    }

  }

  private void HandleAnimationFrameChanged()
  {    
    switch (CharacterNode.AnimatedSprite3DNode.Animation)
    {
      case "Kick" when CharacterNode.AnimatedSprite3DNode.Frame == 4:
      case "Slash" when CharacterNode.AnimatedSprite3DNode.Frame == 3:
        PerformHit();
        break;
    }
  }

  protected override void ExitState()
  {
    CharacterNode.AnimatedSprite3DNode.AnimationFinished -= HandleAnimationFinished;
    CharacterNode.AnimatedSprite3DNode.FrameChanged -= HandleAnimationFrameChanged;

    CharacterNode.HitboxShapeNode.Disabled = true;

    ComboTimerNode.Start();
  }

  private void HandleAnimationFinished() => CharacterNode.StateMachineNode.SwitchState<PlayerIdleState>();

  private void PerformHit()
  {
    CharacterNode.HitboxAreaNode.Position = .75f * (CharacterNode.AnimatedSprite3DNode.FlipH ? Vector3.Left : Vector3.Right);
    CharacterNode.HitboxShapeNode.Disabled = false;
  }
}
