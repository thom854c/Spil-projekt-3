using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private bool isFacingRight;
    private CharacterController2D controller;

    public float MaxSpeed = 8;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
    public void Start()
    {
        controller = GetComponent<CharacterController2D>();
        isFacingRight = transform.localScale.x > 0;

    }

    public void Update()
    {
        HandleInput();

        var movementFactor = controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
        controller.SetHorizontalForce(Mathf.Lerp(controller.Velocity.x, Input.GetAxis("Horizontal") * MaxSpeed, Time.deltaTime * movementFactor));
        controller.PointAndPush(Input.GetAxis("RS Horizontal"), Input.GetAxis("RS Vertical"), Input.GetButton("Push"));
    }

    private void HandleInput()
    {
        if (Input.GetAxisRaw("Horizontal") > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && isFacingRight)
        {
            Flip();
        }

        if (controller.CanJump && Input.GetButtonDown("Jump"))
        {
            controller.Jump();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isFacingRight = transform.localScale.x > 0;
    }


}
