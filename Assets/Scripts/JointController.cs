using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointController : MonoBehaviour
{
    ConfigurableJoint joint;
    public Vector3 euler;
    public Transform rotationSource;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rotationSource)
            euler = rotationSource.localRotation.eulerAngles;
        joint.targetRotation = Quaternion.Euler(euler);
    }
}
