using Godot;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ZenvaHexMap.Game;

public partial class City : Node2D, INotifyEntityPropertyChanged<EntityUpdatedEventArgs<City>>
{
  public static int PopulationThresholdIncrease {get;} = 15;

  public HexTileMap? Map { get; set; }
  public Vector2I CityCentreCoordinates { get; set; }

  public List<Hex> CityTerritory { get; } = [];
  private List<Hex>? BorderTilePool
  {
    get
    {
      return Map is null
        ? null
        : CityTerritory.SelectMany(h => Map.GetAdjacentHexes(h.Coordinates).Where(h => IsValidNeightbourTile(h)))
                      .Distinct().ToList();
    }
  }

  public int Population { get; set; } = 1;
  private int PopulationGrowthThreshold { get; set; }
  private int PopulationGrowthTracker { get; set; }
  public int TotalFood => CityTerritory.Sum(t => t.Food);
  public int TotalProduction => CityTerritory.Sum(t => t.Production);

  // public Queue<Unit> BuildQueue {get;} = [];
  // public Unit? CurrentlyBuilding => BuildQueue.FirstOrDefault();

  public List<Unit> BuildQueue {get;} = [];
  public Unit? CurrentlyBuilding {get;set;}
  public int UnitProductionCompleted {get;set;}

  public Civilization? OwnerCivilization
  {
    get => _ownerCivilization;
    set
    {
      _ownerCivilization = value;

      _sprite ??= GetNode<Sprite2D>("Sprite2D");
      _sprite.Modulate = _ownerCivilization?.CivilizationTerritoryColor ?? default;
    }
  }

  public string? CityName
  {
    get => _cityName;
    set
    {
      _cityName = value;
      _label = GetNode<Label>("Label");
      _label.Text = _cityName;
    }
  }

  public void ProcessTurn()
  {
    PopulationGrowthTracker += TotalFood;

    if (PopulationGrowthTracker > PopulationGrowthThreshold)
    {
      Population++;
      PopulationGrowthTracker = 0;
      PopulationGrowthThreshold += PopulationThresholdIncrease;

      AddRandomNewTile();
      if (_ownerCivilization is not null)
        Map?.UpdateCivTerritoryMap(_ownerCivilization);

    }

    ProcessUnitBuildQueue();
    PropertyChanged?.Invoke(this, new EntityUpdatedEventArgs<City>(this));
  }

  public void SpawnUnit(Unit unit)
  {
    if (Map is null || OwnerCivilization is null) return;

    var toSpawn = (Unit)Unit.UnitSceneResources[unit.GetType()].Instantiate();
    toSpawn.OwnerCivilization = OwnerCivilization;
    toSpawn.Position = Map.MapToLocal(CityCentreCoordinates);
    toSpawn.UnitCoordinates = CityCentreCoordinates;

    Map.AddChild(toSpawn);
  }

  public void ProcessUnitBuildQueue()
  {
    if (BuildQueue.Count == 0) return;

    CurrentlyBuilding ??= BuildQueue.FirstOrDefault();
    UnitProductionCompleted += TotalProduction;

    if (UnitProductionCompleted >= CurrentlyBuilding?.ProductionRequired)
    {
      SpawnUnit(CurrentlyBuilding);
      BuildQueue.RemoveAt(0);
      CurrentlyBuilding = null;
      UnitProductionCompleted = 0;
    }
  }

  private Sprite2D? _sprite;
  private Label? _label;
  private Civilization? _ownerCivilization;
  private string? _cityName;

  #region Build Queue
  public void AddUnitToBuildQueue(Unit unit)
  {
    BuildQueue.Add(unit);
    PropertyChanged?.Invoke(this, new EntityUpdatedEventArgs<City>(this));
  }

  #endregion

  public event EventHandler<EntityUpdatedEventArgs<City>>? PropertyChanged;

  #region Territory Expansion

  public void AddTerritory(IEnumerable<Hex> territoryToAdd)
  {
    foreach (var hex in territoryToAdd)
    {
      hex.OwnerCity = this;
      CityTerritory.Add(hex);
    }
  }

  private void AddRandomNewTile()
  {
    if (BorderTilePool is not null && BorderTilePool.Count > 0)
    {
      var rand = new Random();
      var index = rand.Next(BorderTilePool.Count);

      AddTerritory([BorderTilePool[index]]);
      BorderTilePool.RemoveAt(index);
    }
  }

  private static bool IsValidNeightbourTile(Hex h)
    => h.OwnerCity is null && (h.TerrainType is not TerrainTypes.Water and not TerrainTypes.Ice and not TerrainTypes.Mountain);

  #endregion
}
