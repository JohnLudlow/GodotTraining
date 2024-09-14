using Godot;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerMoveState : PlayerState
{
    [Export(PropertyHint.Range, "0, 30, .1")]
    public float MoveFactor { get; private set; } = 5f;

    public override void _PhysicsProcess(double delta)
    {
        if (_characterNode.Direction == Vector2.Zero)
        {
            _characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
            return;
        }

        _characterNode.Velocity = new(
            _characterNode.Direction.X * MoveFactor,
            0,
            _characterNode.Direction.Y * MoveFactor
        );

        _characterNode.MoveAndSlide();
        _characterNode.ApplyFloorSnap();
        _characterNode.FlipSprite();
    }

    protected override void EnterState() => _characterNode.AnimatedSprite3DNode.Play(nameof(Player.PlayerAnimations.Move));

    public override void _Input(InputEvent @event)
    {
        HandleInput();
    }
}
