using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetTargetController : MonoBehaviour
{
    Rigidbody rigid = null;
    Vector3 balanceTarget = Vector3.zero;
    Vector3 feetCenter = Vector3.zero;

    float stepSpeed = 0.2f;
    float maxStepdistance = 0.2f;

    float timeSinceLastStep = 0.0f;

    public Foot[] feet;
    public Transform[] anchorpoints;
    public float comfortableFootSpread = 1f;

    int lastFootToMove = 0;
    int movingFoot;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        movingFoot = lastFootToMove;
    }

    // Update is called once per frame
    void Update()
    {
        balanceTarget = rigid.position + rigid.velocity;
        balanceTarget.y = 1.035f;

        if (feet.Length == 0) return;

        feetCenter = Vector3.zero;
        for(int i = 0; i < feet.Length; i++)
        {
            feetCenter += feet[i].transform.position;
        }
        feetCenter /= feet.Length;

        if(timeSinceLastStep >= stepSpeed)
        {
            ChooseFoot();
            feet[(int)movingFoot].transform.position = FindTargetPoint(movingFoot);
            timeSinceLastStep = 0.0f;
        }
        timeSinceLastStep += Time.deltaTime;
    }

    private void ChooseFoot()
    {
        lastFootToMove = movingFoot;
        while (movingFoot == lastFootToMove)
        {
            movingFoot = Random.Range(0, 4);
        }
    }

    private Vector3 FindTargetPoint(int footIndex)
    {
        //Find Vector pointing from the FootCenter to the BalanceTarget
        Vector3 toBalanceTargetVector = (balanceTarget - feetCenter);
        toBalanceTargetVector.y = 0;

        maxStepdistance = rigid.velocity.magnitude;
        if(toBalanceTargetVector.magnitude > maxStepdistance)
        {
            toBalanceTargetVector = toBalanceTargetVector.normalized * maxStepdistance;
        }

        //Find Vector pointing from the foot to the the ComfortableLegPosition
        Vector3 comfortableFootPos = feet[footIndex].transform.position - balanceTarget;
        comfortableFootPos = balanceTarget + comfortableFootPos.normalized * comfortableFootSpread;
        comfortableFootPos = comfortableFootPos - feet[movingFoot].transform.position;
        comfortableFootPos.y = 0;

        return feet[footIndex].transform.position + (toBalanceTargetVector + comfortableFootPos)/2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(balanceTarget, comfortableFootSpread);
        Gizmos.color = Color.blue;
        if(rigid != null)
            Gizmos.DrawWireSphere(rigid.worldCenterOfMass, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feetCenter, 0.1f);
    }
}
