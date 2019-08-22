using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    Leg leg;

    public bool isGrounded = false;
    
    // Start is called before the first frame update
    void Start()
    {
        leg = GetComponentInParent<Leg>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Floor"))
        {
            isGrounded = true;
            //leg.SetKinematic(true);
            //GetComponent<FastIKFabrik>().enabled = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Floor") && isGrounded)
        {
            isGrounded = false;
            //leg.SetKinematic(false);
            //GetComponent<FastIKFabrik>().enabled = false;
        }
    }
}
