using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectController : MonoBehaviour
{
    Rigidbody rigid;

    public float acceleration = 10f;
    public float turning = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        rigid.AddForce(transform.forward * v * acceleration * Time.deltaTime, ForceMode.Acceleration);
        rigid.AddForce(transform.right * h * acceleration * Time.deltaTime, ForceMode.Acceleration);
        //rigid.MovePosition(rigid.position + transform.forward * v * acceleration * Time.deltaTime);
    }
}
