using Godot;
using System;

public partial class TransitionScreen : CanvasLayer
{
	public static TransitionScreen Instance { get; private set; }
	private string _scenePath;
	public string ScenePath
	{
		get { return _scenePath; }
		set { _scenePath = value; }
	}
	private AnimationPlayer _animation;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		_animation = GetNode<AnimationPlayer>("Animation");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void FadeIn()
	{
		_animation.Play("fadeIn");
	}

	private void OnAnimationFinished(StringName anim_name)
	{
		if (anim_name == "fadeIn")
		{
			GetTree().ChangeSceneToFile(_scenePath);
			_animation.Play("fadeOut");
		}
	}
}



