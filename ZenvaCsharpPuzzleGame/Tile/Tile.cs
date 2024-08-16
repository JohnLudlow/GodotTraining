using Godot;

public partial class Tile : Polygon2D
{
    private int _value  = 0;
    public int Value 
    {
        get => _value; 
        set 
        { 
            _value = value; 

            _label ??= GetNode<Label>("Label");
            _label.Text = value.ToString();

            UpdateColor(value);
        }
    }
    private Label _label;

    private void UpdateColor(int value)
    {
        switch (value)
        {
            case(2):
                Color = new Color("eee3da");
                break;
            case(4):
                Color = new Color("eddfc8");
                break;
            case(8):
                Color = new Color("f2b178");
                break;
            case(16):
                Color = new Color("f59562");
                break;
            case(32):
                Color = new Color("f57c5f");
                break;
            case(64):
                Color = new Color("f65e3a");
                break;
            case(128):
                Color = new Color("edcf73");
                break;
            case(256):
                Color = new Color("edcc61");
                break;
            case(512):
                Color = new Color("edc750");
                break;
            case(1024):
                Color = new Color("edc53e");
                break;
            case(2048):
                Color = new Color("edc22d");
                break;
        }
    }

    private void PlaySpawnAnimation()
    {
        Scale = Vector2.Zero;
        Position += new Vector2(50, 50);

        var scaleAnimation = CreateTween();
        var positionAnimation = CreateTween();

        scaleAnimation.TweenProperty(this, "scale", Vector2.One, .2f);
        positionAnimation.TweenProperty(this, "position", Position - new Vector2(50, 50), .2f);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        PlaySpawnAnimation();        
	}
}
