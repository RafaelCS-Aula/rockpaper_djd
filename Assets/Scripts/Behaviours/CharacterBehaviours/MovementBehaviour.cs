using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerSoundHandler))]
public class MovementBehaviour : MonoBehaviour, IDataUser<MovementData>, ISoundPlayer<PlayerSoundHandler>
{
    #region Data Handling

    [SerializeField] private MovementData _dataFile;
    public MovementData DataHolder
    {
        get => _dataFile;
        set => value = _dataFile;
    }

    public float maxAcceleration;
    public float jumpAcceleration;
    public float gravityAcceleration;

    public float maxForwardVelocity;
    public float maxStrafeVelocity;
    public float maxJumpVelocity;
    public float maxFallVelocity;

    public float walkVelocityFactor;
    public float diagonalVelocityFactor;
    public float fallingVelocityFactor;

    public float mass;
    public float damping;

    public int maxDoubleJumpCharges;
    public int maxDashCharges;
    public float doubleJumpCooldown;
    public float dashCooldown;
    public float dashForce;
    public float dashDuration;

    public void GetData()
    {
        maxAcceleration = DataHolder.maxAcceleration;
        jumpAcceleration = DataHolder.jumpAcceleration;
        gravityAcceleration = DataHolder.gravityAcceleration;

        maxForwardVelocity = DataHolder.maxForwardVelocity;
        maxStrafeVelocity = DataHolder.maxStrafeVelocity;
        maxJumpVelocity = DataHolder.maxJumpVelocity;
        maxFallVelocity = DataHolder.maxFallVelocity;

        walkVelocityFactor = DataHolder.walkVelocityFactor;
        diagonalVelocityFactor = DataHolder.diagonalVelocityFactor;
        fallingVelocityFactor = DataHolder.fallingVelocityFactor;

        mass = DataHolder.mass;
        damping = DataHolder.damping;

        maxDoubleJumpCharges = DataHolder.maxDoubleJumpCharges;
        maxDashCharges = DataHolder.maxDashCharges;
        doubleJumpCooldown = DataHolder.doubleJumpCooldown;
        dashCooldown = DataHolder.dashCooldown;
        dashForce = DataHolder.dashForce;
        dashDuration = DataHolder.dashDuration;
    }

    #endregion

    #region Class SetUp	

    [HideInInspector] public CharacterController controller;

    // Implement ISOundPlayer
    [HideInInspector] public PlayerSoundHandler audioHandler { get; set; }

    private Vector3 acceleration;
    private Vector3 velocity;


    [HideInInspector] public float strafeAxis;
    [HideInInspector] public float forwardAxis;
     
    [HideInInspector] public float velocityFactor;
    [HideInInspector] public Vector3 currentImpact;

    [HideInInspector] public bool jump;
    [HideInInspector] public bool canDoubleJump;

    [HideInInspector] public int doubleJumpCharges = 2;
    [HideInInspector] public int dashCharges = 2;

    [HideInInspector] public float doubleJumpTimer;
    [HideInInspector] public float dashTimer;

    [HideInInspector] public bool isDoubleJumpCharging;
    [HideInInspector] public bool isDashCharging;


    private void Awake()
    {
        if (DataHolder == null)
        {
            Debug.LogError("Object doesn't have a Data Scriptable Object assigned.");
            throw new UnityException();
        }

        else GetData();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        audioHandler = GetComponent<PlayerSoundHandler>();

        acceleration = Vector3.zero;
        velocity = Vector3.zero;

        strafeAxis = 0;
        forwardAxis = 0;

        velocityFactor = walkVelocityFactor;
        jump = false;

        doubleJumpTimer = 0.0f;
        dashTimer = 0.0f;
        isDoubleJumpCharging = false;
        isDashCharging = false;
    }

    #endregion

    #region FixedUpdate Methods	

    public void UpdateAcceleration()
    {
        acceleration.x = strafeAxis * maxAcceleration;

        acceleration.z = forwardAxis * maxAcceleration;

        if (jump)
        {
            jump = false;
            acceleration.y = jumpAcceleration;
        }

        else acceleration.y = (controller.isGrounded) ? 0 :
                -gravityAcceleration;
    }

    public void UpdateVelocityFactor()
    {
        if (strafeAxis != 0 && forwardAxis != 0)
        {
            if (!controller.isGrounded)
                velocityFactor = diagonalVelocityFactor * fallingVelocityFactor;
            else
                velocityFactor = diagonalVelocityFactor;
        }
        else if (!controller.isGrounded)
            velocityFactor = fallingVelocityFactor;

        else velocityFactor = walkVelocityFactor;
    }

    public void UpdateVelocity()
    {
        velocity += acceleration * Time.fixedDeltaTime;

        velocity.x = acceleration.x == 0f ? velocity.x = 0f : Mathf.Clamp(
            velocity.x, -maxStrafeVelocity  * velocityFactor,
            maxStrafeVelocity * velocityFactor);

        velocity.y = acceleration.y == 0f ? velocity.y = -0.1f : Mathf.Clamp(
            velocity.y, -maxFallVelocity,
            maxJumpVelocity);

        velocity.z = acceleration.z == 0f ? velocity.z = 0f : Mathf.Clamp(
            velocity.z, -maxForwardVelocity * velocityFactor,
            maxForwardVelocity * velocityFactor);

        if (currentImpact.magnitude > 0.2f) velocity += currentImpact;
    }

    public void UpdatePosition()
    {
        Vector3 move = velocity * Time.fixedDeltaTime;

        controller.Move(transform.TransformVector(move));

        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero,
            damping * Time.deltaTime);
    }

    #endregion

    #region AMR Methods

    public void UpdateAMRCharges()
    {
        if (doubleJumpCharges < maxDoubleJumpCharges && !isDoubleJumpCharging)
        {
            isDoubleJumpCharging = true;
            doubleJumpTimer = doubleJumpCooldown;
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

        if (dashCharges < maxDashCharges && !isDashCharging)
        {
            isDashCharging = true;
            dashTimer = dashCooldown;
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

        if (controller.isGrounded)
        {
            jump = true;

            // Play Jump Audio
            audioHandler.PlayAudio(audioHandler.dJump, 3);
        }

        else if (canDoubleJump && doubleJumpCharges > 0)
        {
            jump = true;
            canDoubleJump = false;

            // Play double jump sound
            audioHandler.PlayAudio(audioHandler.dDoubleJump, 3);

            doubleJumpCharges--;
        }
    }

    public void Dash()
    {
        if (dashCharges > 0 && (velocity.x != 0 || velocity.z != 0))
        {

            // Play dash audio
            audioHandler.PlayAudio(audioHandler.dDash, 3);

            StartCoroutine(DashCoroutine());
            dashCharges--;
        }
    }
    private IEnumerator DashCoroutine()
    {
        Vector3 dashVelocity = velocity;
        dashVelocity.y = 0;

        AddForce(dashVelocity, dashForce);

        yield return new WaitForSeconds(dashDuration);

        currentImpact = Vector3.zero; // Resets impact
    }

    private void AddForce(Vector3 direction, float force)
    {
        direction.Normalize();
        currentImpact += direction.normalized * force / mass;
    }

    #endregion
}