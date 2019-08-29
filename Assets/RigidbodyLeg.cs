using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyLeg : MonoBehaviour
{
    Rigidbody[] childRigidbodies;
    ConfigurableJoint[] childConfigurableJoints;

    public bool kinematic;
    bool oldKinematic;
    public bool useSprings = false;
    public float standardSpringForce = 50f;

    public LayerMask floorLayer;
    public Vector3 centerPoint = Vector3.zero;
    public Vector3 centerPointOffset;
    public float maxDistanceFromLeg = 1f;

    // Start is called before the first frame update
    void Start()
    {
        childRigidbodies = GetComponentsInChildren<Rigidbody>();
        childConfigurableJoints = GetComponentsInChildren<ConfigurableJoint>();

        SetKinematic();
        SetUseSprings();
        oldKinematic = kinematic;
    }

    // Update is called once per frame
    void Update()
    {
        if(oldKinematic != kinematic)
        {
            SetKinematic();
        }
        oldKinematic = kinematic;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, 2.0f, floorLayer))
        {
            centerPoint = hit.point + centerPointOffset;
        }
        else
        {
            centerPoint = transform.position + new Vector3(0, -1.5f, 0);
        }
    }

    public void SetKinematic()
    {
        for(int i = 0; i < childRigidbodies.Length; i++)
        {
            childRigidbodies[i].isKinematic = kinematic;
        }
    }

    public void SetUseSprings()
    {
        for (int i = 0; i < childConfigurableJoints.Length; i++)
        {
            childConfigurableJoints[i].angularXDrive = new JointDrive { positionSpring = useSprings ? standardSpringForce : 0, positionDamper = 10 };
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(centerPoint, maxDistanceFromLeg);
    }

}
