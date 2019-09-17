using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RocketController))]
public class ChainController : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteR;

    RocketController rocketController;

    private float jointIntervals = 0.064f;
    private float jointDelta = 0.0f;

    private List<Vector3> wayPoints;

    public LineRenderer chainRender;

    DistanceJoint2D distance;
    float chainLength = 2;
    public float maxChainLength = 15f;


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        rocketController = GetComponent<RocketController>();
        rocketController.OnFire += OnFire;
        rocketController.OnKeepFiring += OnKeepFiring;

        wayPoints = new List<Vector3>();
        distance = GetComponent<DistanceJoint2D>();
    }

    private void Start()
    {
        if(chainRender)
            chainRender.positionCount = Mathf.RoundToInt(rocketController.maxFlightTime / jointIntervals);
    }

    private void OnFire()
    {
        wayPoints.Clear();
        ResetChainPosition();
        chainLength = 2;
    }

    void ResetChainPosition()
    {
        for (int i = 0; i < chainRender.positionCount; i++)
        {
            chainRender.SetPosition(i, transform.position);
        }
    }

    private void OnKeepFiring()
    {

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

    private void OnDrawGizmos()
    {
        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(wayPoints[i], wayPoints[i + 1]);
        }
    }
}
