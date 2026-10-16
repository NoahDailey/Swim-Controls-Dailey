using System.Collections.Generic;
using Godot;

public partial class WaterArea3D : Area3D
{
	[Export]
	public CharacterBody3D player;
	
	[Export]
	public float swimSpeed;
	[Export]
	public float verticalSwimSpeed;
	[Export]
	public float waterDrag;
	
	private bool isSwimming;
	
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if(!isSwimming || player == null)
			return;
		
		Vector2 input = Input.GetVector("left", "right", "up", "down");
		Vector3 direction =  player.GlobalBasis.X * input.X + -player.GlobalBasis.Z * input.Y;
		direction = direction.Normalized();
		
		float verticalInput = 0.0f;
		if(Input.IsActionPressed("swim_up"))
			verticalInput += 1.0f;
		if(Input.IsActionPressed("swim_down"))
			verticalInput -= 1.0f;
		
		Vector3 targetVelocity = direction * swimSpeed + Vector3.Up * (verticalInput * verticalSwimSpeed);
		player.Velocity = player.Velocity.MoveToward(targetVelocity, waterDrag * (float)delta);
		
		player.MoveAndSlide();
	}
	
	private void OnBodyEntered(Node3D body)
	{
		if(player == null || body != player)
			return;
		
		isSwimming = true;
		player.SetPhysicsProcess(false);
	}
	
	private void OnBodyExited(Node3D body)
	{
		if(player == null || body != player)
			return;
		
		isSwimming = false;
		player.SetPhysicsProcess(true);
	}
}
