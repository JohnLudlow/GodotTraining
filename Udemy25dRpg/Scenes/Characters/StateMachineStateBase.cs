using Godot;

namespace Udemy25dRpg.Scenes.Characters;

public abstract partial class StateMachineStateBase : Node 
{   
    protected CharacterBase _characterNode;

    public override void _Ready()
    {
        base._Ready();
        
        _characterNode = GetOwner<CharacterBase>();
    
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
            ExitState();
            SetPhysicsProcess(false);
            SetProcessInput(false);
        }
    }

    protected virtual void ExitState() { }
    protected virtual void EnterState() { }
}
