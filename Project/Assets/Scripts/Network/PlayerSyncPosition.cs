using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Joseph Ha
 * Description: Syncing the position of the player to the server for other clients
 * 
 */

public class PlayerSyncPosition : NetworkBehaviour
{

    [SyncVar]
    private Vector3 syncPos;

    [SerializeField]
    Transform myTransform; 
    [SerializeField]
    float lerpRate = 15;

    private Vector3 lastPos;
    private float threshold = 0.05f;

    // Update is called once per frame
    void Update()
    {
        lerpPosition();
    }

    void FixedUpdate()
    {
        TransmitPosition();
    }

    void lerpPosition()
    {
        if (!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvdidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold)
        {
            CmdProvdidePositionToServer(myTransform.position);
            lastPos = myTransform.position;
        }
    }
}
