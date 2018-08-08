using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes
 * Description: Used by the items and players in order for them to be able to pick up the item
 */

public class PickUpItem : PlayerSlots
{
    [SyncVar]
    public string playerName;
    [SyncVar]
    public GameObject attachedSpawner;
    private bool isPicked;

    private void Start()
    {
        Invoke("SyncItem", 1.5f);
    }

    private void SyncItem()
    {
        if (!string.IsNullOrEmpty(this.playerName))
        {
            base.transform.position = rightHand.transform.position;
            base.transform.parent = rightSlot.transform.parent;
        }
    }

    public void OnTriggerStay(Collider player)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.GetComponent<PlayerActions>().CmdSyncItem(base.gameObject.name);
            player.GetComponent<PlayerActions>().itemId = this.GetComponent<NetworkIdentity>().netId.ToString();

            player.GetComponent<IKSystem>().rightHandObj = base.gameObject.transform;
            player.GetComponent<IKSystem>().lookObj = base.gameObject.transform;
            if (!player.GetComponent<PlayerInventory>().IsInventoryFull(this.gameObject.GetComponent<Item>().itemType))
            {
                player.GetComponent<Animator>().SetBool("GetItem", true);
                player.GetComponent<PlayerMovement>().SetAnim(0f);
            }
        }
    }
}