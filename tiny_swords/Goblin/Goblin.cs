using Godot;
using System;

public partial class Goblin : CharacterBody2D
{
	private Vector2 offset = new Vector2(0, 31); // Ajuste o valor conforme necessário
	private CharacterBody2D _playerRef = null;
	private Vector2 _direction;
	private AnimationPlayer _animation;
	private AnimationPlayer _auxAnimation;
	private Sprite2D _texture;
	private CollisionShape2D _collisionBody;
	private static readonly PackedScene ATTACK_AREA_INSTANCE = ResourceLoader.Load<PackedScene>("res://tiny_swords/Goblin/enemy_attack_area.tscn");
	private Area2D _attackArea;
	private float _moveSpeed = 192;
	private float _distance;
	private int _health = 3;
	private float _distanceThreshHold = 60;
	private bool _isAttacking = false;
	private bool _canDie = false;

	[Export]
	public float MoveSpeed
	{
		get { return _moveSpeed; }
		set { _moveSpeed = value; }
	}

	[Export]
	public float DistanceThreshHold
	{
		get { return _distanceThreshHold; }
		set { _distanceThreshHold = value; }
	}

	[Export]
	public int Health
	{
		get { return _health; }
		set { _health = value; }
	}

	public override void _Ready()
	{
		base._Ready();

		_animation = GetNode<AnimationPlayer>("Animation");
		_auxAnimation = GetNode<AnimationPlayer>("AuxAnimation");
		_collisionBody = GetNode<CollisionShape2D>("Collision");
		_texture = GetNode<Sprite2D>("Texture");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_canDie == true)
			return;

		if (_playerRef == null || (_playerRef is Knight knight && knight.CanDie == true))
		{
			Velocity = Vector2.Zero;
			Animate();
			return;
		}

		_direction = GlobalPosition.DirectionTo(_playerRef.GlobalPosition);
		_distance = GlobalPosition.DistanceTo(_playerRef.GlobalPosition);

		if (_distance < _distanceThreshHold)
		{
			if (!_isAttacking)
			{
				_animation.Play("Attack");
				_isAttacking = true;
			}
			return;
		}

		_isAttacking = false;

		Velocity = _direction * _moveSpeed;
		MoveAndSlide();
		Animate();
	}

	private void Animate()
	{

		if (Velocity.X > 0)
		{
			_texture.FlipH = false;

			//var position = _collisionAttack.Position;
			//position.X = 58;
			//_collisionAttack.Position = position;
		}
		else if (Velocity.X < 0)
		{
			_texture.FlipH = true;

			// var position = _collisionAttack.Position;
			// position.X = -58;
			// _collisionAttack.Position = position;
		}

		if (_isAttacking)
		{
			return; // Não interromper a animação de ataque
		}

		if (Velocity != Vector2.Zero)
		{
			_animation.Play("Run");
		}
		else
		{
			_animation.Play("Idle");
		}
	}

	public void UpdateHealth(int damage)
	{
		_health -= damage;
		if (_health <= 0)
		{
			_canDie = true;
			_collisionBody.SetDeferred("disabled", true);
			_animation.Play("Death");
			//_collisionAttack.Disabled = true;
		}
		else
		{
			_auxAnimation.Play("Hit");
		}
	}

	private void SpawnAttackArea()
	{
		_attackArea = ATTACK_AREA_INSTANCE.Instantiate<Area2D>();
		AddChild(_attackArea);

		if (_texture.FlipH)
			offset.X *= -1;

		_attackArea.Position = offset;
	}

	private void OnDetectionAreaBodyEntered(CharacterBody2D body)
	{
		_playerRef = body;
	}

	private void OnDetectionAreaBodyExited(Node2D body)
	{
		if (body == _playerRef)
		{
			_playerRef = null;
			_isAttacking = false;
		}
	}

	private void OnAnimationFinished(StringName anim_name)
	{
		if (anim_name == "Death")
			QueueFree();
	}
}



