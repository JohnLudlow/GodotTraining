using System.Linq;
using Godot;

namespace GodotTraining.SpaceRocks;

public partial class Main : Node
{
	private object locker = new object();

	[Export]
	public PackedScene RockScene { get; set; }

	private Vector2 _screenSize = Vector2.Zero;

	private int _level = 0;
	private int _score = 0;
	private bool _playing = false;

	private HeadsUpDisplay _hud;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewport().GetVisibleRect().Size;

		_hud = GetNode<HeadsUpDisplay>("HUD");
	}

	public async void NewGame(){
		GetTree().CallGroup("rocks", "queue_free");
		_level = 0;
		_score = 0;
		_hud.UpdateScore(_score);
		await _hud.ShowMessage("Get Ready!");

		GetNode<Player>("Player").Reset();

		ToSignal(GetNode<Timer>("HUD/Timer"), Timer.SignalName.Timeout).GetResult();
		_playing = true;
	}

	public async void NewLevel()
	{
		lock (locker)
		{
			for (var i = 0; i < _level; i++)
			{
				SpawnRock(GD.RandRange(3, 5));
			}

			_level++;
		}

		await _hud.ShowMessage($"Wave {_level}");
	}

	public void GameOver()
	{
		_playing = false;
		GetNode<HeadsUpDisplay>("HUD").GameOver();
	}

	private Vector2 GetRandomRockPosition()
	{
		var rockSpawn = GetNode<PathFollow2D>("RockPath/RockSpawn");
		rockSpawn.Progress = GD.Randi();
		return rockSpawn.Position;
	}

    private static Vector2 GetRandomRockVelocity() 
		=> Vector2.Right.Rotated(
                (float)(GD.RandRange(0, Mathf.Tau) * GD.RandRange(50, 125))
            );

    private void SpawnRock(int size, Vector2? position = null, Vector2? velocity = null)
	{
		var rock = RockScene.Instantiate<Rock>();
		rock.ScreenSize = _screenSize;
		rock.Start(
			position ?? GetRandomRockPosition(), 
			velocity ?? GetRandomRockVelocity(),
			size
		);

		rock.Exploded += explodedRock => {
			if (explodedRock.Size <= 1) return;

			var player = GetNode<Player>("Player");

            void spawn_rock(int offset)
            {
                var direction = player.Position.DirectionTo(explodedRock.Position).Orthogonal() * offset;

                SpawnRock(
                    size - 1,
                    explodedRock.Position + direction * explodedRock.Radius,
                    direction
                );
            }

            spawn_rock(-1);
			spawn_rock(1);
		};

		CallDeferred("add_child", rock);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!_playing) return;

		if (!GetTree().GetNodesInGroup("rocks").Any())
		{
			NewLevel();
		}
	}
}
