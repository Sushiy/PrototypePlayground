using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    public Transform anchorPoint;

    public float legLength = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        if(anchorPoint != null)
            legLength = (anchorPoint.position - transform.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
