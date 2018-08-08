using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Author: Brian Tat
 * Description: Player Inventory does backend tracking on player's items they have in their inventory
 */

public class PlayerInventory : MonoBehaviour
{
    private Item[] items;

    //determines Inventory size limit
    private int inventorySize = 4;
    private GameObject invUI;

    // Use this for initialization 
    void Start()
    {
        items = new Item[inventorySize];
        invUI = GameObject.Find("InventoryUI");
    }

	//Get an item Name based on index 
    public string GetItemName(int index)
    {
        return items[index].itemName;
    }

	//Get an item based on index 
	public Item getItem(int index)
	{
		return items [index];
	}

    //Add item to player inventory
    public void AddItem(Item item)
    {
        int startIndex = 0;
        if (item.itemType.Equals("Equipment") != true)
        {
            startIndex = 1;
        }
        for (int i = startIndex; i < inventorySize; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                //Debug.Log("Item: " + item.itemName + " Added to slot " + i);
                invUI.GetComponent<InventoryController>().updateSlotImage(i, item.itemType);

                break;
            }
        }
    }

    //method to check if inventory is full
    public bool IsInventoryFull(string itemType)
    {
        int index = 0;
        bool full = true;
        if (!itemType.Equals("Equipment"))
        {
            index = 1;
        }
        for (int i = index; i < inventorySize; i++)
        {
            if (items[i] == null)
            {
                full = false;
                break;
            }
        }

        return full;
    }

    //method to receieve the inventory list
    public Item[] getItemList()
    {
        return items;
    }

	//Remove an item from inventory based on index
    public void removeItem(int index)
    {

        items[index] = null;

        invUI.GetComponent<InventoryController>().updateSlotImage(index, "null");
    }

	//Swap items around the inventory
    public void swapItems(int item1Index, int item2Index)
    {
        Item temp = items[item1Index];
        items[item1Index] = items[item2Index];
        items[item2Index] = temp;
    }

	//Checks if items are swappable (you cannot move a regular item to the "On-Hand" slot as it is reserved for equipment only
    public bool canSwapItems(int slotAIndex, int slotBIndex)
    {
        //check to see if they are trying to move items to the On-Hand

        if (slotAIndex == 0 || slotBIndex == 0)
        {
            if (slotAIndex == 1 || slotBIndex == 1)
            {
                if (items[1] == null)

                    return false;

                else if (items[1].itemType.Equals("Equipment") != true)
                    return false;
            }
            else if (slotAIndex == 2 || slotBIndex == 2)
            {
                if (items[2] == null)

                    return false;

                else if (items[2].itemType.Equals("Equipment") != true)
                    return false;
            }
            else
            {
                if (items[3] == null)

                    return false;

                else if (items[3].itemType.Equals("Equipment") != true)
                    return false;
            }

        }
        return true;
    }

	//Checks if a item slot is empty in the inventory
    public bool isItemSlotEmpty(int index)
    {
        bool result = false;
        if (items[index] == null)
            result = true;
        return result;
    }

}
