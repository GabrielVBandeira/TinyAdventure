using Godot;
using System;

public partial class knight : CharacterBody2D
{	

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = get_direction();
		GD.Print(direction.Length());
	}

	public Vector2 get_direction() {
		float x = Input.GetAxis("ui_left", "ui_right");
		float y = Input.GetAxis("ui_up", "ui_down");
		Vector2 direction = new Vector2(x, y);
		return direction.Normalized();
	}
}
