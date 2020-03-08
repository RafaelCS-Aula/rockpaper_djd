using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float MAX_ACCELERATION = 80.0f;

    private const float JUMP_ACCELERATION    = 500.0f;
    private const float GRAVITY_ACCELERATION = 20.0f;

    private const float MAX_FORWARD_VELOCITY  = 5.0f;
    private const float MAX_BACKWARD_VELOCITY = 4.0f;
    private const float MAX_STRAFE_VELOCITY   = 4.0f;
    private const float MAX_JUMP_VELOCITY     = 50.0f;
    private const float MAX_FALL_VELOCITY     = 100.0f;

    private const float DIAGONAL_VELOCITY_FACTOR = 0.6f;
    private const float WALK_VELOCITY_FACTOR     = 1.0f;
    private const float SPRINT_VELOCITY_FACTOR   = 1.5f;

    private CharacterController controller;

    private Vector3 acceleration;
    private Vector3 velocity;
    private float velocityFactor;
    private bool jump;
    private bool autoRun = false; // Tirar se não se incorporar no jogo

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        velocityFactor = WALK_VELOCITY_FACTOR;
        jump = false;
    }

    private void Update()
    {
        UpdateVelocityFactor();
        Updatejump();
        UpdateAutoRun(); // Tirar se não se incorporar no jogo
        print(autoRun);
    }

    private void UpdateVelocityFactor()
    {
        velocityFactor = Input.GetButton("Sprint") ?
            SPRINT_VELOCITY_FACTOR : WALK_VELOCITY_FACTOR;

        if (Input.GetAxis("Strafe") != 0 && Input.GetAxis("Forward") != 0)
        {
            velocity *= DIAGONAL_VELOCITY_FACTOR;
            velocity.y /= DIAGONAL_VELOCITY_FACTOR;
        }
    }

    private void Updatejump()
    {
        if (controller.isGrounded && Input.GetButtonDown("Jump")) jump = true;
    }

    private void UpdateAutoRun()
    {
        if (Input.GetButtonDown("AutoRun")) autoRun = !autoRun;

        else if (autoRun && Input.GetAxis("Forward") != 0) autoRun = false;
    }

    private void FixedUpdate()
    {
        UpdateAcceleration();
        UpdateVelocity();
        UpdatePosition();
    }

    private void UpdateAcceleration()
    {
        acceleration.x = Input.GetAxis("Strafe") * MAX_ACCELERATION;

        acceleration.z = autoRun ? 1f : Input.GetAxis("Forward") * MAX_ACCELERATION;

        if (jump)
        {
            jump = false;
            acceleration.y = JUMP_ACCELERATION;
        }

        else acceleration.y = (controller.isGrounded) ? 0 : -GRAVITY_ACCELERATION;
    }

    private void UpdateVelocity()
    {
        velocity += acceleration * Time.fixedDeltaTime;

        velocity.x = acceleration.x == 0f ? velocity.x = 0f : Mathf.Clamp(
            velocity.x, -MAX_STRAFE_VELOCITY * velocityFactor,
            MAX_STRAFE_VELOCITY * velocityFactor);

        velocity.y = acceleration.y == 0f ? velocity.y = -0.1f : Mathf.Clamp(
            velocity.y, -MAX_FALL_VELOCITY, MAX_JUMP_VELOCITY);

        velocity.z = acceleration.z == 0f ? velocity.z = 0f : Mathf.Clamp(
            velocity.z, -MAX_BACKWARD_VELOCITY * velocityFactor,
            MAX_FORWARD_VELOCITY * velocityFactor);
    }

    private void UpdatePosition()
    {
        Vector3 move = velocity * Time.fixedDeltaTime;

        controller.Move(transform.TransformVector(move));
    }
}