using Godot;
using System;

public partial class enemy_attack_area : Area2D
{
	private int _damage = 1;

	[Export]
	public int Damage
	{
		get { return _damage; }
		set { _damage = value; }
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Knight knight && !knight.CanDie)
		{
			knight.UpdateHealth(_damage);
			GD.Print("Player Health: " + knight.Health);
		}
	}

	private void OnLifetimeTimeout()
	{
		QueueFree();
	}
}




