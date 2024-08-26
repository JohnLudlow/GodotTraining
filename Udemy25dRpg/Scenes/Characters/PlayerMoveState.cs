using Godot;

namespace Udemy25dRpg.Scenes.Characters;

public partial class PlayerMoveState : Node
{
    private Player _characterNode;

    public override void _Ready()
    {
        _characterNode = GetOwner<Player>();
        SetPhysicsProcess(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_characterNode.Direction == Vector2.Zero)
        {
            _characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
        }
    }

    public override void _Notification(int what)
    {        
        base._Notification(what);

        if (what == (int)StateMachine.StateMachineMessages.EnableState)
        {
            _characterNode.AnimationPlayerNode.Play(nameof(Player.PlayerAnimations.Move));
            SetPhysicsProcess(true);
        }
        else if (what == (int)StateMachine.StateMachineMessages.DisableState)
        {
            SetPhysicsProcess(false);
        }
    }
}
