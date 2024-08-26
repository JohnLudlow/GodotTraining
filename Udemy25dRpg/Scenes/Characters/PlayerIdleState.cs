using Godot;

namespace Udemy25dRpg.Scenes.Characters;

public partial class PlayerIdleState : Node
{
    private Player _characterNode;

    public override void _Ready()
    {
        _characterNode = GetOwner<Player>();                
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_characterNode.Direction != Vector2.Zero)
        {
            _characterNode.StateMachineNode.SwitchState<PlayerMoveState>();
        }
    }

    public override void _Notification(int what)
    {        
        base._Notification(what);

        if (what == 5001)
        {
            _characterNode.AnimationPlayerNode.Play(nameof(Player.PlayerAnimations.Idle));
            SetPhysicsProcess(true);
        }
        else if (what == 5002)
        {
            SetPhysicsProcess(false);
        }
    }
}
