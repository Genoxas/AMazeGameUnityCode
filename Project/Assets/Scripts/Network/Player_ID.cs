using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Brian Tat
 * Description: Used update the Alpha(Clone) <- Player Object to a suitable name to find help with updating clients information on the network
 */

public class Player_ID : NetworkBehaviour
{
    [SyncVar]
    public string playerUniqueIdentity;
    private NetworkInstanceId playerNetID;
    public Transform myTransform;

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
        SetIdentity();

    }


    // Use this for initialization
    void Awake()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (myTransform.name == "" || myTransform.name == "Jim(Clone)")
        {
            SetIdentity();
        }
    }

	//Client gets their Network ID and tells the server their network identity
    [Client]
    void GetNetIdentity()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }

	//Client setIdentity if it has not been set
    [Client]
    void SetIdentity()
    {
        if (!isLocalPlayer)
        {
            myTransform.name = playerUniqueIdentity;
        }
        else
        {
            myTransform.name = MakeUniqueIdentity();
            GameObject.Find("NetworkManager").GetComponent<Test>().playerNetworkID = MakeUniqueIdentity();
        }
    }

	//Create Player + ID name
    string MakeUniqueIdentity()
    {
        string uniqueName = "Player " + playerNetID.ToString();
        return uniqueName;
    }

	//Command sent to server update player's Alpha(Clone) on the server
    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        playerUniqueIdentity = name;
    }



}
