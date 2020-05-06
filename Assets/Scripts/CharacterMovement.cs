using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
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

		velocityFactor = MovementSettings.walkVelocityFactor;
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
		acceleration.x = strafeAxis * MovementSettings.maxAcceleration;

		acceleration.z = forwardAxis * MovementSettings.maxAcceleration;

		if (isJumping)
		{
			isJumping = false;
			acceleration.y = MovementSettings.jumpAcceleration;
		}

		else acceleration.y = (controller.isGrounded) ? 0 : -MovementSettings.gravityAcceleration;
	}

	private void UpdateVelocity()
	{
		velocity += acceleration * Time.fixedDeltaTime;

		velocity.x = acceleration.x == 0f ? velocity.x = 0f : Mathf.Clamp(
			velocity.x, -MovementSettings.maxStrafeVelocity * velocityFactor,
			MovementSettings.maxStrafeVelocity * velocityFactor);

		velocity.y = acceleration.y == 0f ? velocity.y = -0.1f : Mathf.Clamp(
			velocity.y, -MovementSettings.maxFallVelocity, MovementSettings.maxJumpVelocity);

		velocity.z = acceleration.z == 0f ? velocity.z = 0f : Mathf.Clamp(
			velocity.z, -MovementSettings.maxForwardVelocity * velocityFactor,
			MovementSettings.maxForwardVelocity * velocityFactor);
	}

	private void UpdatePosition()
	{
		Vector3 move = velocity * Time.fixedDeltaTime;

		controller.Move(transform.TransformVector(move));
	}

	#endregion
}