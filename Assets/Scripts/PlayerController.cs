using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rocketFistRecoil = 4;
    Vector2 moveInput = Vector2.zero;

    Rigidbody2D rigid;
    Vector3 mouseDirection;

    public Transform aimArm;

    public RocketController rocketFist;

    public List<InputListener> listeners;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        listeners = new List<InputListener>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rocketFist.OnBreak += ReactivateAimArm;
        rocketFist.OnGrab += Grab;
    }

    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        for(int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnMoveInput(moveInput);
        }

        if(Input.GetButtonDown("Jump"))
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnJumpInput();
            }
        }

        if(Input.GetButton("Fire1"))
        {
            bool inputDown = Input.GetButtonDown("Fire1");
            if (inputDown)
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    listeners[i].OnFireInput(true);
                }
            }
            else
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    listeners[i].OnFireInput(inputDown);
                }

            }
        }

        if (Input.GetButton("Fire2"))
        {
            bool inputDown = Input.GetButtonDown("Fire2");
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnRetractInput(inputDown);
            }
        }


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
            if(rocketFist.flightTime <= 0 || !rocketFist.gameObject.activeInHierarchy)
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

    private void Grab()
    {
        GetComponent<HingeJoint2D>().connectedBody = rocketFist.GetComponent<Rigidbody2D>();
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
