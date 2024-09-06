using Godot;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerIdleState : StateMachineStateBase
{
    public override void _Ready()
    {
        base._Ready();

        _characterNode.AnimatedSprite3DNode.AnimationFinished += () => {             
            _characterNode.StateMachineNode.SwitchState<PlayerIdleState>(); 
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_characterNode.Direction != Vector2.Zero)
        {
            _characterNode.StateMachineNode.SwitchState<PlayerMoveState>();
        }
    }

    protected override void EnterState()
    {
        _characterNode.AnimatedSprite3DNode.Play(nameof(Player.PlayerAnimations.Idle));
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed(nameof(Player.PlayerInputs.Dash)))
        {
            _characterNode.StateMachineNode.SwitchState<PlayerDashState>();
        }
        else if (Input.IsActionJustPressed(nameof(Player.PlayerInputs.Kick)))
        {
            _characterNode.StateMachineNode.SwitchState<PlayerKickingState>();
        }
    }
}
