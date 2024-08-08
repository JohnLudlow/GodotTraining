using System;
using System.Collections.Generic;
using Godot;

namespace HexTileMap;

public enum TerrainTypes {
    Plains,
    Desert,
    Mountain,
    Forest,
    Beach,
    Ice,
    Water,    
    ShallowWater,
    
}


public record Hex(TerrainTypes TerrainType, Vector2I Coordinates);

public partial class HexTileMap : Node2D
{

    [Export]
    public int Width { get; set; } = 100;

    [Export]
    public int Height { get; set; } = 60;

    private Dictionary<Vector2I, Hex> _mapData;
    private Dictionary<TerrainTypes, Vector2I> _terrainTextures;

    private TileMapLayer _baseLayer, _borderLayer, _overlayLayer;

    private float[,] _noiseMap= new float [100, 60];
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _mapData = [];
        _terrainTextures = new() {
            [TerrainTypes.Plains]        = new Vector2I(0, 0),
            [TerrainTypes.Water]         = new Vector2I(1, 0),

            [TerrainTypes.Desert]        = new Vector2I(0, 1),
            [TerrainTypes.Mountain]      = new Vector2I(1, 1),

            [TerrainTypes.Beach]         = new Vector2I(0, 2),
            [TerrainTypes.ShallowWater]  = new Vector2I(1, 2),

            [TerrainTypes.Ice]           = new Vector2I(0, 3),
            [TerrainTypes.Forest]        = new Vector2I(1, 3),
        };

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

