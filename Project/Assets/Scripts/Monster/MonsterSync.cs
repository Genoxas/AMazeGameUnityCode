using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes
 * Description: Script used to sync monster between server and other clients connected to the same game
 */

public class MonsterSync : NetworkBehaviour
{

    [SyncVar]
    private Vector3 clientSyncPos;
    [SyncVar]
    private float clientSyncRot;

    private Vector3 lastPos;
    private Quaternion lastRot;
    private Transform currentPos;
    private float posThreshhold = 0.25f;
    private float rotThreshhold = 2.5f;

    // Use this for initialization
    void Start()
    {
        currentPos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        TransmitMotion();
        LerpMotion();
    }
    void TransmitMotion()
    {
        if (!isServer)
        {
            return;
        }
        else
        {
            if (Vector3.Distance(currentPos.position, lastPos) > posThreshhold || Quaternion.Angle(currentPos.transform.rotation, lastRot) > rotThreshhold)
            {
                lastPos = currentPos.transform.position;
                lastRot = currentPos.transform.rotation;

                clientSyncPos = currentPos.position;
                clientSyncRot = currentPos.localEulerAngles.y;
            }
        }
    }

    void LerpMotion()
    {
        if (isServer)
        {
            return;
        }
        currentPos.position = Vector3.Lerp(currentPos.position, clientSyncPos, Time.deltaTime * 10);

        Vector3 newRot = new Vector3(0, clientSyncRot, 0);
        currentPos.rotation = Quaternion.Lerp(currentPos.rotation, Quaternion.Euler(newRot), Time.deltaTime * 10);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
    }

    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
    }
}
