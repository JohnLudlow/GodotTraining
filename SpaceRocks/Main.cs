using Godot;

public partial class Main : Node
{
	[Export]
	public PackedScene RockScene { get; set; }

	private Vector2 _screenSize = Vector2.Zero;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewport().GetVisibleRect().Size;

		for (var i = 0; i < 5; i++)
		{
			SpawnRock(GD.RandRange(3, 5));
		}
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
			if (explodedRock.Size < 1) return;

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

	}
}
