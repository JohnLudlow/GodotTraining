using Godot;

public partial class Player : RigidBody2D
{
	private PlayerStates _state = PlayerStates.Init;

	private Vector2 _thrust = Vector2.Zero;
	private Vector2 _screenSize = Vector2.Zero;
	private float _rotationDirection = 0;

	private bool _canShoot = true;

	public enum PlayerStates
	{
		Init, Alive, Invlunerable, Dead
	}

	[Export]
	public PlayerStates State
	{
		get => _state;
		set
		{
			_state = value;

			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", _state == PlayerStates.Alive);
		}
	}

	[Export]
	public int EnginePower { get; set; } = 15000;

	[Export]
	public int SpinPower { get; set; } = 500;

	[Export]
	public PackedScene BulletScene { get; set; }

	[Export]
	public float FireRate { get; set; } = 0.25f;

	public override void _Ready()
	{
		State = PlayerStates.Alive;
		_screenSize = GetViewportRect().Size;
		GetNode<Timer>("GunCoolDown").WaitTime = FireRate;
	}

	public override void _Process(double delta)
	{
		_thrust = Vector2.Zero;

		if (_state == PlayerStates.Init || _state == PlayerStates.Dead)
		{
			return;
		}
		else if (Input.IsActionPressed("thrust"))
		{
			_thrust = Transform.X * EnginePower;
		}

		if(Input.IsActionPressed("shoot") && _state != PlayerStates.Invlunerable && _canShoot)
		{
			Shoot();
		}

		_rotationDirection = Input.GetAxis("rotate_left", "rotate_right");
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		var xform = state.Transform;
		xform.Origin.X = Mathf.Wrap(xform.Origin.X, 0, _screenSize.X);
		xform.Origin.Y = Mathf.Wrap(xform.Origin.Y, 0, _screenSize.Y);
		state.Transform = xform;
	}

	public override void _PhysicsProcess(double delta)
	{
		ConstantForce = _thrust * (float)delta;
		ConstantTorque = _rotationDirection * SpinPower * (float)delta;
	}

	private void OnCoolDownTimeout()
	{
		_canShoot = true;
	}

	private void Shoot()
	{
		_canShoot = false;
		GetNode<Timer>("GunCoolDown").Start();
		var bullet = BulletScene.Instantiate<Bullet>();
		GetTree().Root.AddChild(bullet);
		bullet.Start(GetNode<Marker2D>("Muzzle").GlobalTransform);
	}
}
