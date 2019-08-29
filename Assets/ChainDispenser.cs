using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainDispenser : MonoBehaviour
{
    public GameObject chainLinkPrefab;
    public float dispenseSpeed = 1f;

    public float currentChainLength = 0;
    public int numOfLinks = 0;

    public HingeJoint2D latestJoint;
    Vector2 connectedAnchorOffset = new Vector2(0, 1.3f);

    List<HingeJoint2D> joints;

    // Start is called before the first frame update
    void Start()
    {
        latestJoint = GetComponentInChildren<HingeJoint2D>();
        joints = new List<HingeJoint2D>();
        joints.Add(latestJoint);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            currentChainLength += 1f * Time.deltaTime * dispenseSpeed;
            if(latestJoint != null)
                latestJoint.transform.localPosition += latestJoint.transform.up * -1.3f * dispenseSpeed * Time.deltaTime;
        }
        if(Input.GetMouseButton(1))
        {
            currentChainLength -= 1f * Time.deltaTime * dispenseSpeed;
            if (latestJoint != null)
                latestJoint.transform.localPosition -= latestJoint.transform.up * -1.3f * dispenseSpeed * Time.deltaTime;
        }

        if (Mathf.FloorToInt(currentChainLength) > numOfLinks)
        {
            numOfLinks++;
            HingeJoint2D h = Instantiate(chainLinkPrefab, transform).GetComponent<HingeJoint2D>();
            if (latestJoint != null)
            {
                h.connectedBody = latestJoint.connectedBody;
                h.connectedAnchor = Vector2.zero;
                latestJoint.connectedBody = h.GetComponent<Rigidbody2D>();
                latestJoint.connectedAnchor = connectedAnchorOffset;
                latestJoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                latestJoint = h;
                latestJoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                latestJoint.transform.localEulerAngles = Vector3.zero;
                latestJoint.transform.localPosition = new Vector3(0, latestJoint.transform.localPosition.y,0);
                h.enabled = true;
            }
            else
            {
                h.connectedBody = GetComponent<Rigidbody2D>();
                h.connectedAnchor = Vector2.zero;
                latestJoint = h;
                latestJoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                latestJoint.transform.localEulerAngles = Vector3.zero;
                latestJoint.transform.localPosition = new Vector3(0, latestJoint.transform.localPosition.y, 0);
                h.enabled = true;
            }
            joints.Add(h);
        }
        else if (Mathf.FloorToInt(currentChainLength) < numOfLinks)
        {
            numOfLinks--;
            if(joints.Count > 1)
            {
                joints[joints.Count - 2].connectedBody = GetComponent<Rigidbody2D>();
                joints[joints.Count - 2].connectedAnchor = Vector2.zero;
                joints.Remove(latestJoint);
                Destroy(latestJoint.gameObject);
                latestJoint = joints[joints.Count - 1];
                latestJoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                latestJoint.transform.localEulerAngles = Vector3.zero;
                latestJoint.transform.localPosition = new Vector3(0, latestJoint.transform.localPosition.y, 0);
            }
            else
            {
                joints.Remove(latestJoint);
                Destroy(latestJoint.gameObject);
            }


        }
    }
}
