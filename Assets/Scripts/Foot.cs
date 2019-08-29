using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    RigidbodyLeg leg;
    FastIKFabrik ikSolver;

    public bool isGrounded = false;
    public bool isMoving = false;

    const float STEPTIME = 0.2f;
    
    // Start is called before the first frame update
    void Awake()
    {
        leg = GetComponentInParent<RigidbodyLeg>();
        ikSolver = GetComponent<FastIKFabrik>();
    }
    
    // Update is called once per frame
    public void TryMove()
    {
        if (isMoving) return;

        if((transform.position - leg.centerPoint).sqrMagnitude > leg.maxDistanceFromLeg * leg.maxDistanceFromLeg)
        {
            StartCoroutine(MoveToTarget());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Floor") && isGrounded)
        {
            isGrounded = false;
        }
    }

    IEnumerator MoveToTarget()
    {
        Vector3 oldPosition = ikSolver.target.position;
        Vector3 newPosition = leg.centerPoint + (leg.centerPoint - ikSolver.target.position).normalized * leg.maxDistanceFromLeg;
        float t = 0;
        isMoving = true;
        do
        {
            ikSolver.target.position = Vector3.Lerp(oldPosition, newPosition, t);
            t += Time.deltaTime / STEPTIME;
            yield return null;
        } while (t < 1.0f);
        isMoving = false;
    }
}
