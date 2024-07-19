using System.Diagnostics;

using Godot;

using GodotTraining.SpaceRocks;

public partial class Player : RigidBody2D
{
	private PlayerStates _state = PlayerStates.Init;

	private Vector2 _thrust = Vector2.Zero;
	private Vector2 _screenSize = Vector2.Zero;
	private float _rotationDirection = 0;
	private bool _canShoot = true;
	private bool _resetPosition = false;
    private int _lives;

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

			var collider = GetNode<CollisionShape2D>("CollisionShape2D");
			var sprite = GetNode<Sprite2D>("Sprite2D");
			var modulate = sprite.Modulate;

            switch (_state)
            {
                case PlayerStates.Init:
					collider.SetDeferred("disabled", true);
					modulate.A = .5f;

                    break;

                case PlayerStates.Alive:
					collider.SetDeferred("disabled", false);
					modulate.A = 1.0f;

                    break;

                case PlayerStates.Invlunerable:
					collider.SetDeferred("disabled", true);
					modulate.A = .5f;
					GetNode<Timer>("InvulnerablityTimer").Start();

                    break;

                case PlayerStates.Dead:
					collider.SetDeferred("disabled", true);
					sprite.Hide();
					LinearVelocity = Vector2.Zero;
					EmitSignal(SignalName.PlayerDied);

                    break;
            }

            GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", _state == PlayerStates.Alive);
        }
    }

	public void OnInvulnerabilityTimerTimeout()
	{
		State = PlayerStates.Alive;
	}

	public void OnBodyEntered(Node body)
	{
		Debug.WriteLine("collided with something!");
		if (body.IsInGroup("rocks") && body is Rock rock)
		{
			rock.Explode();
			Lives--;
			Explode();
		}
	}


	public async void Explode()
	{
		var explosionPlayer = GetNode<AnimationPlayer>("Explosion/AnimationPlayer");
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
		GetNode<Sprite2D>("Sprite2D").Hide();
		GetNode<Sprite2D>("Explosion").Show();

		await ToSignal(explosionPlayer, AnimationPlayer.SignalName.AnimationFinished);
		explosionPlayer.Play("Explosion");

		GetNode<Sprite2D>("Explosion").Hide();
	}

    [Export]
	public int EnginePower { get; set; } = 15000;

	[Export]
	public int SpinPower { get; set; } = 500;

	[Export]
	public PackedScene BulletScene { get; set; }

	[Export]
	public float FireRate { get; set; } = 0.25f;

	[Signal]
	public delegate void LivesChangedEventHandler(int lives);

	[Signal]
	public delegate void PlayerDiedEventHandler();

    public int Lives { 
		get => _lives; 
		set {
			_lives = value; 
			if(_lives <= 0)
            {
                EmitSignal(SignalName.PlayerDied);
            }
            else
            {
                EmitSignal(SignalName.LivesChanged, _lives);
            }
        }
	}

	public void Reset()
	{
		_resetPosition = true;
		GetNode<Sprite2D>("Sprite2D").Show();
		_lives = 3;
		State = PlayerStates.Alive;
	}

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
		if (_resetPosition)
		{
			xform.Origin = _screenSize / 2;
			_resetPosition = false;
		}

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
