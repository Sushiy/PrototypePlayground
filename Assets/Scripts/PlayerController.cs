using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 1;
    public float jumpForce = 10;
    Vector2 moveInput = Vector2.zero;

    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        //moveInput.y = Input.GetAxisRaw("Vertical");

        rigid.velocity += moveInput * playerSpeed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
