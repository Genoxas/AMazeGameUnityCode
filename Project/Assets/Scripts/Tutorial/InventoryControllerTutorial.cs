﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Author: Brian Tat
 * Description: Script used to help with control the Inventory UI for the user
 * InventoryController works with ItemController and Slot Controller as the Inventory UI contains an an Inventory Box, Slot Boxes and Item Icons
 */

public class InventoryControllerTutorial : MonoBehaviour
{
  public Transform selectedItem;
  public Transform selectedSlot;
  public Transform orignalSlot;
  private int ItemToThrowAway;

  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButton(0) && selectedItem != null)
    {
      selectedItem.position = Input.mousePosition;
    }
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      useItem(1);
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      useItem(2);
    }
    if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      useItem(3);
    }

  }

  //Method use to update the Slot Image in Inventory UI based on the index (slot location) and type of item
  public void updateSlotImage(int index, string type)
  {
    if (index == 0)
    {
      GameObject itemImage = GameObject.Find("ItemImage0");
      Image itemComp = itemImage.GetComponent<Image>();
      itemComp.sprite = determineImage(type, itemImage);
      string playerID = "AlphaTest";

      GameObject player = GameObject.Find(playerID);
      player.GetComponent<PlayerActionsTutorial>().CmdDestroyEquip();
      if (type.Equals("null"))
      {
        player.GetComponent<PlayerCombatTutorial>().hasWeapon = false;
        player.GetComponentInChildren<SwordDamage>().isPickedUp = false;
      }
      else
      {
        player.GetComponent<PlayerActionsTutorial>().CmdInventorySpawn(type, playerID);
        player.GetComponent<PlayerCombatTutorial>().hasWeapon = true;
        player.GetComponentInChildren<SwordDamage>().isPickedUp = true;
      }
    }
    else if (index == 1)
    {
      GameObject itemImage = GameObject.Find("ItemImage1");
      Image itemComp = itemImage.GetComponent<Image>();
      itemComp.sprite = determineImage(type, itemImage);
    }
    else if (index == 2)
    {
      GameObject itemImage = GameObject.Find("ItemImage2");
      Image itemComp = itemImage.GetComponent<Image>();
      itemComp.sprite = determineImage(type, itemImage);
    }
    else
    {
      GameObject itemImage = GameObject.Find("ItemImage3");
      Image itemComp = itemImage.GetComponent<Image>();
      itemComp.sprite = determineImage(type, itemImage);
    }
  }

  //Method used to determine what type of update to
  private Sprite determineImage(string type, GameObject itemImage)
  {
    if (type.Equals("Equipment"))
    {
      return itemImage.GetComponent<SpriteController>().Sword;
    }
    else if (type.Equals("PuzzleItem"))
    {
      return itemImage.GetComponent<SpriteController>().Key;
    }
    else if (type.Equals("HealthPotion"))
    {
      return itemImage.GetComponent<SpriteController>().HealthPotion;
    }
    else if (type.Equals("StaminaPotion"))
    {
      return itemImage.GetComponent<SpriteController>().StaminaPotion;
    }

    return itemImage.GetComponent<SpriteController>().UIMask;
  }

  //Method used to setItemPlayer wants to throw away
  public void setItemToThrowAway(int index)
  {
    ItemToThrowAway = index;
  }

  //Method for when Player decides not to throw the item away
  public void DropButtonNoPressed()
  {
    GameObject.Find("SoundManager").GetComponent<InGameSoundManager>().playInventoryDialogPopupNoEffect();
    GameObject dialog = GameObject.Find("/ScreenUI/InventoryDropItemDialog/Panel");
    ItemToThrowAway = -1;
    dialog.SetActive(false);
  }

  //Method for when Player decides to throw the item away
  public void DropButtonYesPressed()
  {
    GameObject.Find("SoundManager").GetComponent<InGameSoundManager>().playInventoryDialogPopupYesEffect();
    GameObject dialog = GameObject.Find("/ScreenUI/InventoryDropItemDialog/Panel");
    string playerID = "AlphaTest";

    PlayerInventoryTutorial inventory = GameObject.Find(playerID).GetComponent<PlayerInventoryTutorial>();
    string itemToRemove = inventory.GetItemName(ItemToThrowAway);

    foreach (Transform child in GameObject.Find(playerID).transform.FindChild("Alpha:Hips/Alpha:Spine/Alpha:Spine1/Alpha:Spine2/Alpha:RightShoulder/Alpha:RightArm/Alpha:RightForeArm/Alpha:RightHand"))
    {
      if (child.name.Contains("Item"))
      {
        if (child.GetComponent<ItemTutorial>().itemName.Equals(itemToRemove))
        {
          GameObject.Find(playerID).GetComponent<PlayerActionsTutorial>().CmdDropItem(GameObject.Find(playerID), child.gameObject);
        }
      }
    }
    inventory.removeItem(ItemToThrowAway);
    dialog.SetActive(false);
  }

  //Method for when Player uses a consummable Item like a potion
  public void useItem(int index)
  {
    string playerID = "AlphaTest";

    PlayerInventoryTutorial inventory = GameObject.Find(playerID).GetComponent<PlayerInventoryTutorial>();
    ItemTutorial item = inventory.getItem(index);
    if (item.itemType.Equals("HealthPotion"))
    {
      GameObject.Find("SoundManager").GetComponent<InGameSoundManager>().playHpHealEffect();
      GameObject.Find(playerID).GetComponent<PlayerStatTutorial>().CmdAddHealth(20f);
      inventory.removeItem(index);
      updateSlotImage(index, "");
      foreach (Transform child in GameObject.Find(playerID).transform.FindChild("Alpha:Hips/Alpha:Spine/Alpha:Spine1/Alpha:Spine2/Alpha:RightShoulder/Alpha:RightArm/Alpha:RightForeArm/Alpha:RightHand"))
      {
        if (child.name.Contains("Item"))
        {
          if (child.GetComponent<ItemTutorial>().itemName.Equals(item.itemName))
          {
            GameObject.Find(playerID).GetComponent<PlayerActionsTutorial>().CmdDeleteItem(child.gameObject);
            break;
          }
        }
      }
    }
    else if (item.itemType.Equals("StaminaPotion"))
    {
      GameObject.Find("SoundManager").GetComponent<InGameSoundManager>().playHpHealEffect();
      GameObject.Find(playerID).GetComponent<PlayerMovementTutorial>().CmdAddStamina();
      inventory.removeItem(index);
      updateSlotImage(index, "");
      foreach (Transform child in GameObject.Find(playerID).transform.FindChild("Alpha:Hips/Alpha:Spine/Alpha:Spine1/Alpha:Spine2/Alpha:RightShoulder/Alpha:RightArm/Alpha:RightForeArm/Alpha:RightHand"))
      {
        if (child.name.Contains("Item"))
        {
          if (child.GetComponent<ItemTutorial>().itemName.Equals(item.itemName))
          {
            GameObject.Find(playerID).GetComponent<PlayerActionsTutorial>().CmdDeleteItem(child.gameObject);
            break;
          }
        }
      }
    }
  }
}