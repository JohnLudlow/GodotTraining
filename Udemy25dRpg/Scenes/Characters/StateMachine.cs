using System.Linq;
using Godot;

namespace Udemy25dRpg.Scenes.Characters;

public partial class StateMachine : Node
{
    public enum StateMachineMessages {
        EnableState = 5001,
        DisableState = 5002
    }

    [Export]
    public Node CurrentState { get; set; }

    [Export]
    public Node[] PossibleStates {get; private set;}

    public void SwitchState<TState>() where TState : Node
    {
        var newState = PossibleStates.OfType<TState>().FirstOrDefault();

        if (newState is null)
        {
            GD.PushError("An invalid state was passed");
            return;
        }

        CurrentState.Notification((int)StateMachineMessages.DisableState);
        CurrentState = newState;
        CurrentState.Notification((int)StateMachineMessages.EnableState);
    }
}
