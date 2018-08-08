using UnityEngine;
using System.Collections;

public class ItemContainer : MonoBehaviour
{

    /*
     * Author: Christian Reyes
     * Description: Used to store prefabs for items 
     */

    [SerializeField]
    GameObject swordPrefab;

    public GameObject FindItemToSpawn(string itemName)
    {
        if (itemName == "Equipment")
        {
            return swordPrefab;
        }
        else
        {
            return null;
        }
    }
}
