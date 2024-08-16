using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Grid : Node2D
{
    private Tile[,] _grid;
    private PackedScene _sceneTile;

    [Signal]
    public delegate void ScoreUpdateEventHandler(int value);

    [Signal]
    public delegate void GameOverEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sceneTile = ResourceLoader.Load<PackedScene>("res://Tile/Tile.tscn");

        _grid = new Tile[4, 4];
        PopulateStartingTiles();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        var moved = false;
        if (@event.IsActionPressed("up"))
        {
            moved = MoveTiles("up");
        }
        if (@event.IsActionPressed("down"))
        {
            moved = MoveTiles("down");
        }
        if (@event.IsActionPressed("left"))
        {
            moved = MoveTiles("left");
        }
        if (@event.IsActionPressed("right"))
        {
            moved = MoveTiles("right");
        }

        if (moved)
        {
            SpawnRandomTile();
        }
    }

    private bool MoveTiles(string direction)
    {
        var isHorizontal = direction == "left" || direction == "right";
        var isReverse = direction == "up" || direction == "left";
        var hasMoved = false;

        var mergeCoords = new Dictionary<Tile, Vector2>();
        var originalPositions = new Dictionary<Tile, Vector2>();

        var points = 0;

        for (var i = 0; i < 4; i++)
        {
            var tiles = new Stack<Tile>();

            for (var j = 0; j < 4; j++)
            {
                var x = isHorizontal ? (isReverse ? 3 - j : j) : i;
                var y = isHorizontal ? i : (isReverse ? 3 - j : j);

                if (_grid[x, y] is not null)
                {
                    originalPositions[_grid[x, y]] = new Vector2(x, y);

                    tiles.Push(_grid[x, y]);
                    _grid[x, y] = null;
                }
            }

            var newIdx = isReverse ? 0 : 3;
            while (tiles.Count > 0)
            {
                var currentTile = tiles.Pop();
                var nextTile = tiles.Count > 0 ? tiles.Peek() : null;
                var merged = default(Tile);

                if (nextTile?.Value == currentTile.Value)
                {
                    points += currentTile.Value * 2;

                    merged = tiles.Pop();
                    currentTile.Value *= 2;
                    hasMoved = true;

                }

                if (isHorizontal)
                {
                    _grid[newIdx, i] = currentTile;
                    if (merged is not null)
                    {
                        mergeCoords.Add(merged, ArrayToTileCoords(new Vector2(newIdx, i)));
                    }
                }
                else 
                {
                    _grid[i, newIdx] = currentTile;
                    if (merged is not null)
                    {
                        mergeCoords.Add(merged, ArrayToTileCoords(new Vector2(i, newIdx)));
                    }
                }

                newIdx += isReverse ? 1 : -1;
            }
        }

        if (!hasMoved)
        {
            if (originalPositions.Any(kvp => _grid[(int)kvp.Value.X, (int)kvp.Value.Y] != kvp.Key))
            {
                hasMoved = true;
            }
        }

        for (var x = 0; x < 4; x++)
        {
            for (var y = 0; y < 4; y++)
            {
                if (_grid[x, y] is Tile tile)
                {
                    var tween = tile.CreateTween();
                    tween.TweenProperty(tile, "position", ArrayToTileCoords(new Vector2(x, y)), 0.1f);
                }
            }
        }

        foreach (var tile in mergeCoords.Keys)
        {
            var coords = mergeCoords[tile];
            var tween = tile.CreateTween();
            tween.TweenProperty(tile, "position", coords, 0.1f);
            tween.TweenCallback(Callable.From(() => tile.QueueFree()));
        }

        EmitSignal(SignalName.ScoreUpdate, points);

        if (IsGameOver)
        {
            EmitSignal(SignalName.GameOver);
        }

        return hasMoved;
    }

    public void SpawnRandomTile()
    {
        var spaces = new List<Vector2I>();

        for (var x = 0; x < 4; x++)
        {
            for (var y = 0; y < 4; y++)
            {
                if (_grid[x, y] is null)
                {
                    spaces.Add(new Vector2I(x, y));
                }
            }
        }

        if (spaces.Count > 0)
        {
            var rand = new Random();
            var select = rand.Next(0, spaces.Count);
            SpawnTile(spaces[select].X, spaces[select].Y);
        }
    }

    public void SpawnTile(int x, int y)
    {
        var rand = new Random();
        var newTile = _sceneTile.Instantiate() as Tile;
        newTile.Position =ArrayToTileCoords(new Vector2(x, y));
        newTile.Value = rand.Next(0, 10) < 7 ? 2 : 4; 

        _grid[x, y] = newTile;

        AddChild(newTile);
    }

    private static Vector2 ArrayToTileCoords(Vector2 arraycoords) => new(arraycoords.X * 115 + 15, arraycoords.Y * 115 + 15);

    private void PopulateStartingTiles()
    {
        var rand = new Random();
        var tile1coords = new Vector2(rand.Next(0, 4), rand.Next(0, 4));
        var tile2coords = new Vector2(rand.Next(0, 4), rand.Next(0, 4));

        while (tile1coords.X == tile2coords.X && tile1coords.Y == tile2coords.Y)
        {
            tile1coords = new Vector2(rand.Next(0, 4), rand.Next(0, 4));
            tile2coords = new Vector2(rand.Next(0, 4), rand.Next(0, 4));
        }

        var t1 = _sceneTile.Instantiate() as Tile;
        t1.Value = 2;
        t1.Position = ArrayToTileCoords(tile1coords);
        AddChild(t1);

        _grid[(int)tile1coords.X, (int)tile1coords.Y] = t1;

        var t2 = _sceneTile.Instantiate() as Tile;
        t2.Value = 2;
        t2.Position = ArrayToTileCoords(tile2coords);
        AddChild(t2);

        _grid[(int)tile2coords.X, (int)tile2coords.Y] = t2;
    }

    private bool IsGameOver
    {
        get 
        {
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {                
                    if (_grid[x, y] is null) return false;
                    
                    var adj =  new (int x, int y)[] {
                        (x + 1, y),
                        (x - 1, y),
                        (x, y + 1),
                        (x, y - 1),
                    };
                    if(adj.Any(a => 
                        a.x >= 0 && a.x < 4 && 
                        a.y >= 0 && a.y < 4 && 
                        (_grid[a.x, a.y] is null || _grid[a.x, a.y].Value == _grid[x, y].Value)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public void Reset()
    {
        for (var x = 0; x < 4; x++)
        {
            for (var y = 0; y < 4; y++)
            {                
                _grid[x, y]?.QueueFree();
                _grid[x, y] = null;
            }
        }

        PopulateStartingTiles();
    }
}
