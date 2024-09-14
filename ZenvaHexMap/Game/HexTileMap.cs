using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace HexTileMap;

public enum TerrainTypes {
    Water,
    Plains,
    Desert,
    Mountain,
    Forest,
    Beach,
    ShallowWater,
    Ice,    
}


public class Hex(TerrainTypes _terrainType, Vector2I _coordinates, int _food = 0, int _production = 0)
{
    public TerrainTypes TerrainType { get; } = _terrainType;
    public Vector2I Coordinates { get; } = _coordinates;
    public int Food { get; set; } = _food;
    public int Production { get; set; } = _production;

    public override string ToString()
    {
        return $"{TerrainType} at {Coordinates} => Food {Food}, Production {Production}";
    }
}


public partial class HexTileMap : Node2D
{
    [Export]
    public int Width { get; set; } = 100;

    [Export]
    public int Height { get; set; } = 60;

    private Dictionary<Vector2I, Hex> _mapData;

    private Dictionary<TerrainTypes, Vector2I> _terrainTextures;

    private TileMapLayer _baseLayer, _borderLayer, _overlayLayer;

    private Vector2I _selectedCell = new(-1, -1);

    private UIManager _uiManager;

    public delegate void SendHexDataEventHandler(Hex hex);

    public event SendHexDataEventHandler SendHexData;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _uiManager = GetNode<UIManager>("/root/Game/CanvasLayer/UIManager");
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
        GenerateResources();

        SendHexData += _uiManager.UpdateTerrainInfoUI;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            var mapCoords = _baseLayer.LocalToMap(GetLocalMousePosition());

