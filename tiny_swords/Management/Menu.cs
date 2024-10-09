using Godot;
using System;

public partial class Menu : Control
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Button button in GetTree().GetNodesInGroup("button"))
		{
			button.Pressed += () => OnButtonPressed(button.Name);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Método chamado quando o botão é pressionado
	private void OnButtonPressed(string buttonName)
	{
		switch (buttonName)
		{
			case "NewGame":
				TransitionScreen.Instance.ScenePath = "res://tiny_swords/Management/level.tscn";
				TransitionScreen.Instance.FadeIn();
				break;

			case "Quit":
				GetTree().Quit();
				break;
		}
	}
}
