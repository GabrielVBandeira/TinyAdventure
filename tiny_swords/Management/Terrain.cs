using Godot;
using System;

public partial class Terrain : Node2D
{
    private TileMap _terrain;
    private TileMap _water;
    private Rect2 _grassRect;
    private Vector2I[] _neighborsList;
    private Vector2I _upNeighbor;
    private Vector2I _downNeighbor;
    private Vector2I _leftNeighbor;
    private Vector2I _rightNeighbor;
    Godot.Collections.Array<Godot.Vector2I> _grassCells;
    private const int DEFAULT_LAYER = 0;
    private static readonly PackedScene FOAM = ResourceLoader.Load<PackedScene>("res://tiny_swords/Management/foam.tscn");

    public override void _Ready()
    {
        base._Ready();

        _terrain = GetNode<TileMap>("Grass");
        _water = GetNode<TileMap>("Water");
        _grassRect = _terrain.GetUsedRect();
        _grassCells = _terrain.GetUsedCells(DEFAULT_LAYER);
        GenerateWaterTiles(_grassRect);
        GenerateFoamTiles();
    }

    private void CheckGrassNeighbor(Vector2I cell)
    {
        _leftNeighbor = new Vector2I(cell.X - 1, cell.Y);
        _rightNeighbor = new Vector2I(cell.X + 1, cell.Y);
        _upNeighbor = new Vector2I(cell.X, cell.Y + 1);
        _downNeighbor = new Vector2I(cell.X, cell.Y - 1);
    }

    private void GenerateFoamTiles()
    {
        foreach (var cell in _grassCells)
        {
            CheckGrassNeighbor(cell);
        }
    }


    private void GenerateWaterTiles(Rect2 usedRect)
    {
        for (float x = usedRect.Position.X - 10; x < usedRect.Position.X + usedRect.Size.X + 10; x++)
        {
            for (float y = usedRect.Position.Y - 10; y < usedRect.Position.Y + usedRect.Size.Y + 10; y++)
            {
                Vector2I cell = new Vector2I((int)x, (int)y);

                // Verifica se _grassCells contém a célula atual
                if (_grassCells.Contains(cell))
                    continue;

                _water.SetCell(DEFAULT_LAYER, new Vector2I((int)x, (int)y), DEFAULT_LAYER, new Vector2I(0, 0));
            }
        }
    }


}
