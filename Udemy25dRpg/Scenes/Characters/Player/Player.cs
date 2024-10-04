using Godot;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class Player : CharacterBase
{
  public enum PlayerInputs
  {
    MoveLeft,
    MoveRight,
    MoveForward,
    MoveBackward,
    Dash,
    Attack,
  }

  public enum PlayerAnimations
  {
    Idle,
    Move,
    Dash,
    Kick,
    Slash,
  }

  public override void _Ready()
  {
    base._Ready();
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
}
