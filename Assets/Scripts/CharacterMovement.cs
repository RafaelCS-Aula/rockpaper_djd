using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	#region Serializable Classes

	[Serializable]
	public class MovementSettings
	{
		[Header("-Acceleration Options-")]
		public float maxAcceleration = 80.0f;
		public float jumpAcceleration = 500.0f;
		public float gravityAcceleration = 20.0f;

		[Header("-Velocity Options-")]
		public float maxForwardVelocity = 5.0f;
		public float maxStrafeVelocity = 4.0f;
		public float maxJumpVelocity = 50.0f;
		public float maxFallVelocity = 50.0f;

		[Header("-Factor Options-")]
		public float walkVelocityFactor = 1.0f;

		public float diagonalVelocityFactor = 0.75f;
		public float fallingVelocityFactor = 0.7f;
	}

	[SerializeField]
	public MovementSettings movementSettings;

    #endregion

    #region Class SetUp	

    public CharacterController controller;

	private Vector3 acceleration;
	private Vector3 velocity;

	[NonSerialized] public float strafeAxis;
	[NonSerialized] public float forwardAxis;

	[NonSerialized] public float velocityFactor;
	[NonSerialized] public bool isJumping;

	private void Start()
	{
		controller = GetComponent<CharacterController>();

		acceleration = Vector3.zero;
		velocity = Vector3.zero;

		strafeAxis = 0;
		forwardAxis = 0;

		velocityFactor = movementSettings.walkVelocityFactor;
		isJumping = false;
	}

	#endregion

	#region FixedUpdate Methods	

	private void FixedUpdate()
	{
		UpdateAcceleration();
		UpdateVelocity();
		UpdatePosition();
	}

	private void UpdateAcceleration()
	{
		acceleration.x = strafeAxis * movementSettings.maxAcceleration;

		acceleration.z = forwardAxis * movementSettings.maxAcceleration;

		if (isJumping)
		{
			isJumping = false;
			acceleration.y = movementSettings.jumpAcceleration;
		}

		else acceleration.y = (controller.isGrounded) ? 0 : -movementSettings.gravityAcceleration;
	}

	private void UpdateVelocity()
	{
		velocity += acceleration * Time.fixedDeltaTime;

		velocity.x = acceleration.x == 0f ? velocity.x = 0f : Mathf.Clamp(
			velocity.x, -movementSettings.maxStrafeVelocity * velocityFactor,
			movementSettings.maxStrafeVelocity * velocityFactor);

		velocity.y = acceleration.y == 0f ? velocity.y = -0.1f : Mathf.Clamp(
			velocity.y, -movementSettings.maxFallVelocity, movementSettings.maxJumpVelocity);

		velocity.z = acceleration.z == 0f ? velocity.z = 0f : Mathf.Clamp(
			velocity.z, -movementSettings.maxForwardVelocity * velocityFactor,
			movementSettings.maxForwardVelocity * velocityFactor);
	}

	private void UpdatePosition()
	{
		Vector3 move = velocity * Time.fixedDeltaTime;

		controller.Move(transform.TransformVector(move));
	}

	#endregion
}