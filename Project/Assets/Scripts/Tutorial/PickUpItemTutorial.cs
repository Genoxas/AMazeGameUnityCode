using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes
 * Description: Used by the items and players in order for them to be able to pick up the item
 */

public class PickUpItemTutorial : MonoBehaviour
{
  public string playerName;
  public GameObject attachedSpawner;
  private bool isPicked;

  private void Start()
  {
    //Invoke("SyncItem", 1.5f);
  }

  private void SyncItem()
  {
    if (!string.IsNullOrEmpty(this.playerName))
    {
      base.transform.position = GameObject.Find(this.playerName + "/Alpha:Hips/Alpha:Spine/Alpha:Spine1/Alpha:Spine2/Alpha:RightShoulder/Alpha:RightArm/Alpha:RightForeArm/Alpha:RightHand").transform.position;
      base.transform.parent = GameObject.Find(this.playerName + "/Alpha:Hips/Alpha:Spine/Alpha:Spine1/Alpha:Spine2/Alpha:RightShoulder/Alpha:RightArm/Alpha:RightForeArm/Alpha:RightHand").transform.parent;
    }
  }


  public void OnTriggerStay(Collider player)
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      player.GetComponent<PlayerActionsTutorial>().CmdSyncItem(base.gameObject.name);
      //player.GetComponent<PlayerActions>().itemId = this.GetComponent<NetworkIdentity>().netId.ToString();

      player.GetComponent<IKSystemTutorial>().rightHandObj = base.gameObject.transform;
      player.GetComponent<IKSystemTutorial>().lookObj = base.gameObject.transform;
      if (!player.GetComponent<PlayerInventoryTutorial>().IsInventoryFull(this.gameObject.GetComponent<ItemTutorial>().itemType))
      {
        player.GetComponent<Animator>().SetBool("GetItem", true);
        player.GetComponent<PlayerMovementTutorial>().SetAnim(0f);
        this.transform.FindChild("Area Light").gameObject.SetActive(false);
      }
    }
  }
}