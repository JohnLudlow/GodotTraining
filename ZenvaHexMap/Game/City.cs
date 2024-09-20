using Godot;
using System.Collections.Generic;
using System.Linq;

namespace HexTileMap;

public partial class City : Node2D
{
    public HexTileMap Map { get; set; }
    public Vector2I CityCentreCoordinates { get; set; }

    public List<Hex> CityTerritory { get; } = [];
    public List<Hex> BorderTilePool { get; } = [];

    public int Population { get;set; } = 1;
    public int TotalFood => CityTerritory.Sum(t => t.Food);
    public int TotalProduction => CityTerritory.Sum(t => t.Production);

    public Civilization OwnerCivilization
    {
        get => _ownerCivilization;
        set
        {
            _ownerCivilization = value;

            _sprite ??= GetNode<Sprite2D>("Sprite2D");
            _sprite.Modulate = _ownerCivilization.CivilizationTerritoryColor;
        }
    }

    public string CityName
    {
        get => _cityName; 
        set
        {
            _cityName = value;
            _label ??= GetNode<Label>("Label");
            _label.Text = _cityName;            
        }
    }

    private Sprite2D _sprite;
    private Label _label;
    private Civilization _ownerCivilization;
    private string _cityName;

    public void AddTerritory(IEnumerable<Hex> territoryToAdd)
    {
        foreach(var hex in territoryToAdd)
        {
            hex.OwnerCity = this;
            CityTerritory.Add(hex);
        }
    }
}
