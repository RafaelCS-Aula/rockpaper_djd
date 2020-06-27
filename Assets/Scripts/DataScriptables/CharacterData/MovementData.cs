using UnityEngine;

namespace RPS_DJDIII.Assets.Scripts.DataScriptables.CharacterData
{
	/// <summary>
    /// Stores all static info about the movement of whoever this is applied to
    /// </summary>
	[CreateAssetMenu(fileName = "MovementData",
	menuName = "Data/Settings/Movement Data", order = 1)]
	public class MovementData : ScriptableObject
	{
		[Header("-Acceleration Options-")]
		[Tooltip("Acceleration applied in the direction of the movement")]
		public float maxAcceleration;

		[Tooltip("Upwards acceleration applied to a jump")]
		public float jumpAcceleration;

		[Tooltip("Downwards acceleration applied to character when falling")]
		public float gravityAcceleration;

		[Header("-Velocity Options-")]
		[Tooltip("Maximum velocity of character's movement, forward or backward")]
		public float maxForwardVelocity;

		[Tooltip("Maximum velocity of character's movement, when strafing")]
		public float maxStrafeVelocity;

		[Tooltip("Maximum velocity of character's jump")]
		public float maxJumpVelocity;

		[Tooltip("Maximum velocity of a character's fall")]
		public float maxFallVelocity;

		[Header("-Factor Options-")]
		[Tooltip("Velocity factor multiplier for when the player is walking")]
		public float walkVelocityFactor;

		[Tooltip("Velocity factor multiplier for when the player is walking")]
		public float backwardsVelocityFactor;

		[Tooltip("Velocity factor multiplier for when" +
			"the character is walking diagonally")]
		public float diagonalVelocityFactor;

		[Tooltip("Velocity factor multiplier for forward, backward and strafing" +
			"movements when the character is falling")]
		public float fallingVelocityFactor;

		[Header("-Physics Options-")]
		[Tooltip("Mass of the character to calculate and" +
			"add forces to it's movement")]
		public float mass;

		[Tooltip("How far the player is goimg to move until the dash force is" +
			"lerped to zero after dash is complete")]
		public float damping;

		[Header("-AMR Options-")]
		[Tooltip("Maximum number of Double Jump charges" +
			"the character can have at a time")]
		public int maxDoubleJumpCharges;

		[Tooltip("Maximum number of Dash charges" +
			"the character can have at a time")]
		public int maxDashCharges;

		[Tooltip("Time until a new Double Jump charge can be used")]
		public float doubleJumpCooldown;

		[Tooltip("Time until a new Dash charge can be used")]
		public float dashCooldown;

		[Tooltip("Force applied to the dash movement")]
		public float dashForce;

		[Tooltip("How long the Dash movement lasts until it stops")]
		public float dashDuration;
	}
}