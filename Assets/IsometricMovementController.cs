using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricMovementController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float dashDistance = 1f;
    public float dashTime = 0.2f;

    Vector2 moveInput;
    float dash = 0;
    Rigidbody2D rigid;
    IsometricCharacterAnimationController animator;

    bool isDashing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<IsometricCharacterAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if(!isDashing && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(StartDash());
        }
        else if(!isDashing)
        {
            Vector2 adjustedMove = moveInput;
            adjustedMove.y *= 0.5f;
            rigid.MovePosition(rigid.position + adjustedMove.normalized * (moveSpeed * Time.deltaTime));
        }

        animator.Animate(moveInput);
    }

    IEnumerator StartDash()
    {
        isDashing = true;
        dash = 0;
        float t = 0;
        Vector2 preDashPosition = rigid.position;
        Vector2 dashDirection = moveInput.normalized;
        do
        {
            rigid.MovePosition(preDashPosition + dashDirection * Mathf.Lerp(0, dashDistance, t));
            t += Time.deltaTime / dashTime;
            yield return null;
        } while (t < 1.0f);
        dash = 0;
        isDashing = false;
    }
}
