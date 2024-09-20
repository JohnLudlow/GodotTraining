using Godot;

namespace ZenvaHexMap.Game;

public partial class CityUI : Panel
{
  private City _city;
  Label _cityNameLabel, _cityPopLabel, _cityFoodLabel, _cityProdLabel;

  public City City
  {
    get
    {
      return _city;
    }
    set
    {
      _city = value;

      _cityNameLabel.Text = _city.CityName;
      _cityPopLabel.Text  = $"Population : {_city.Population}";
      _cityFoodLabel.Text = $"Food : {_city.TotalFood}";
      _cityProdLabel.Text = $"Production : {_city.TotalProduction}";
    }
  }

  public override void _Ready()
    {
        _cityNameLabel = GetNode<Label>("CityName");
        _cityPopLabel  = GetNode<Label>("Population");
        _cityFoodLabel = GetNode<Label>("Food");
        _cityProdLabel = GetNode<Label>("Production");
    }
}
