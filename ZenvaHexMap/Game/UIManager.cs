using Godot;

namespace HexTileMap;

public partial class UIManager : Node2D
{
    private TerrainTileUI _terrainTileUI;

    private PackedScene TerrainUIScene {get;set;}

    private TerrainTileUI TerrainTileUI
    {

        get => _terrainTileUI; 
        set
        {
            _terrainTileUI?.QueueFree();
            _terrainTileUI = value;
        }
    }

    public override void _Ready()
    {
        base._Ready();

        TerrainUIScene = ResourceLoader.Load<PackedScene>("Game/TerrainTileUI.tscn");
    }


    public void HideAllPopups()
    {
        _terrainTileUI?.QueueFree();
        _terrainTileUI = null;
    }

    public void UpdateTerrainInfoUI(Hex hex)
    {
        TerrainTileUI = (TerrainTileUI)TerrainUIScene.Instantiate();
        AddChild(TerrainTileUI);
        TerrainTileUI.Hex = hex;
    }
}
