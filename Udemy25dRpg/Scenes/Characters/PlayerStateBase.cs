using Godot;

namespace Udemy25dRpg.Scenes.Characters;

public abstract partial class PlayerStateBase : Node 
{
    protected Player _characterNode;

    public override void _Ready()
    {
        _characterNode = GetOwner<Player>();
        SetPhysicsProcess(false);
        SetProcessInput(false);
    }

    public override void _Notification(int what)
    {        
        base._Notification(what);

        if (what == (int)StateMachine.StateMachineMessages.EnableState)
        {
            EnterState();
            SetPhysicsProcess(true);
            SetProcessInput(true);
        }
        else if (what == (int)StateMachine.StateMachineMessages.DisableState)
        {
            SetPhysicsProcess(false);
            SetProcessInput(false);
        }
    }

    protected abstract void EnterState();
}
