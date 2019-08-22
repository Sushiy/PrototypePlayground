using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabrikIkPhysics : MonoBehaviour
{
    /// <summary>
    /// chain length of bones
    /// </summary>
    [Header("Leg Parameters")]
    public int chainLength = 2;
    /// <summary>
    /// Target the chain should bent to
    /// </summary>
    public Transform target;

    /// <summary>
    /// pole that controls the rotation of the chain
    /// </summary>
    public Transform pole;

    /// <summary>
    /// Solver iterations per Update
    /// </summary>
    [Header("Solver Parameters")]
    public int iterations = 10;

    /// <summary>
    /// Distance when the solver stops
    /// </summary>
    public float delta = 0.001f;

    /// <summary>
    /// Strength of going back to the start position
    /// </summary>
    [Range(0, 1)]
    public float SnapBackStrength = 1f;

    public bool useSprings = false;

    protected float[] bonesLength;
    protected float completeLength;
    protected Rigidbody[] bones;
    protected Vector3[] positions;

    protected Vector3[] startDirectionSucc;
    protected Quaternion[] startBoneRotations;
    protected Quaternion startTargetRotation;
    protected Quaternion startRootRotation;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    void Init()
    {
        bones = new Rigidbody[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];
        startDirectionSucc = new Vector3[chainLength + 1];
        startBoneRotations = new Quaternion[chainLength + 1];

        completeLength = 0;
        startTargetRotation = target.rotation;

        //init data
        Rigidbody current = GetComponent<Rigidbody>();
        for (int i = chainLength; i >= 0; i--)
        {
            bones[i] = current;
            startBoneRotations[i] = current.rotation;
            if (i == chainLength)
            {
                startDirectionSucc[i] = target.position - current.position;
            }
            else
            {
                startDirectionSucc[i] = bones[i + 1].position - current.position;
                bonesLength[i] = startDirectionSucc[i].magnitude;
                completeLength += bonesLength[i];
            }
            current = current.transform.parent.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ResolveIK();
    }

    private void ResolveIK()
    {
        if (!target) return;

        if (bonesLength.Length != chainLength)
        {
            Init();
        }

        //get positions
        for (int i = 0; i < bones.Length; i++)
        {
            positions[i] = bones[i].position;
        }

        Quaternion rootRot = (bones[0].transform.parent != null) ? bones[0].transform.parent.rotation : Quaternion.identity;
        Quaternion rootRotDiff = rootRot * Quaternion.Inverse(startRootRotation);


        //Calculations
        if ((target.position - bones[0].position).sqrMagnitude >= completeLength * completeLength)
        {
            Vector3 direction = (target.position - positions[0]).normalized;

            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = positions[i - 1] + direction * bonesLength[i - 1];
            }
        }
        else
        {
            for (int i = 0; i < positions.Length - 1; i++)
            {
                positions[i + 1] = Vector3.Lerp(positions[i + 1], positions[i] + rootRotDiff * startDirectionSucc[i], SnapBackStrength);
            }

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                //backward
                for (int i = chainLength; i > 0; i--)
                {
                    if (i == chainLength)
                    {
                        positions[i] = target.position; //set first bone to target
                    }
                    else
                    {
                        positions[i] = positions[i + 1] + (positions[i] - positions[i + 1]).normalized * bonesLength[i]; //set in line on distance;
                    }
                }

                //forward
                for (int i = 1; i < positions.Length; i++)
                {
                    positions[i] = positions[i - 1] + (positions[i] - positions[i - 1]).normalized * bonesLength[i - 1]; //set in line on distance;
                }

                //close enough?
                if ((positions[chainLength] - target.position).sqrMagnitude < delta * delta)
                {
                    break;
                }
            }
        }

        //turn towards pole
        if (pole)
        {
            for (int i = 1; i < positions.Length - 1; i++)
            {
                Plane plane = new Plane(positions[i + 1] - positions[i - 1], positions[i - 1]);
                Vector3 projectedPole = plane.ClosestPointOnPlane(pole.position);
                Vector3 projectedBone = plane.ClosestPointOnPlane(positions[i]);
                float angle = Vector3.SignedAngle(projectedBone - positions[i - 1], projectedPole - positions[i - 1], plane.normal);
                positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (positions[i] - positions[i - 1]) + positions[i - 1];
            }
        }

        // set positions and rotations
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == positions.Length - 1)
            {
                bones[i].MoveRotation(target.rotation * Quaternion.Inverse(startTargetRotation) * startBoneRotations[i]);
            }
            else
            {
                bones[i].MoveRotation(Quaternion.FromToRotation(startDirectionSucc[i], positions[i + 1] - positions[i]) * startBoneRotations[i]);
            }
            bones[i].MovePosition(positions[i]);
        }
    }

    private void OnDrawGizmos()
    {
        Transform current = transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(current.position, current.parent.position);
            current = current.parent;
        }
    }
}