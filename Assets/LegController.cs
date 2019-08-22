using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour
{
    ConfigurableJoint joint;
    public Vector3 euler;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        joint.targetRotation = Quaternion.Euler(euler);
    }
}
