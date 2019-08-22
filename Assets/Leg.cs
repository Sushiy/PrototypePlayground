using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    Rigidbody[] childRigidbodies;
    ConfigurableJoint[] childConfigurableJoints;

    public bool kinematic = true;
    public bool useSprings = false;
    public float standardSpringForce = 50f;

    // Start is called before the first frame update
    void Start()
    {
        childRigidbodies = GetComponentsInChildren<Rigidbody>();
        childConfigurableJoints = GetComponentsInChildren<ConfigurableJoint>();

        SetKinematic();
        SetUseSprings();
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
