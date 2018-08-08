using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Author: Brian Tat
 * Description: Script used to help with the Inventory UI use to do listener logic on the Item Icons being moved around the inventory
 * Works with InventoryController and Slot Controller to help with the Inventory UI
 */

public class ItemController : MonoBehaviour {

    public Transform orignalParent;
	private int clicks;

	//when the Item icon is moved into a slot
    public void OnTriggerEnter(Collider slot)
    {
      if (transform.parent.parent.GetComponent<InventoryController>().selectedItem != null)
		{
      transform.parent.parent.GetComponent<InventoryController>().selectedSlot = slot.transform;
		}
    }

	//when the item icon is moved out of a slot
	public void OnTriggerExit (Collider slot)
	{
    transform.parent.parent.GetComponent<InventoryController>().selectedSlot = null;
	}

	//When player double left clicks a consummable Item in the inventory it is used
	public void UseItem()
	{
		clicks++;
		if(clicks == 2)
		{
			clicks = 0;
      transform.parent.parent.GetComponent<InventoryController>().useItem(transform.parent.GetComponent<SlotController>().Index);
		}
	}

	//Method called based of an Event Trigger of when the Mouse Pointer (left click) is held down
    public void ItemSelected()
    {
		if (GetComponent<Image>().sprite.name.Equals("UIMask") == false)
		{
      transform.parent.parent.GetComponent<InventoryController>().selectedItem = this.transform;
      transform.parent.parent.GetComponent<InventoryController>().orignalSlot = this.transform.parent.transform;
      transform.parent.parent.GetComponent<InventoryController>().selectedSlot = this.transform.parent.transform;
      transform.parent.parent.GetComponent<InventoryController>().orignalSlot.SetAsLastSibling();
		}
    }

	//Method called based of an Event Trigger of when the Mouse Pointer (left click) is released
    public void ItemDeselected()
    {
      Transform selectedItem = transform.parent.parent.GetComponent<InventoryController>().selectedItem;
		//if player has selected an item go through with logic
		if (selectedItem != null) 
		{
      Transform orignalSlot = transform.parent.parent.GetComponent<InventoryController>().orignalSlot;

      Transform selectedtSlot = transform.parent.parent.GetComponent<InventoryController>().selectedSlot;

			// if player is swapping two items and their locations
			if (selectedItem != null &&
				orignalSlot != null &&
				selectedtSlot != null && orignalSlot != selectedtSlot) {
				string playerID = GameObject.Find ("NetworkManager").GetComponent<Test> ().playerNetworkID;

        PlayerInventory inventory = GameObject.Find(playerID).GetComponent<PlayerInventory>();
				int indexA = orignalSlot.GetComponent<SlotController> ().Index;
				int indexB = selectedtSlot.GetComponent<SlotController> ().Index;

				if (inventory.isItemSlotEmpty (indexA) != true) {
					if (inventory.canSwapItems (indexA, indexB)) {
						inventory.swapItems (indexA, indexB);
						Sprite temp;

						selectedItem.position = orignalSlot.position;
						temp = selectedItem.GetComponent<Image> ().sprite;
						selectedItem.GetComponent<Image> ().sprite = selectedtSlot.GetChild (0).GetComponent<Image> ().sprite;
						selectedtSlot.GetChild (0).GetComponent<Image> ().sprite = temp;
					}
				}
			
			} 
			//if player is deciding to throw their item away
			else if (selectedItem != null && orignalSlot != null && selectedtSlot == null) 
			{
				GameObject.Find("SoundManager").GetComponent<InGameSoundManager>().playInventoryDialogPopupEffect();
				GameObject dialog = GameObject.Find ("/ScreenUI/InventoryDropItemDialog/Panel");
        transform.parent.parent.GetComponent<InventoryController>().setItemToThrowAway(orignalSlot.GetComponent<SlotController>().Index);
				dialog.SetActive (true);
			}

			selectedItem.SetParent (selectedItem.GetComponent<ItemController> ().orignalParent);
			transform.position = transform.parent.GetComponent<SlotController> ().imageOriginalLocation.position;
      transform.parent.parent.GetComponent<InventoryController>().selectedItem = null;
      transform.parent.parent.GetComponent<InventoryController>().orignalSlot = null;
      transform.parent.parent.GetComponent<InventoryController>().selectedSlot = null;
		}
    }

	//Method called based of an Event Trigger of when the Mouse Pointer is moved into a image icon
    public void Movetem()
    {
		clicks = 0;
    if (transform.parent.parent.GetComponent<InventoryController>().selectedItem != null)
        {
			if(!this.transform.parent.transform.Equals(transform.parent.parent.GetComponent<InventoryController>().orignalSlot))
			{
        transform.parent.parent.GetComponent<InventoryController>().selectedSlot = this.transform.parent.transform;
			}
        }
        
    }

	//Method called based of an Event Trigger of when the Mouse Pointer is moved out of a image icon
	public void PointerOut()
	{
		clicks = 0;
    transform.parent.parent.GetComponent<InventoryController>().selectedSlot = null;
	}

}
