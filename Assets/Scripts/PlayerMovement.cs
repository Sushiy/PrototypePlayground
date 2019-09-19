using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, InputListener
{
    Rigidbody2D rigid;

    [Header("Moving")]
    public float playerSpeed = 10f;
    public float playerAcceleration = 2000f;
    public float dragFactor = 10f;
    Vector3 velocity;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;

    [Header("Jumping")]
    public float jumpForce = 10;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GetComponent<PlayerController>().listeners.Add(this);
    }

    public void OnMoveInput(Vector2 moveInput)
    {
        MovePhysical(moveInput);
    }

    void Move(Vector2 moveInput)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(moveInput.x * playerSpeed, rigid.velocity.y);
        // And then smoothing it out and applying it to the character
        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    void MovePhysical(Vector2 moveInput)
    {
        float acceleration = playerAcceleration ;
        Vector2 velocityX = new Vector2(rigid.velocity.x, 0); 
        rigid.AddForce(moveInput * acceleration - velocityX * dragFactor, ForceMode2D.Force);
    }

    public void OnJumpInput()
    {
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void OnFireInput(bool inputDown)
    {
        //Do nothing here
    }

    public void OnRetractInput(bool inputDown)
    {
        //Do nothing here
    }

    public void OnCursorMove(Vector2 cursorPosition)
    {
        //Do nothing here
    }
}
