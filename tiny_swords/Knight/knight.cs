using Godot;
using System;

public partial class knight : CharacterBody2D
{	

	private float _moveSpeed = 256.0f;

	[Export]
	public float MoveSpeed
	{
		get { return _moveSpeed; }
		set { _moveSpeed = value; }
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = GetDirection();
		
		Vector2 velocity = direction * MoveSpeed;
		Velocity = velocity;
		MoveAndSlide();
	}

	public Vector2 GetDirection() {
		float x = Input.GetAxis("move_left", "move_right");
		float y = Input.GetAxis("move_up", "move_down");
		Vector2 direction = new Vector2(x, y);
		return direction.Normalized();
	}
}
