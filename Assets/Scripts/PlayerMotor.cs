using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 3.0f;
    // Movement
    private CharacterController controller;
    private float jumpForce =  4.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private float speed = 7.0f;
    public int desiredLane = 1;  // 0 -> Left, 1 -> Middle, 2 -> Right

    // Animation
    private Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Gather inputs on which lane we should be
        if (Input.GetKeyDown(KeyCode.LeftArrow)) 
            MoveLane(false);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveLane(true);
        
        // Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;

        // Calculate y
        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);

        if (isGrounded) 
        {
            verticalVelocity = -1.0f;
            if (Input.GetKeyDown(KeyCode.Space)) {
                // Jump
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
        } else 
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            // Fast falling mechanics
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = -jumpForce;
            }
        }

        // Calculate move vector
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;
        moveVector.y = verticalVelocity ;
        moveVector.z = speed;


        // Move Player
        controller.Move(moveVector * Time.deltaTime);

        // Rotate pengu when they try to change lanes
        Vector3 dir = controller.velocity;
        if (dir != Vector3.zero) 
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, 0.05f);
        }
    }

    private void MoveLane(bool goingRight) {
        desiredLane += goingRight ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(
                controller.bounds.center.x,
                controller.bounds.center.y - controller.bounds.extents.y + 0.2f,
                controller.bounds.center.z
            ),
            Vector3.down
        );

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
    }
}
