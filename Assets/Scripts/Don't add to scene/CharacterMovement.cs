using System;
using System.Collections;
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

		[Header("-Physics Options-")]
		public float mass = 1f;
		public float damping = 5f;

		[Header("-AMR Options-")]
		public int maxDoubleJumpCharges = 2;
		public int maxDashCharges = 2;

		public float doubleJumpCooldown = 5f;
		public float dashCooldown = 5f;

		public float dashForce = 50f;
		public float dashDuration = 0.5f;
	}

	[SerializeField]
	public MovementSettings movementSettings;

    #endregion

    #region Class SetUp	

    [HideInInspector] public CharacterController controller;

	private Vector3 acceleration;
	private Vector3 velocity;

	[NonSerialized] public float strafeAxis;
	[NonSerialized] public float forwardAxis;

	[NonSerialized] public float velocityFactor;
	[NonSerialized] public Vector3 currentImpact;

	[NonSerialized] public bool jump;
	[NonSerialized] public bool canDoubleJump;

	[NonSerialized] public int doubleJumpCharges = 2;
	[NonSerialized] public int dashCharges = 2;

	[NonSerialized] public float doubleJumpTimer;
	[NonSerialized] public float dashTimer;
	
	[NonSerialized] public bool isDoubleJumpCharging;
	[NonSerialized] public bool isDashCharging;

	private void Start()
	{
		controller = GetComponent<CharacterController>();

		acceleration = Vector3.zero;
		velocity = Vector3.zero;

		strafeAxis = 0;
		forwardAxis = 0;

		velocityFactor = movementSettings.walkVelocityFactor;
		jump = false;

		doubleJumpTimer = 0.0f;
		dashTimer = 0.0f;
		isDoubleJumpCharging = false;
		isDashCharging = false;
	}

	#endregion

	#region FixedUpdate Methods	

	private void FixedUpdate()
	{
		UpdateAcceleration();
		UpdateVelocityFactor();
		UpdateVelocity();
		UpdatePosition();
	}

	private void UpdateAcceleration()
	{
		acceleration.x = strafeAxis * movementSettings.maxAcceleration;

		acceleration.z = forwardAxis * movementSettings.maxAcceleration;

		if (jump)
		{
			jump = false;
			acceleration.y = movementSettings.jumpAcceleration;
		}

		else acceleration.y = (controller.isGrounded) ? 0 :
				-movementSettings.gravityAcceleration;
	}

	private void UpdateVelocityFactor()
	{
		if (strafeAxis != 0 && forwardAxis != 0)
		{
			if (!controller.isGrounded)
				velocityFactor = movementSettings.diagonalVelocityFactor * movementSettings.fallingVelocityFactor;
			else
				velocityFactor = movementSettings.diagonalVelocityFactor;
		}
		else if (!controller.isGrounded)
			velocityFactor = movementSettings.fallingVelocityFactor;

		else velocityFactor = movementSettings.walkVelocityFactor;
	}

	private void UpdateVelocity()
	{
		velocity += acceleration * Time.fixedDeltaTime;

		velocity.x = acceleration.x == 0f ? velocity.x = 0f : Mathf.Clamp(
			velocity.x, -movementSettings.maxStrafeVelocity * velocityFactor,
			movementSettings.maxStrafeVelocity * velocityFactor);

		velocity.y = acceleration.y == 0f ? velocity.y = -0.1f : Mathf.Clamp(
			velocity.y, -movementSettings.maxFallVelocity,
			movementSettings.maxJumpVelocity);

		velocity.z = acceleration.z == 0f ? velocity.z = 0f : Mathf.Clamp(
			velocity.z, -movementSettings.maxForwardVelocity * velocityFactor,
			movementSettings.maxForwardVelocity * velocityFactor);

		if (currentImpact.magnitude > 0.2f) velocity += currentImpact;
	}

	private void UpdatePosition()
	{
		Vector3 move = velocity * Time.fixedDeltaTime;

		controller.Move(transform.TransformVector(move));

		currentImpact = Vector3.Lerp(currentImpact, Vector3.zero,
			movementSettings.damping * Time.deltaTime);
	}

	#endregion

	#region AMR Methods

	public void UpdateAMRCharges()
	{
		if (doubleJumpCharges < movementSettings.maxDoubleJumpCharges && !isDoubleJumpCharging)
		{
			isDoubleJumpCharging = true;
			doubleJumpTimer = movementSettings.doubleJumpCooldown;
		}

		if (isDoubleJumpCharging)
		{
			if (doubleJumpTimer > 0.0f) doubleJumpTimer -= Time.deltaTime;
			else
			{
				isDoubleJumpCharging = false;
				doubleJumpCharges += 1;
			}
		}

		if (dashCharges < movementSettings.maxDashCharges && !isDashCharging)
		{
			isDashCharging = true;
			dashTimer = movementSettings.dashCooldown;
		}

		if (isDashCharging)
		{
			if (dashTimer > 0.0f) dashTimer -= Time.deltaTime;
			else
			{
				isDashCharging = false;
				dashCharges += 1;
			}
		}
	}

	public void Jump()
	{
		if (controller.isGrounded) canDoubleJump = true;

		if (controller.isGrounded) jump = true;

		else if (canDoubleJump && doubleJumpCharges > 0)
		{
			jump = true;
			canDoubleJump = false;
			doubleJumpCharges--;
		}
	}

    public void Dash()
	{
		if (dashCharges > 0 && (velocity.x != 0 || velocity.z != 0))
		{
			StartCoroutine(DashCoroutine());
			dashCharges--;
		}
	}
	private IEnumerator DashCoroutine()
	{
		Vector3 dashVelocity = velocity;
		dashVelocity.y = 0;

		AddForce(dashVelocity, movementSettings.dashForce);

		yield return new WaitForSeconds(movementSettings.dashDuration);

		currentImpact = Vector3.zero; // Resets impact
	}

    private void AddForce(Vector3 direction, float force)
	{
		direction.Normalize();
		currentImpact += direction.normalized * force / movementSettings.mass;
	}

    #endregion
}