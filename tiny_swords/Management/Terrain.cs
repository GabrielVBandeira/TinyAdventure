using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Terrain : Node2D
{
	private TileMap _terrain;
	private TileMap _water;
	private Rect2 _grassRect;
	private List<Vector2I> _neighborsList;
	private Vector2I _upNeighbor;
	private Vector2I _downNeighbor;
	private Vector2I _leftNeighbor;
	private Vector2I _rightNeighbor;
	private List<Vector2I> _waterUsedCells;
	Godot.Collections.Array<Godot.Vector2I> _grassUsedCells;
	private const int DEFAULT_LAYER = 0;
	private static readonly PackedScene FOAM = ResourceLoader.Load<PackedScene>("res://tiny_swords/Management/foam.tscn");

	public override void _Ready()
	{
		base._Ready();

		_terrain = GetNode<TileMap>("Grass");
		_water = GetNode<TileMap>("Water");
		_grassRect = _terrain.GetUsedRect();
		_grassUsedCells = _terrain.GetUsedCells(DEFAULT_LAYER);
		GenerateWaterTiles(_grassRect);
		GenerateFoamTiles();
	}

	private bool CheckGrassNeighbor(Vector2I cell)
	{
		_leftNeighbor = new Vector2I(cell.X - 1, cell.Y);
		_rightNeighbor = new Vector2I(cell.X + 1, cell.Y);
		_upNeighbor = new Vector2I(cell.X, cell.Y + 1);
		_downNeighbor = new Vector2I(cell.X, cell.Y - 1);

		_neighborsList = new List<Vector2I> { _leftNeighbor, _rightNeighbor, _upNeighbor, _downNeighbor };

		foreach (var neighbor in _neighborsList)
		{
			if (_waterUsedCells.Contains(neighbor))
			{
				return true;
			}
		}

		// Se nenhum vizinho está na água, retorna falso
		return false;
	}

	private void GenerateFoamTiles()
	{
		foreach (var cell in _grassUsedCells)
		{
			if (CheckGrassNeighbor(cell))
			{
				SpawnFoams(cell);
			};
		}
	}

	private void SpawnFoams(Vector2I foamCell)
	{
		var foam = FOAM.Instantiate<Godot.Node2D>(); // Certifique-se de que o tipo de 'foam' seja Node2D ou um derivado.
		AddChild(foam);

		foam.Position = foamCell * 64; // Multiplica a célula pela dimensão do tile
	}


	private void GenerateWaterTiles(Rect2 usedRect)
	{
		for (float x = usedRect.Position.X - 10; x < usedRect.Position.X + usedRect.Size.X + 10; x++)
		{
			for (float y = usedRect.Position.Y - 10; y < usedRect.Position.Y + usedRect.Size.Y + 10; y++)
			{
				Vector2I cell = new Vector2I((int)x, (int)y);

				// Verifica se _grassCells contém a célula atual
				if (_grassUsedCells.Contains(cell))
					continue;

				_water.SetCell(DEFAULT_LAYER, new Vector2I((int)x, (int)y), DEFAULT_LAYER, new Vector2I(0, 0));
			}
		}
		_waterUsedCells = _water.GetUsedCells(DEFAULT_LAYER).ToList();
	}

}
