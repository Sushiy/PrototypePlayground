using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float RocketAcceleration = 10f;
    public float maxVelocity = 4f;
    Rigidbody2D rigid;

    Vector3 mouseDirection;

    public bool keyboardController = false;
    Vector2 moveInput = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(keyboardController)
        {
            rigid.gravityScale = 0;
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            rigid.MovePosition(rigid.position + moveInput.normalized * RocketAcceleration * Time.fixedDeltaTime);
            if (rigid.velocity.sqrMagnitude > 0.01f)
            {
                // Get Angle in Radians
                float AngleRad = Mathf.Atan2(moveInput.y, moveInput.x);
                // Get Angle in Degrees
                float AngleDeg = (180 / Mathf.PI) * AngleRad;
                // Rotate Object
                transform.rotation = Quaternion.Euler(0,0,AngleDeg);
            }
        }
        else
        {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseDirection = worldMousePos - transform.position;
            bool leftMouse = Input.GetMouseButton(0);
            if (leftMouse)
            {
                rigid.gravityScale = 0;
                rigid.AddForce(mouseDirection * RocketAcceleration, ForceMode2D.Force);
                rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxVelocity);
                if (rigid.velocity.sqrMagnitude > 0.01f)
                {
                    // Get Angle in Radians
                    float AngleRad = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x);
                    // Get Angle in Degrees
                    float AngleDeg = (180 / Mathf.PI) * AngleRad - 90;
                    // Rotate Object
                    rigid.MoveRotation(AngleDeg);
                }
            }
            else
            {
                rigid.gravityScale = 1;
            }

        }
    }
}
