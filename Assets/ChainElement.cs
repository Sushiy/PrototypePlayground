using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainElement : MonoBehaviour
{
    HingeJoint2D hinge;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        hinge = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}
