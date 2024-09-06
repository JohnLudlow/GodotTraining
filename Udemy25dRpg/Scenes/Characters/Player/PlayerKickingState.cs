namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerKickingState : StateMachineStateBase
{
    public override void _Ready()
    {
        base._Ready();
        _characterNode.AnimatedSprite3DNode.AnimationFinished += _characterNode.StateMachineNode.SwitchState<PlayerIdleState>;
    }

    protected override void EnterState()
    {
        _characterNode.AnimatedSprite3DNode.Play(nameof(Player.PlayerAnimations.Kick));
    }
}
