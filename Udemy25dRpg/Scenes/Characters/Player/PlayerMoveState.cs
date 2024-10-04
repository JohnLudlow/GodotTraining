using Godot;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerMoveState : PlayerState
{
    [Export(PropertyHint.Range, "0, 30, .1")]
    public float MoveFactor { get; private set; } = 5f;

    public override void _PhysicsProcess(double delta)
    {
        if (CharacterNode.Direction == Vector2.Zero)
        {
            CharacterNode.StateMachineNode.SwitchState<PlayerIdleState>();
            return;
        }

        CharacterNode.Velocity = new(
            CharacterNode.Direction.X * MoveFactor,
            0,
            CharacterNode.Direction.Y * MoveFactor
        );

        CharacterNode.MoveAndSlide();
        CharacterNode.ApplyFloorSnap();
        CharacterNode.FlipSprite();
    }

    protected override void EnterState() => CharacterNode.AnimatedSprite3DNode.Play(nameof(Player.PlayerAnimations.Move));

    public override void _Input(InputEvent @event)
    {
        HandleInput();
    }
}
