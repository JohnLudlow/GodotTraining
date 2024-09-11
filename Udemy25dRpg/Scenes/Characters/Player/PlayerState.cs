using Godot;

namespace Udemy25dRpg.Scenes.Characters.Player;

public abstract partial class PlayerState : StateMachineStateBase 
{
    protected void HandleInput()
    {
        if (Input.IsActionJustPressed(nameof(Player.PlayerInputs.Dash)))
        {
            _characterNode.StateMachineNode.SwitchState<PlayerDashState>();
        }
        else if (Input.IsActionJustPressed(nameof(Player.PlayerInputs.Attack)))
        {
            _characterNode.StateMachineNode.SwitchState<PlayerAttackState>();
        }
    }
}
