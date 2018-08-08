using UnityEngine;
using System.Collections;

public class PlayerActionsTutorial : MonoBehaviour {

	// Use this for initialization

  public string selectedItem;
  public string itemId;
  public GameObject PlayerItemToDrop;
  private GameObject currentEquip;
  private Animator anim;

	void Start () {
    anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void CmdDestroyEquip()
  {
    if (currentEquip != null)
    {
      Destroy(currentEquip);
    }
  }

  public void CmdDropItem(GameObject player, GameObject item)
  {
    GameObject change = GameObject.Find(item.name);


    Vector3 positionToPlaceObject = new Vector3(player.transform.position.x, 1.33f, player.transform.position.z);
    
    change.transform.parent = null;
    change.transform.position = positionToPlaceObject;
    change.transform.GetComponent<PickUpItemTutorial>().playerName = "";
    change.transform.GetComponent<Collider>().enabled = true;
    change.GetComponent<MeshRenderer>().enabled = true;
    change.transform.FindChild("Area Light").gameObject.SetActive(true);
    //call RPC command to sync up all players with the item being dropped on the ground
  }

  public void CmdInventorySpawn(string type, string playerID)
  {
    GameObject rightHand = GameObject.Find(playerID).transform.FindChild("Alpha:Hips/Alpha:Spine/Alpha:Spine1/Alpha:Spine2/Alpha:RightShoulder/Alpha:RightArm/Alpha:RightForeArm/Alpha:RightHand/RightSlot").gameObject;
    GameObject spawnItem = GameObject.Instantiate(GameObject.Find("GameManagerTutorial").GetComponent<ItemContainer>().FindItemToSpawn(type), rightHand.transform.position, Quaternion.identity) as GameObject;
    //NetworkServer.Spawn(spawnItem);
    spawnItem.transform.rotation = rightHand.transform.rotation;
    spawnItem.transform.parent = rightHand.transform;
    currentEquip = spawnItem;
  }

  public void CmdDeleteItem(GameObject item)
  {
    Destroy(item);
  }

  public void CmdSyncItem(string item)
    {
        selectedItem = item;
    }

  public void CmdPickUp()
  {
    GameObject item = GameObject.Find(selectedItem);
    item.transform.position = transform.FindChild("Alpha:Hips/Alpha:Spine/Alpha:Spine1/Alpha:Spine2/Alpha:RightShoulder/Alpha:RightArm/Alpha:RightForeArm/Alpha:RightHand").position;
    item.transform.parent = transform.FindChild("Alpha:Hips/Alpha:Spine/Alpha:Spine1/Alpha:Spine2/Alpha:RightShoulder/Alpha:RightArm/Alpha:RightForeArm/Alpha:RightHand");
    GetComponent<Animator>().SetBool("GetItem", false);
    item.GetComponent<PickUpItemTutorial>().playerName = this.gameObject.name;
    if (item.GetComponent<PickUpItemTutorial>().attachedSpawner != null)
    {
      item.GetComponent<PickUpItemTutorial>().attachedSpawner.GetComponent<ItemTracker>().hasItem = false;
      item.GetComponent<PickUpItemTutorial>().attachedSpawner = null;
    }
    ItemTutorial component = item.GetComponent<ItemTutorial>();
    item.GetComponent<MeshRenderer>().enabled = false;
    this.GetComponent<PlayerInventoryTutorial>().AddItem(component);
  }

}
