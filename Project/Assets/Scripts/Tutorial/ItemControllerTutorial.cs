using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Author: Brian Tat
 * Description: Script used to help with the Inventory UI use to do listener logic on the Item Icons being moved around the inventory
 * Works with InventoryController and Slot Controller to help with the Inventory UI
 */

public class ItemControllerTutorial : MonoBehaviour
{

  public Transform orignalParent;
  private int clicks;
  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  //when the Item icon is moved into a slot
  public void OnTriggerEnter(Collider slot)
  {
    if (transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedItem != null)
    {
      transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedSlot = slot.transform;
    }
  }

  //when the item icon is moved out of a slot
  public void OnTriggerExit(Collider slot)
  {
    transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedSlot = null;
  }

  //When player double left clicks a consummable Item in the inventory it is used
  public void UseItem()
  {
    clicks++;
    if (clicks == 2)
    {
      clicks = 0;
      transform.parent.parent.GetComponent<InventoryControllerTutorial>().useItem(transform.parent.GetComponent<SlotControllerTutorial>().Index);
    }
  }

  //Method called based of an Event Trigger of when the Mouse Pointer (left click) is held down
  public void ItemSelected()
  {
    if (GetComponent<Image>().sprite.name.Equals("UIMask") == false)
    {
      transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedItem = this.transform;
      transform.parent.parent.GetComponent<InventoryControllerTutorial>().orignalSlot = this.transform.parent.transform;
      transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedSlot = this.transform.parent.transform;
      transform.parent.parent.GetComponent<InventoryControllerTutorial>().orignalSlot.SetAsLastSibling();
    }
  }

  //Method called based of an Event Trigger of when the Mouse Pointer (left click) is released
  public void ItemDeselected()
  {
    Transform selectedItem = transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedItem;
    //if player has selected an item go through with logic
    if (selectedItem != null)
    {
      Transform orignalSlot = transform.parent.parent.GetComponent<InventoryControllerTutorial>().orignalSlot;

      Transform selectedtSlot = transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedSlot;

      // if player is swapping two items and their locations
      if (selectedItem != null &&
        orignalSlot != null &&
        selectedtSlot != null && orignalSlot != selectedtSlot)
      {
        string playerID = "AlphaTest";

        PlayerInventoryTutorial inventory = GameObject.Find(playerID).GetComponent<PlayerInventoryTutorial>();
        int indexA = orignalSlot.GetComponent<SlotControllerTutorial>().Index;
        int indexB = selectedtSlot.GetComponent<SlotControllerTutorial>().Index;

        if (inventory.isItemSlotEmpty(indexA) != true)
        {
          if (inventory.canSwapItems(indexA, indexB))
          {
            inventory.swapItems(indexA, indexB);
            Sprite temp;

            selectedItem.position = orignalSlot.position;
            temp = selectedItem.GetComponent<Image>().sprite;
            selectedItem.GetComponent<Image>().sprite = selectedtSlot.GetChild(0).GetComponent<Image>().sprite;
            selectedtSlot.GetChild(0).GetComponent<Image>().sprite = temp;
          }
        }

      }
      //if player is deciding to throw their item away
      else if (selectedItem != null && orignalSlot != null && selectedtSlot == null)
      {
        GameObject.Find("SoundManager").GetComponent<InGameSoundManager>().playInventoryDialogPopupEffect();
        GameObject dialog = GameObject.Find("/ScreenUI/InventoryDropItemDialog/Panel");
        transform.parent.parent.GetComponent<InventoryControllerTutorial>().setItemToThrowAway(orignalSlot.GetComponent<SlotControllerTutorial>().Index);
        dialog.SetActive(true);
      }

      selectedItem.SetParent(selectedItem.GetComponent<ItemControllerTutorial>().orignalParent);
      transform.position = transform.parent.GetComponent<SlotControllerTutorial>().imageOriginalLocation.position;
      transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedItem = null;
      transform.parent.parent.GetComponent<InventoryControllerTutorial>().orignalSlot = null;
      transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedSlot = null;
    }
  }

  //Method called based of an Event Trigger of when the Mouse Pointer is moved into a image icon
  public void Movetem()
  {
    clicks = 0;
    if (transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedItem != null)
    {
      if (!this.transform.parent.transform.Equals(transform.parent.parent.GetComponent<InventoryControllerTutorial>().orignalSlot))
      {
        transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedSlot = this.transform.parent.transform;
      }
    }

  }

  //Method called based of an Event Trigger of when the Mouse Pointer is moved out of a image icon
  public void PointerOut()
  {
    clicks = 0;
    transform.parent.parent.GetComponent<InventoryControllerTutorial>().selectedSlot = null;
  }

}
