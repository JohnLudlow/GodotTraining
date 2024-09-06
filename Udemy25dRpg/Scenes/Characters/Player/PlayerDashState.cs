using Godot;

namespace Udemy25dRpg.Scenes.Characters.Player;

public partial class PlayerDashState : StateMachineStateBase
{
    [Export]
    public Timer DashTimer {get; private set;}

    [Export(PropertyHint.Range, "0, 20, .1")]
    public float DashSpeed {get; private set;} = 10f;

    public override void _Ready()
    {
        base._Ready();

        DashTimer.Timeout += () => { 
            _characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
            _characterNode.Velocity = Vector3.Zero;
        };
    }

    public override void _PhysicsProcess(double delta)
    {
		_characterNode.MoveAndSlide();
        _characterNode.ApplyFloorSnap();
        _characterNode.FlipSprite();
    }

    protected override void EnterState()
    {
        _characterNode.AnimatedSprite3DNode.Play(nameof(Player.PlayerAnimations.Dash));

        if (_characterNode.Velocity == Vector3.Zero)
        {
            _characterNode.Velocity = _characterNode.AnimatedSprite3DNode.FlipH ? Vector3.Left : Vector3.Right;
        }

        _characterNode.Velocity = new (
            _characterNode.Direction.X * DashSpeed, 
            0, 
            _characterNode.Direction.Y * DashSpeed
        );
        
        DashTimer.Start();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustReleased(nameof(Player.PlayerInputs.Dash)))
        {
            _characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
        }
    }
}
