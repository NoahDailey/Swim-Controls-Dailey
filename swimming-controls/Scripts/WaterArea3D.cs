using Godot;
using System;

// Script that keeps track of which movement scheme to use
public partial class WaterArea3D : Area3D
{
	[Export]
	public CharacterBody3D player;

	// Strings for the scripts to go under
	[ExportGroup("File Paths")]
	[Export]
	public string swimmingScriptPath = "res://Scripts/SwimmingController.cs";
	[Export]
	public string mainPlayerScriptPath = "res://player-controller-8c3769cae34a3c111a3c9f4d97a88c22b113b0c5/addons/player_controller/Scripts/PlayerController.cs";

	// Things to do upon startup of the game
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node3D body)
	{
		GD.Print($"Detected: {body.Name}; expected player: {player?.Name}");
		if(body == player)
		{
			var swimmingScript = GD.Load(swimmingScriptPath);
			body.SetScript(swimmingScript);
			GD.Print("Switching to Swimming");
		}
	}

	private void OnBodyExited(Node3D body)
	{
		GD.Print($"Detected: {body.Name}; expected player: {player?.Name}");
		if(body == player)
		{
			var mainPlayerScript = GD.Load(mainPlayerScriptPath);
			body.SetScript(mainPlayerScript);
			GD.Print("Switching to Regular Movement");
		}
	}
}
