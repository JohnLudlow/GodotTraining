using Godot;
using System;

namespace HexTileMap;

public partial class HexTileMap : Node2D
{
    [Export]
    public int Width { get; set; } = 100;

    [Export]
    public int Height { get; set; } = 60;

    private TileMapLayer _baseLayer, _borderLayer, _overlayLayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _baseLayer = GetNode<TileMapLayer>("BaseLayer");
        _borderLayer = GetNode<TileMapLayer>("HexBordersLayer");
        _overlayLayer = GetNode<TileMapLayer>("SelectionOverlayLayer");

        GenerateTerrain();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void GenerateTerrain()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Width; y++)
            {
                _baseLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
                _borderLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
            }
        }
    }

    public Vector2 MapToLocal(Vector2I coords)
    {
        return _baseLayer.MapToLocal(coords);
    }

}

