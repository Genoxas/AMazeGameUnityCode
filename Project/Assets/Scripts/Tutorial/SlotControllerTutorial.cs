using UnityEngine;
using System.Collections;

/*
 * Author: Brian Tat
 * Description: Script used to help with control the Inventory UI for the user
 * SlotController works with ItemController and InventoryController as the Inventory UI contains an an Inventory Box, Slot Boxes and Item Icons
 */

public class SlotControllerTutorial : MonoBehaviour
{

  //index of the slot number in inventory
  public int Index;
  public Transform imageOriginalLocation;

  // Use this for initialization
  void Start()
  {
    //sets imageOriginalLocation to know where the image came from
    imageOriginalLocation = GetComponentInChildren<Transform>().transform;
  }

  // Update is called once per frame
  void Update()
  {

  }

}