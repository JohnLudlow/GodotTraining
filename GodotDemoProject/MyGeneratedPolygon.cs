using System.Linq;
using DelaunatorSharp;
using Godot;


public partial class MyGeneratedPolygon : Polygon2D
{
	RandomNumberGenerator _random = new RandomNumberGenerator();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public override void _Draw()
	{
		var numpoints = _random.RandiRange(10, 15);
		var points = new IPoint[numpoints];

		for(var i = 0; i < numpoints; i++)
		{
			points[i] = new Point(_random.RandfRange(50, 300), _random.RandfRange(50, 300));
		}

		var d = new Delaunator(points);

		d.ForEachVoronoiCell(cell => {
			DrawPolygon(cell.Points.Select(point => new Vector2((float)point.X, (float)point.Y)).ToArray(), new Color[] { new("478cbf") });
		});

		d.ForEachVoronoiEdge(edge => {
			DrawPolyline(new [] { edge.P, edge.Q }.Select(point => new Vector2((float)point.X, (float)point.Y)).ToArray(), new("ffffff"));		
		});
	}
}