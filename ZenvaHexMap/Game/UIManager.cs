using Godot;

namespace HexTileMap;

public partial class UIManager : Node2D
{
  private TerrainTileUI _terrainTileUI;
  private CityUI _cityUI;

  private PackedScene TerrainUIScene { get; } = ResourceLoader.Load<PackedScene>("Game/TerrainTileUI.tscn");
  private PackedScene CityUIScene { get; } = ResourceLoader.Load<PackedScene>("Game/CityUI.tscn");

  private CityUI CityUI
  {

    get => _cityUI;
    set
    {
      HideAllPopups();

      _cityUI?.QueueFree();
      _cityUI = value;
    }
  }

  private TerrainTileUI TerrainTileUI
  {

    get => _terrainTileUI;
    set
    {
      HideAllPopups();

      _terrainTileUI = value;
    }
  }

  public void HideAllPopups()
  {
    _terrainTileUI?.QueueFree();
    _terrainTileUI = null;

    _cityUI?.QueueFree();
    _cityUI = null;
  }

  public void UpdateTerrainInfoUI(Hex hex)
  {
    TerrainTileUI = (TerrainTileUI)TerrainUIScene.Instantiate();
    AddChild(TerrainTileUI);
    TerrainTileUI.Hex = hex;
  }

  public void UpdateCityInfoUI(City city)
  {
    CityUI = (CityUI)CityUIScene.Instantiate();
    AddChild(CityUI);
    CityUI.City = city;
  }
}
