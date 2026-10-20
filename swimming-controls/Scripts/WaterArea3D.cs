using Godot;
using System;

namespace PolarBears.PlayerControllerAddon;

// Script that keeps track of which movement scheme to use
public partial class WaterArea3D : Area3D
{
	[Export] public CharacterBody3D player;
	[Export] public Node3D swimmingController;
	public PlayerController playerScript;
	public SwimmingController swimmingScript;

	// Things to do upon startup of the game
	public override void _Ready()
	{
		// Get the swimmingController
		swimmingController = (Node3D)player.GetChild(12);
		
		// Get access to the two scripts
		playerScript = GetNode<PlayerController>("../../Player");
		swimmingScript = GetNode<SwimmingController>("../../Player/SwimmingController");
		
		// Set the specific physics process we want to follow
		playerScript.SetPhysicsProcess(true);
		swimmingScript.SetPhysicsProcess(false);
		
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node3D body)
	{
		if(body == player) 
		{
			playerScript.SetPhysicsProcess(false);
			swimmingScript.SetPhysicsProcess(true);
		}
	}

	private void OnBodyExited(Node3D body)
	{
		if(body == player)
		{
			playerScript.SetPhysicsProcess(true);
			swimmingScript.SetPhysicsProcess(false);
		} 
	}
}
