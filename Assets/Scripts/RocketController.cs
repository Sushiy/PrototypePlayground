using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float RocketAcceleration = 10f;
    public float maxVelocity = 4f;
    Rigidbody2D rigid;

    Vector3 mouseDirection;

    public float flightTime = 0; //How many seconds can the glove fly
    float maxFlightTime = 3;

    public bool keyboardController = false;
    Vector2 moveInput = Vector2.zero;

    public LayerMask breakMask;
    public bool broken;

    SpriteRenderer spriteR;

    public System.Action OnBreak;

    private float jointIntervals = 0.064f;
    private float jointDelta = 0.0f;

    private List<Vector3> wayPoints;

    public LineRenderer chainRender;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        wayPoints = new List<Vector3>();
        chainRender.positionCount = Mathf.RoundToInt(maxFlightTime / jointIntervals);
    }

    public void Fire()
    {
        flightTime = maxFlightTime;
        broken = false;
        wayPoints.Clear();
        ResetChainPosition();
    }

    void ResetChainPosition()
    {
        for(int i = 0; i < chainRender.positionCount;  i++)
        {
            chainRender.SetPosition(i, transform.position);
        }
    }

    private void LateUpdate()
    {
        spriteR.color = Color.Lerp(Color.green, Color.red, 1.0f - (flightTime / maxFlightTime));
        if (flightTime <= 0) spriteR.color = Color.black;
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
            if(flightTime > 0 && Input.GetMouseButtonDown(1))
            {
                Break();
            }
            if (leftMouse)
            {
                if(flightTime > 0)
                {
                    rigid.gravityScale = 0;
                    rigid.AddForce(mouseDirection * RocketAcceleration, ForceMode2D.Force);
                    flightTime -= Time.deltaTime;
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
                    jointDelta += Time.fixedDeltaTime;

                }
                else
                {
                    Break();
                }
            }
            else
            {
                rigid.gravityScale = 1;
            }

        }

        if(jointDelta > jointIntervals)
        {
            jointDelta = 0;
            wayPoints.Insert(0,transform.position);
            chainRender.SetPositions(wayPoints.ToArray());
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if( breakMask == (breakMask | (1 << col.gameObject.layer)))
        {
            Break();
        }
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < wayPoints.Count-1; i++)
        {
            Gizmos.DrawLine(wayPoints[i], wayPoints[i + 1]);
        }
    }

    void Break()
    {
        if (broken) return;
        broken = true;
        flightTime = 0;
        rigid.gravityScale = 1;
        OnBreak?.Invoke();
    }
}
