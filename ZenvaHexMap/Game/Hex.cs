using Godot;

namespace HexTileMap;

public class Hex(TerrainTypes _terrainType, Vector2I _coordinates, int _food = 0, int _production = 0)
{
    public TerrainTypes TerrainType { get; } = _terrainType;
    public Vector2I Coordinates { get; } = _coordinates;
    public int Food { get; set; } = _food;
    public int Production { get; set; } = _production;

    public City OwnerCity { get; set; } = null;
    public bool IsCityCenter { get; set; } = false;

    public override string ToString()
    {
        return $"{TerrainType} at {Coordinates} => Food {Food}, Production {Production}";
    }
}

