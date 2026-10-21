using System.Collections.Generic;
using Godot;

namespace PolarBears.PlayerControllerAddon;

public partial class SwimmingController : Node3D
{
	// Player related nodes
	[Export] public CharacterBody3D player;
	public Node3D playerHead;
	
	// Movement related fields
	[ExportGroup("Movement Values")]
	[Export(PropertyHint.Range, "0,20,0.1,or_greater")]
	public float swimSpeed             { get; set; } = 2.0f;
	[Export(PropertyHint.Range, "0,20,0.1,or_greater")]
	public float verticalSwimSpeed             { get; set; } = 3.0f;
	[Export(PropertyHint.Range, "0,20,0.1,or_greater")]
	public float waterGravity          { get; set; } = 0.25f;
	
	// Input related fields inspired from original controller
	[ExportGroup("Input")]
	[Export] public string moveForward;
	[Export] public string moveBackward;
	[Export] public string strafeLeft;
	[Export] public string strafeRight;
	[Export] public string swimUp;
	[Export] public string swimDown;
	
	//public HealthSystem    HealthSystem;
	//public Gravity         Gravity;
	
	// gravity required for proper differences in world
	private float gravity;
	
	private float oxygenAmount;
	
	public override void _Ready()
	{
		// Get access to the player head for movement and set the gravity
		playerHead = (Node3D)player.GetChild(2);
		
		//// Initialize the Gravity and HealthSystems
		//Gravity = GetNode<Gravity>("../Gravity");
		//Gravity.Init(gravity);
		//
		//// Needed Rectangles for the HealthSystem
		//ColorRect vignetteRect = GetNode<ColorRect>(
			//"../Head/CameraSmooth/Camera3D/CLVignette(Layer_1)/HealthVignetteRect");
		//ColorRect distortionRect = GetNode<ColorRect>(
			//"../Head/CameraSmooth/Camera3D/CLDistortion(Layer_2)/HealthDistortionRect");
		//ColorRect blurRect = GetNode<ColorRect>("../Head/CameraSmooth/Camera3D/CLBlur(Layer_2)/BlurRect");
		//
		//HealthSystem = GetNode<HealthSystem>("../HealthSystem");
		//HealthSystem.HealthSystemInitParams healthSystemParams = new HealthSystem.HealthSystemInitParams()
		//{
			//Gravity = Gravity,
			//Parent = player,
			//Camera = (Camera3D)GetNode("../Head/CameraSmooth/Camera3D"),
			//AnimationPlayer = (AnimationPlayer)GetNode("../AnimationPlayer"),
			//Head =  playerHead,
			//VignetteRect = vignetteRect,
			//DistortionRect = distortionRect,
			//BlurRect = blurRect,
		//};
		//HealthSystem.Init(healthSystemParams);
		
		gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
		//oxygenAmount = 100.0f;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// Determine the forward and sideways movement keys and values for movement
		float forward = Strength(moveBackward, Key.S) - Strength(moveForward, Key.W);
		float sideways = Strength(strafeRight, Key.D) - Strength(strafeLeft, Key.A);
		
		// Determine the movement direction
		Vector3 input = new Vector3(sideways, 0, forward).LimitLength();
		Vector3 direction = (playerHead.GlobalTransform.Basis * input).Normalized();
		
		// Apply the velocity to the player
		Vector3 velocity = player.Velocity;
		velocity.X = direction.X * swimSpeed;
		velocity.Z = direction.Z * swimSpeed;
		
		// Determmine vertical movement
		float verticalInput = Strength(swimUp, Key.Space) - Strength(swimDown, Key.Ctrl);
		
		// Apply vertical movement
		velocity.Y -= gravity * waterGravity * (float)delta;
		velocity.Y = Mathf.Clamp(velocity.Y + verticalInput * verticalSwimSpeed,
		-verticalSwimSpeed, verticalSwimSpeed);
		
		// Move the player
		player.Velocity = velocity;
		player.MoveAndSlide();
		
	}
	
	// Method borrowed from the playerController script
	// Checks to see what button was pressed and calculates how to apply the movement
	 private float Strength(string action, Key fallback)
	{
		if (!string.IsNullOrEmpty(action) && InputMap.HasAction(action))
			return Input.GetActionStrength(action);
		
		return Input.IsPhysicalKeyPressed(fallback) ? 1.0f : 0.0f;
	}
}
