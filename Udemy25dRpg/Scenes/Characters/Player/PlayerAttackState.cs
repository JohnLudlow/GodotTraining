using Godot;
using System;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerAttackState : StateMachineStateBase
{
    private int _comboCounter = 1;
    private int _maxComboCount = 2;

    [Export] public Timer ComboTimerNode {get;set;}

    public override void _Ready()
    {
        base._Ready();
        ComboTimerNode.Timeout += () => _comboCounter = 1;
    }

    protected override void EnterState()
    {
        _characterNode.AnimatedSprite3DNode.AnimationFinished += HandleAnimationFinished;

        if (_comboCounter >= _maxComboCount)
        {
            _comboCounter = 1;
            _characterNode.AnimatedSprite3DNode.Play($"{nameof(Player.PlayerAnimations.Slash)}", customSpeed: 1.5f);
        }
        else
        {
            _comboCounter++;
            _characterNode.AnimatedSprite3DNode.Play($"{nameof(Player.PlayerAnimations.Kick)}", customSpeed: 1.5f);
        }
    }

    protected override void ExitState()
    {
        _characterNode.AnimatedSprite3DNode.AnimationFinished -= HandleAnimationFinished;
        ComboTimerNode.Start();
    }

    private void HandleAnimationFinished() => _characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
}
