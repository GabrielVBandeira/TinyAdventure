using Godot;
using System;

public partial class Knight : CharacterBody2D
{

	private AnimationPlayer _animation;
	private Sprite2D _texture;
	private CollisionShape2D _collisionAttack;
	private CollisionShape2D _collisionBody;
	private AnimationPlayer _auxAnimation;
	private float _moveSpeed = 256.0f;
	private bool _canAttack = true;
	private bool _canDie = false;
	private int _damage = 1;
	private int _health = 5;

	public bool CanDie
	{
		get { return _canDie; }
		set { _canDie = value; }
	}

	[Export]
	public int Health
	{
		get { return _health; }
		set { _health = value; }
	}

	[Export]
	public float MoveSpeed
	{
		get { return _moveSpeed; }
		set { _moveSpeed = value; }
	}

	[Export]
	public int Damage
	{
		get { return _damage; }
		set { _damage = value; }
	}

	public override void _Ready()
	{
		base._Ready();

		_animation = GetNode<AnimationPlayer>("Animation");
		_texture = GetNode<Sprite2D>("Texture");
		_collisionAttack = GetNode<CollisionShape2D>("AttackArea/Collision");
		_collisionBody = GetNode<CollisionShape2D>("Collision");
		_auxAnimation = GetNode<AnimationPlayer>("AuxAnimationPlayer");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_canAttack == false || _canDie == true)
		{
			return;
		}

		Move();
		Animate();
		AttackHandler();
	}

	public void Move()
	{
		Vector2 direction = GetDirection();

		Vector2 velocity = direction * MoveSpeed;
		Velocity = velocity;
		MoveAndSlide();
	}

	public void Animate()
	{
		if (Velocity.X > 0)
		{
			_texture.FlipH = false;

			var position = _collisionAttack.Position;
			position.X = 58;
			_collisionAttack.Position = position;
		}
		else if (Velocity.X < 0)
		{
			_texture.FlipH = true;

			var position = _collisionAttack.Position;
			position.X = -58;
			_collisionAttack.Position = position;
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

	public void AttackHandler()
	{
		if (Input.IsActionPressed("attack") && _canAttack)
		{
			_canAttack = false;
			_animation.Play("attack");
		}
	}

	public Vector2 GetDirection()
	{
		float x = Input.GetAxis("move_left", "move_right");
		float y = Input.GetAxis("move_up", "move_down");
		Vector2 direction = new Vector2(x, y);
		return direction.Normalized();
	}

	public void UpdateHealth(int damage)
	{
		_health -= damage;
		if (_health <= 0)
		{
			_canDie = true;
			_collisionAttack.SetDeferred("disabled", true);
			_collisionBody.SetDeferred("disabled", true);
			_animation.Play("death");
		}
		else
		{
			_auxAnimation.Play("hit");
		}
	}

	private void OnAnimationFinished(StringName anim_name)
	{
		switch (anim_name)
		{
			case "attack":
				_canAttack = true;
				break;

			case "death":
				GetTree().ReloadCurrentScene();
				break;

		}
	}

	private void OnAttackAreaBodyEntered(Node2D body)
	{
		if (body is Goblin goblin)
		{
			goblin.UpdateHealth(_damage);
			GD.Print("Goblin Health: " + goblin.Health);
		}
	}

}
