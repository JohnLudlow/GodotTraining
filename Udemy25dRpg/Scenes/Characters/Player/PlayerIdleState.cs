using Godot;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerIdleState : PlayerState
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
        HandleInput();
    }
}