            if (_mapData.TryGetValue(mapCoords, out var hex))
            {                
                if (mouseButton.ButtonMask == MouseButtonMask.Left)
                {
                    GD.Print(hex);

                    SendHexData?.Invoke(hex);

                    if (mapCoords != _selectedCell)
                    {
                        _overlayLayer.SetCell(_selectedCell, -1);
                    }

                    _overlayLayer.SetCell(mapCoords, 0, new (0, 1));
                    _selectedCell = mapCoords;
                }
            }
            else
            {
                _overlayLayer.SetCell(_selectedCell, -1);
            }
        }
    }

    public void GenerateResources()
    {
        var r = new Random();
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var h = _mapData[new(x, y)];

                var (food, production) = h.TerrainType switch
                {
                    TerrainTypes.Plains => (r.Next(2, 6), r.Next(0, 3)),
                    TerrainTypes.Desert => (r.Next(0, 2), r.Next(0, 2)),
                    TerrainTypes.Forest => (r.Next(1, 4), r.Next(2, 6)),
                    TerrainTypes.Beach => (r.Next(0, 4), r.Next(0, 2)),
                    _ => (0, 0),
                };

                h.Food = food;
                h.Production = production;
            }
        }
    }

    public void GenerateTerrain()
    {
        var rand = new Random();
        var seed = rand.Next(100000);

        var (baseNoiseMax, baseNoiseMap) = MakeSomeNoise(
            new FastNoiseLite
            {
                NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
                Seed = seed,
                Frequency = 0.008f,
                FractalType = FastNoiseLite.FractalTypeEnum.Fbm,
                FractalOctaves = 4,
                FractalLacunarity = 2.25f
            }
        );
        
        var baseTerrainRanges = new[] {
            (0                        , baseNoiseMax / 10 * 2.5f , TerrainTypes.Water) ,
            (baseNoiseMax / 10 * 2.5f , baseNoiseMax / 10 * 4f   , TerrainTypes.ShallowWater) ,
            (baseNoiseMax / 10 * 4f   , baseNoiseMax / 10 * 4.5f , TerrainTypes.Beach) ,
            (baseNoiseMax / 10 * 4.5f , baseNoiseMax + .05f      , TerrainTypes.Plains) ,
        };

        DrawTerrain(baseNoiseMap, baseTerrainRanges);

        var (forestNoiseMax, forestNoiseMap) = MakeSomeNoise(
            new FastNoiseLite
            {
                NoiseType = FastNoiseLite.NoiseTypeEnum.Cellular,
                Seed = seed,
                Frequency = 0.04f,
                FractalType = FastNoiseLite.FractalTypeEnum.Fbm,
                FractalLacunarity = 2f
            }
        );

        DrawOverrideTerrain(baseNoiseMap, baseTerrainRanges, forestNoiseMap, forestNoiseMax / 10 * 7 , forestNoiseMax + .05f , TerrainTypes.Forest);

        var (desertNoiseMax, desertNoiseMap) = MakeSomeNoise(
            new FastNoiseLite
            {
                NoiseType = FastNoiseLite.NoiseTypeEnum.SimplexSmooth,
                Seed = seed,
                Frequency = 0.06f,
                FractalType = FastNoiseLite.FractalTypeEnum.Fbm,
                FractalLacunarity = 2f
            }
        );

        DrawOverrideTerrain(baseNoiseMap, baseTerrainRanges, desertNoiseMap, desertNoiseMax / 10 * 5 , desertNoiseMax + .05f , TerrainTypes.Desert);

        var (mountainNoiseMax, mountainNoiseMap) = MakeSomeNoise(
            new FastNoiseLite
            {
                NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
                Seed = seed,
                Frequency = 0.02f,
                FractalType = FastNoiseLite.FractalTypeEnum.Ridged,
                FractalLacunarity = 2f
            }
        );

        DrawOverrideTerrain(baseNoiseMap, baseTerrainRanges, mountainNoiseMap, mountainNoiseMax / 10 * 5 , mountainNoiseMax + .05f , TerrainTypes.Mountain);

        DrawIceCap(rand);
    }

    private void DrawIceCap(Random random, int maxIce = 5)
    {
        for(var x = 0; x < Width; x++)
        {
            for (var y = 0; y < random.Next(1, maxIce) + 1; y++)
            {
                _baseLayer.SetCell(new Vector2I(x, y), 0, _terrainTextures[TerrainTypes.Ice]);
            }

            for (var y = Height - 1; y > Height - random.Next(1, maxIce) - 1; y--)
            {
                _baseLayer.SetCell(new Vector2I(x, y), 0, _terrainTextures[TerrainTypes.Ice]);
            }
        }
    }

    private void DrawOverrideTerrain(
        float[,] baseNoiseMap,
        IEnumerable<(float min, float max, TerrainTypes terrainType)> baseTerrainRanges,
        float[,] overrideNoiseMap,
        float overrideMin,
        float overrideMax,
        TerrainTypes overrideTerrainType)
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var terrainType = baseTerrainRanges.First(range =>
                    baseNoiseMap[x, y] >= range.min &&
                    baseNoiseMap[x, y] < range.max
                ).terrainType;

                if (
                    overrideNoiseMap[x, y] >= overrideMin && 
                    overrideNoiseMap[x ,y] <  overrideMax && 
                    terrainType == TerrainTypes.Plains
                )
                {
                    var hex = new Hex(overrideTerrainType, new Vector2I(x, y));
                    _mapData[hex.Coordinates] = hex;

                    _baseLayer.SetCell(hex.Coordinates, 0, _terrainTextures[hex.TerrainType]);
                    _borderLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
                }
            }
        }
    }    

    private void DrawTerrain(float[,] noiseMap, IEnumerable<(float min, float max, TerrainTypes terrainType)> terrainRanges)
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            { 
                var terrainType = terrainRanges.First(range =>
                    noiseMap[x, y] >= range.min &&
                    noiseMap[x, y] < range.max
                ).terrainType;

                var hex = new Hex(terrainType, new Vector2I(x, y));
                _mapData[hex.Coordinates] = hex;

                _baseLayer.SetCell(hex.Coordinates, 0, _terrainTextures[terrainType]);
                _borderLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
            }
        }
    }

    private (float noiseMax, float[,] noiseMap) MakeSomeNoise(FastNoiseLite noise)
    {
        var noiseMap = new float[Width, Height];
        var noiseMax = 0f;

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                noiseMap[x, y] = Math.Abs(noise.GetNoise2D(x, y));
                if (noiseMap[x, y] > noiseMax)
                {
                    noiseMax = noiseMap[x, y];
                }
            }
        }

        return (noiseMax, noiseMap);
    }

    public Vector2 MapToLocal(Vector2I coords)
    {
        return _baseLayer.MapToLocal(coords);
    }

}

