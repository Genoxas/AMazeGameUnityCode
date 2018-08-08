using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Brian Tat
 * Description: Item class
 */

public class Item : NetworkBehaviour {

    [SyncVar]
    public int id;
    [SyncVar]
    public string itemName;
    [SyncVar]
    public string itemType;

	// Use this for initialization


}
