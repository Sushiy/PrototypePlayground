using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsCoordinator : MonoBehaviour
{
    public bool fLMoving;
    public bool fRMoving;
    public bool bLMoving;
    public bool bRMoving;

    public Foot fLFoot;
    public Foot fRFoot;
    public Foot bLFoot;
    public Foot bRFoot;

    private void Update()
    {
        if(!fRFoot.isMoving && !bRFoot.isMoving)
        {
            fLFoot.TryMove();
            bLFoot.TryMove();
        }

        if (!fLFoot.isMoving && !bLFoot.isMoving)
        {
            fRFoot.TryMove();
            bRFoot.TryMove();
        }
    }
}
