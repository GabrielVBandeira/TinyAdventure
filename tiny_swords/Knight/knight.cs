using Godot;
using System;

public partial class knight : CharacterBody2D
{	

	private AnimationPlayer _animation;
	private Sprite2D _texture;
	private float _moveSpeed = 256.0f;
	private bool _canAttack = true; 

	[Export]
	public float MoveSpeed
	{
		get { return _moveSpeed; }
		set { _moveSpeed = value; }
	}

	public override void _Ready()
	{
		base._Ready();

		_animation = GetNode<AnimationPlayer>("Animation");
		_texture = GetNode<Sprite2D>("Texture");
	}

	public override void _PhysicsProcess(double delta)
	{
		if(_canAttack == false){
			return;
		}

		Move();
		Animate();
		AttackHandler();
	}

	public void Move(){
		Vector2 direction = GetDirection();
		
		Vector2 velocity = direction * MoveSpeed;
		Velocity = velocity;
		MoveAndSlide();
	}

	public void Animate()
	{
		if(Velocity.X > 0){
			_texture.FlipH = false;
		}else if(Velocity.X < 0){
			_texture.FlipH = true;
		}

		if (Velocity != Vector2.Zero)
		{
			_animation.Play("run");
		}
		else
		{
			_animation.Play("idle");
		}
	}

	public void AttackHandler(){
		if (Input.IsActionPressed("attack") && _canAttack){
			_canAttack = false;
			_animation.Play("attack");
		}
	}

	public Vector2 GetDirection() {
		float x = Input.GetAxis("move_left", "move_right");
		float y = Input.GetAxis("move_up", "move_down");
		Vector2 direction = new Vector2(x, y);
		return direction.Normalized();
	}

	private void _on_animation_finished(StringName anim_name)
	{
		_canAttack = true;
	}
}

