using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainDispenser : MonoBehaviour
{
    public GameObject chainLinkPrefab;
    public float dispenseSpeed = 1f;

    public float currentChainLength = 0;
    public int numOfLinks = 0;
    float jointLength = 1.3f;
    Vector2 connectedAnchorOffset;

    public HingeJoint2D latestJoint;
    public DistanceJoint2D oldestJoint;
    float totalLength = 0f;

    Rigidbody2D rigid;

    List<HingeJoint2D> joints;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        connectedAnchorOffset = new Vector2(0, jointLength);
        latestJoint = GetComponentInChildren<HingeJoint2D>(true);
        SetAsOldestJoint(latestJoint);
        joints = new List<HingeJoint2D>();
        joints.Add(latestJoint);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentChainLength > 1f)
        {
            numOfLinks++;
            AddLink();
        }
        else if (currentChainLength < 0f)
        {
            numOfLinks--;
            RemoveLink();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            currentChainLength += 1f * Time.fixedDeltaTime * dispenseSpeed;
        }
        if (Input.GetMouseButton(1))
        {
            currentChainLength -= 1f * Time.fixedDeltaTime * dispenseSpeed;
        }

        totalLength = (numOfLinks) * jointLength;
        if(oldestJoint)
            oldestJoint.distance = totalLength;

        if (latestJoint != null && latestJoint.gameObject.activeInHierarchy)
            latestJoint.transform.localPosition = Vector3.Lerp(new Vector3(connectedAnchorOffset.x, -connectedAnchorOffset.y, 0), Vector3.zero, currentChainLength);
    }

    void AddLink()
    {
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
            latestJoint.transform.localPosition = new Vector3(0, latestJoint.transform.localPosition.y, 0);
            h.enabled = true;
        }
        else
        {
            h.connectedBody = rigid;
            h.connectedAnchor = Vector2.zero;
            latestJoint = h;
            latestJoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            latestJoint.transform.localEulerAngles = Vector3.zero;
            latestJoint.transform.localPosition = new Vector3(0, latestJoint.transform.localPosition.y, 0);
            SetAsOldestJoint(latestJoint);
            h.enabled = true;
        }
        joints.Add(h);
        currentChainLength = 0.0f;
    }

    void RemoveLink()
    {
        if (joints.Count == 0) return;
        if (joints.Count > 1)
        {
            joints[joints.Count - 2].connectedBody = GetComponent<Rigidbody2D>();
            joints[joints.Count - 2].connectedAnchor = Vector2.zero;
            joints.Remove(latestJoint);
            Destroy(latestJoint.gameObject);
            latestJoint = joints[joints.Count - 1];
            latestJoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            latestJoint.transform.localEulerAngles = Vector3.zero;
            latestJoint.transform.localPosition = new Vector3(0, latestJoint.transform.localPosition.y, 0);
            currentChainLength = 1.0f;
        }
        else
        {
            joints.Remove(latestJoint);
            oldestJoint = null;
            Destroy(latestJoint.gameObject);
        }
    }

    void SetAsOldestJoint(HingeJoint2D newOldest)
    {
        if (newOldest == null) return;
        DistanceJoint2D distance = newOldest.gameObject.AddComponent<DistanceJoint2D>();
        distance.connectedBody = rigid;
        distance.distance = totalLength;
        distance.maxDistanceOnly = true;
        oldestJoint = distance;
    }
}
