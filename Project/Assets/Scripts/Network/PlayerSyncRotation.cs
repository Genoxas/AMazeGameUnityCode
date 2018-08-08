using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Joseph Ha
 * Description: Syncing the rotation of the player to the server for other clients
 * 
 */

public class PlayerSyncRotation : NetworkBehaviour
{

    [SyncVar]
    private Quaternion syncPlayerRotation;

    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    float lerpRate = 15;

    private Quaternion lastPlayerRot;
    private float threshold = 2.5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TransmitRotation();
        SetRotationForOtherClients();
    }

    void SetRotationForOtherClients()
    {
        if (!isLocalPlayer)
        {
            playerTransform.rotation = syncPlayerRotation;
        }
    }

    [Command]
    void CmdProvideRotationsToServer(Quaternion playerRotation)
    {
        syncPlayerRotation = playerRotation;
    }
    [Client]
    void TransmitRotation()
    {
        if (isLocalPlayer)
        {
            if (Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold)
            {
                CmdProvideRotationsToServer(playerTransform.rotation);
                lastPlayerRot = playerTransform.rotation;
            }
        }
    }
}