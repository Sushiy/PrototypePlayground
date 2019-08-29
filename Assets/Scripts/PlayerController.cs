using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 1;
    public float jumpForce = 10;

    public float rocketFistRecoil = 4;
    Vector2 moveInput = Vector2.zero;

    Rigidbody2D rigid;
    Vector3 mouseDirection;

    public Transform aimArm;

    public RocketController rocketFist;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rocketFist.OnBreak += ReactivateAimArm;
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

    private void Update()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDirection = worldMousePos - transform.position;
        //Aim at mouse
        if (mouseDirection.sqrMagnitude > 0.01f)
        {
            // Get Angle in Radians
            float AngleRad = Mathf.Atan2(mouseDirection.y, mouseDirection.x);
            // Get Angle in Degrees
            float AngleDeg = (180 / Mathf.PI) * AngleRad - 90;
            // Rotate Object
            aimArm.rotation = Quaternion.Euler(0, 0, AngleDeg);
        }

        if(Input.GetMouseButtonDown(0) && aimArm.gameObject.activeInHierarchy)
        {
            if(rocketFist.flightTime <= 0 ||!rocketFist.gameObject.activeInHierarchy)
            {
                aimArm.gameObject.SetActive(false);
                rocketFist.gameObject.SetActive(false);
                rocketFist.transform.rotation = aimArm.transform.rotation;
                rocketFist.transform.position = aimArm.transform.position + aimArm.transform.up * 0.5f;
                rocketFist.gameObject.SetActive(true);
                rocketFist.Fire();
                rigid.AddForce(-rocketFist.transform.up * rocketFistRecoil, ForceMode2D.Impulse);
            }
        }
    }

    private void ReactivateAimArm()
    {
        StartCoroutine(DelayedActivate());
    }

    IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(2.0f);
        ActivateAimArm();
    }

    private void ActivateAimArm()
    {
        aimArm.gameObject.SetActive(true);
        rocketFist.gameObject.SetActive(false);
    }
}
