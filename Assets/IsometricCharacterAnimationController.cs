using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCharacterAnimationController : MonoBehaviour
{

    Vector3 lastFacing = new Vector3(1, 1, 0);
    bool isMoving = false;
    float movingThreshhold = 0.01f;
    Animator animator;

    int facingXHash = Animator.StringToHash("FacingX");
    int facingYHash = Animator.StringToHash("FacingY");
    int isMovingHash = Animator.StringToHash("IsMoving");
    int pickupHash = Animator.StringToHash("Pickup");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Animate(Vector2 moveInput)
    {
        isMoving = moveInput.sqrMagnitude > movingThreshhold;

        if (isMoving)
        {
            lastFacing = moveInput;
        }

        animator.SetFloat(facingXHash, lastFacing.x);
        animator.SetFloat(facingYHash, lastFacing.y);

        if (Input.GetButtonDown("Jump"))
        {
            //animator.SetTrigger(pickupHash);
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
        animator.SetBool(isMovingHash, isMoving);
    }
}
