using UnityEngine;

[CreateAssetMenu(fileName = "MovementData",
    menuName = "Data/Settings/Movement Data", order = 1)]
public class MovementData : ScriptableObject
{
	[Header("-Acceleration Options-")]
	[Tooltip("Acceleration applied in the direction of the movement")]
	public float maxAcceleration = 80.0f;

	[Tooltip("Upwards acceleration applied to a jump")]
	public float jumpAcceleration = 500.0f;
	
	[Tooltip("Downwards acceleration applied to character when falling")]
	public float gravityAcceleration = 20.0f;

	[Header("-Velocity Options-")]
	[Tooltip("Maximum velocity of character's movement, forward or backward")]
	public float maxForwardVelocity = 5.0f;
	
	[Tooltip("Maximum velocity of character's movement, when strafing")]
	public float maxStrafeVelocity = 4.0f;
	
	[Tooltip("Maximum velocity of character's jump")]
	public float maxJumpVelocity = 50.0f;
	
	[Tooltip("Maximum velocity of a character's fall")]
	public float maxFallVelocity = 50.0f;

	[Header("-Factor Options-")]
	
	[Tooltip("Velocity factor multiplier for when the player is walking")]
	public float walkVelocityFactor = 1.0f;

	[Tooltip("Velocity factor multiplier for when" +
		"the character is walking diagonally")]
	public float diagonalVelocityFactor = 0.75f;
	
	[Tooltip("Velocity factor multiplier for forward, backward and strafing" +
		"movements when the character is falling")]
	public float fallingVelocityFactor = 0.7f;

	[Header("-Physics Options-")]
	[Tooltip("Mass of the character to calculate and" +
		"add forces to it's movement")]
	public float mass = 1f;
	
	[Tooltip("How far the player is goimg to move until the dash force is" +
		"lerped to zero after dash is complete")]
	public float damping = 5f;

	[Header("-AMR Options-")]
	[Tooltip("Maximum number of Double Jump charges" +
		"the character can have at a time")]
	public int maxDoubleJumpCharges = 2;
	
	[Tooltip("Maximum number of Dash charges" +
		"the character can have at a time")]
	public int maxDashCharges = 2;

	[Tooltip("Time until a new Double Jump charge can be used")]
	public float doubleJumpCooldown = 5f;
	
	[Tooltip("Time until a new Dash charge can be used")]
	public float dashCooldown = 5f;

	[Tooltip("Force applied to the dash movement")]
	public float dashForce = 50f;
	
	[Tooltip("How long the Dash movement lasts until it stops")]
	public float dashDuration = 0.5f;
}
