using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RocketController))]
public class ChainController : MonoBehaviour, InputListener
{
    Rigidbody2D rigid;
    SpriteRenderer spriteR;

    RocketController rocketController;

    private float jointIntervals = 0.064f;
    private float jointDelta = 0.0f;

    private List<Vector3> wayPoints;

    private LineRenderer chainRender;

    DistanceJoint2D distance;
    float chainLength = 2;
    public float maxChainLength = 15f;

    bool init = false;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    void Init()
    {
        print("init");
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        rocketController = GetComponent<RocketController>();
        GetComponentInParent<PlayerController>().listeners.Add(this);

        wayPoints = new List<Vector3>();
        distance = GetComponent<DistanceJoint2D>();

        chainRender = GetComponentInChildren<LineRenderer>();
        chainRender.positionCount = Mathf.RoundToInt(rocketController.maxFlightTime / jointIntervals);
        init = true;
    }

    private void OnEnable()
    {
        if(!init)
        {
            Init();
        }
    }

    public void OnFireInput(bool inputDown)
    {
        if(inputDown)
        {
            Debug.Log("OnFireChain true");
            wayPoints.Clear();
            ResetChainPosition();
            chainLength = 2;
            distance.distance = chainLength;
        }
        jointDelta += Time.fixedDeltaTime;
        if (chainLength < maxChainLength)
            chainLength += Time.fixedDeltaTime * rigid.velocity.magnitude;
        else
            chainLength = maxChainLength;

        if (chainRender != null && jointDelta > jointIntervals)
        {
            jointDelta = 0;
            wayPoints.Insert(0, transform.position);
            chainRender.SetPositions(wayPoints.ToArray());
        }

        if (distance != null)
        {
            distance.distance = chainLength;
            if (Vector2.Distance(distance.connectedBody.position, rigid.position) > (distance.distance - 0.1f))
            {
                rocketController.Break();
            }
        }
    }

    void ResetChainPosition()
    {
        Debug.Log("resetchain");
        for (int i = 0; i < chainRender.positionCount; i++)
        {
            chainRender.SetPosition(i, transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(wayPoints[i], wayPoints[i + 1]);
        }
    }

    public void OnRetractInput(bool inputDown)
    {
        distance.distance -= rocketController.maxVelocity * Time.deltaTime;
    }

    public void OnMoveInput(Vector2 moveInput)
    {
        //Nope
    }

    public void OnJumpInput()
    {
        //Nope
    }    

    public void OnCursorMove(Vector2 cursorPosition)
    {
        //Nope
    }
}
