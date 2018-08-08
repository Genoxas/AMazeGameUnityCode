using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Joseph Ha (old)
 * Description: Not used 
 */

public class Monster_ID : NetworkBehaviour {

    [SyncVar]
    public string monsterId;
    private Transform myTransform;

	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
    myTransform = transform;
    SetIdentity();
	}

    void SetIdentity()
    {
        if(myTransform.name == "" || myTransform.name == "Monster(Clone)")
        {
            myTransform.name = monsterId;
        }
    }
}
