using Godot;

namespace Udemy25dRpg.Scenes.Characters;

public partial class Player : CharacterBody3D
{
    public enum PlayerInputs
    {
        MoveLeft,
        MoveRight,
        MoveForward,
        MoveBackward,
        Dash,
        Kick,
    }

    public enum PlayerAnimations
    {
        Idle,
        Move,
        Dash,
        Kick,
    }


    [Export, ExportGroup("Required Nodes")]
    public StateMachine StateMachineNode { get; private set; }

    [Export, ExportGroup("Required Nodes")]
    public AnimatedSprite3D AnimatedSprite3DNode { get; private set; }

    public Vector2 Direction { get; private set; } = Vector2.Zero;


    public override void _Ready()
    {
        StateMachineNode.SwitchState<PlayerIdleState>();
    }

    public override void _Input(InputEvent @event)
    {
        Direction = Input.GetVector(
            nameof(PlayerInputs.MoveLeft),
            nameof(PlayerInputs.MoveRight),
            nameof(PlayerInputs.MoveForward),
            nameof(PlayerInputs.MoveBackward)
        );
    }

    public void FlipSprite()
    {
        if (Direction.X < 0)
        {
            AnimatedSprite3DNode.FlipH = true;
        }
        else if (Direction.X > 0)
        {
            AnimatedSprite3DNode.FlipH = false;
        }
    }

}
