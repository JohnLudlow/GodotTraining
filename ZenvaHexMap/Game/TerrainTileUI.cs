using Godot;

namespace HexTileMap;

public partial class TerrainTileUI : Panel
{
    TextureRect _terrainImage;
    Label _terrainLabel, _foodLabel, _productionLabel;
    private Hex _hex = null;


    public Hex Hex
    {
        get => _hex; 
        set
        {
            _hex = value;
            _terrainLabel.Text     = value.TerrainType.ToString();
            _foodLabel.Text        = $"Food: {value.Food.ToString()}";
            _productionLabel.Text  = $"Prodution : {value.Production.ToString()}";
        }
    }

    public override void _Ready()
    {
        _terrainImage = GetNode<TextureRect>("TerrainImage");
        _terrainLabel = GetNode<Label>("TerrainLabel");
        _foodLabel = GetNode<Label>("FoodLabel");
        _productionLabel = GetNode<Label>("ProductionLabel");
    }
}
