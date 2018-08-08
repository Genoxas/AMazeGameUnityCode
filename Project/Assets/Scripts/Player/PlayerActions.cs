using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Brian Tat & Christan Reyes
 * Description: Custom Script used to send Commands to and From the Server. Since Unity only lets LocalAuthority objects call commands we created this script to send since it is attached to
 * the Alpha(Clone) object which is has LocalAuthority (players have the right to it)
 * [Commands] and [ClientRpc] 
 */

public class PlayerActions : PlayerSlots
{

    [SyncVar]
    public string selectedItem;
    public string itemId;
    private GameObject itemManager;
    private GameObject gameManager;
    public GameObject PlayerItemToDrop;
    private GameObject currentEquip;
    private Animator anim;
    private int finishLineCounter = 0;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(SetVariables());
    }

    IEnumerator SetVariables()
    {
        yield return new WaitForEndOfFrame();
        itemManager = GameObject.Find("ItemManager");
        gameManager = GameObject.Find("GameManager");
        anim = GetComponent<Animator>();
    }

    //Author: Brian Tat
    //Send Command to Server to update a Puzzle 2 Wall (Sequence Puzzle) onces its completed)
    [Command]
    public void CmdUpdatePuzzleWall(GameObject wall)
    {
        while (wall.transform.position.y >= -2)
        {
            wall.transform.Translate(Vector3.down * Time.deltaTime);
        }
        wall.GetComponent<Puzzle2Network>().syncPos = wall.transform.position;
        wall.GetComponent<Puzzle2Network>().IsCompleted = true;
        RpcUpdatePuzzle2Wall(wall);
    }

    //Author: Brian Tat
    //Sends Clients an update to a Puzzle 2 Wall (Sequence Puzzle) onces its completed)
    [ClientRpc]
    private void RpcUpdatePuzzle2Wall(GameObject wall)
    {
        while (wall.transform.position.y >= -2)
        {
            wall.transform.Translate(Vector3.down * Time.deltaTime);
        }
        wall.GetComponent<Puzzle2Network>().syncPos = wall.transform.position;
        wall.GetComponent<Puzzle2Network>().IsCompleted = true;
        if (isLocalPlayer)
        {
            gameManager.GetComponent<GameManager>().IncrementPuzzlesCompleted();
        }
    }

    //Author: Brian Tat
    //Send Command to Server to update a Puzzle Key Wall (Sequence Puzzle) onces its completed)
    [Command]
    public void CmdUpdatePuzzleKeyWall(GameObject wall)
    {
        while (wall.transform.position.y >= -2)
        {
            wall.transform.Translate(Vector3.down * Time.deltaTime);
        }
        wall.GetComponent<PuzzleKeyNetwork>().syncPos = wall.transform.position;
        wall.GetComponent<PuzzleKeyNetwork>().IsCompleted = true;
        RpcUpdatePuzzleKeyWall(wall);
    }
    //Author: Brian Tat
    //Sends Clients an update to a Puzzle Key Wall (Sequence Puzzle) onces its completed)
    [ClientRpc]
    public void RpcUpdatePuzzleKeyWall(GameObject wall)
    {
        while (wall.transform.position.y >= -2)
        {
            wall.transform.Translate(Vector3.down * Time.deltaTime);
        }
        wall.GetComponent<PuzzleKeyNetwork>().syncPos = wall.transform.position;
        wall.GetComponent<PuzzleKeyNetwork>().IsCompleted = true;
        if (isLocalPlayer)
        {
            gameManager.GetComponent<GameManager>().IncrementPuzzlesCompleted();
        }
    }

    //Author: Brian Tat
    //Command sent to server that a Puzzle Sequence has been ended
    [Command]
    public void CmdEndPuzzle(GameObject wall)
    {
        wall.GetComponent<Puzzle2Network>().playerDoingPuzzle = false;
    }

    //Author: Brian Tat
    //Command sent to server that a Puzzle Sequence has been started
    [Command]
    public void CmdStartPuzzle(GameObject wall)
    {
        wall.GetComponent<Puzzle2Network>().playerDoingPuzzle = true;
    }

    //Author: Christan Reyes
    [Command]
    public void CmdSetHasItem(GameObject item)
    {
        item.GetComponent<ItemTracker>().hasItem = false;
    }

    //Author: Christian Reyes
    [Command]
    public void CmdSyncItem(string item)
    {
        selectedItem = item;
    }

    //Author: Christian Reyes
    [Command]
    public void CmdPickUp()
    {
        if (!isServer)
            return;
        GameObject item = GameObject.Find(selectedItem);
        item.transform.position = rightHand.transform.position;
        item.transform.parent = rightHand.transform;


        GetComponent<Animator>().SetBool("GetItem", false);
        item.GetComponent<PickUpItem>().playerName = this.gameObject.name;
        if (item.GetComponent<PickUpItem>().attachedSpawner != null)
        {
            item.GetComponent<PickUpItem>().attachedSpawner.GetComponent<ItemTracker>().hasItem = false;
            item.GetComponent<PickUpItem>().attachedSpawner = null;
        }
        Item component = item.GetComponent<Item>();
        item.GetComponent<MeshRenderer>().enabled = false;
        RpcPickUp();
        //this.GetComponent<PlayerInventory>().AddItem(component);
    }

    [ClientRpc]
    public void RpcPickUp()
    {
        if (isLocalPlayer)
        {
            GameObject item = GameObject.Find(selectedItem);
            item.transform.position = rightHand.transform.position;
            item.transform.parent = rightHand.transform;

            GetComponent<Animator>().SetBool("GetItem", false);
            item.GetComponent<PickUpItem>().playerName = this.gameObject.name;
            if (item.GetComponent<PickUpItem>().attachedSpawner != null)
            {
                item.GetComponent<PickUpItem>().attachedSpawner.GetComponent<ItemTracker>().hasItem = false;
                item.GetComponent<PickUpItem>().attachedSpawner = null;
            }
            Item component = item.GetComponent<Item>();
            item.GetComponent<MeshRenderer>().enabled = false;
            RpcSetItemRendererOff(item);
            this.GetComponent<PlayerInventory>().AddItem(component);
        }
    }

    [ClientRpc]
    private void RpcSetItemRendererOff(GameObject item)
    {
        //Added the if is local player to stop warning errors - if gamebreaking issues occur with pickup remove this
        if (isLocalPlayer)
            return;
        item.GetComponent<MeshRenderer>().enabled = false;
    }

    //Author: Brian Tat
    //Command sent to server that Puzzle Key has been started
    [Command]
    public void CmdStartPuzzleKey(GameObject wall)
    {
        wall.GetComponent<PuzzleKeyNetwork>().playerDoingPuzzle = true;
    }

    //Author: Brian Tat
    //Command sent to server that Puzzle Key has been ended
    [Command]
    public void CmdEmdPuzzleKey(GameObject wall)
    {
        wall.GetComponent<PuzzleKeyNetwork>().playerDoingPuzzle = false;

    }

    //Author: Brian Tat
    //Command Sent to server to remove a game object
    [Command]
    public void CmdDeleteItem(GameObject item)
    {
        Destroy(item);
        RpcDeleteItem(item);
    }

    //Author: Brian Tat
    //Have server update all the clients with the removed game object
    [ClientRpc]
    public void RpcDeleteItem(GameObject item)
    {
        Destroy(item);
    }

    //Author: Brian Tat
    //Send Command to Server to item being dropped by player
    [Command]
    public void CmdDropItem(GameObject player, GameObject item)
    {
        GameObject change = GameObject.Find(item.name);


        Vector3 positionToPlaceObject = new Vector3(player.transform.position.x, 0.286438f, player.transform.position.z);

        change.transform.parent = null;
        change.transform.position = positionToPlaceObject;
        change.transform.GetComponent<PickUpItem>().playerName = "";
        change.transform.GetComponent<Collider>().enabled = true;
        change.GetComponent<MeshRenderer>().enabled = true;
        //call RPC command to sync up all players with the item being dropped on the ground
        RpcDropItem(player, item);
    }

    //Author: Brian Tat
    //Tell Server to update all the clients of item being dropped
    [ClientRpc]
    private void RpcDropItem(GameObject player, GameObject item)
    {
        GameObject change = GameObject.Find(item.name);
        //selectedItem = item;

        Vector3 positionToPlaceObject = new Vector3(player.transform.position.x, 0.286438f, player.transform.position.z);

        change.transform.parent = null;
        change.transform.position = positionToPlaceObject;
        change.transform.GetComponent<PickUpItem>().playerName = "";
        change.transform.GetComponent<Collider>().enabled = true;
        change.GetComponent<MeshRenderer>().enabled = true;
    }

    [Command]
    public void CmdInventorySpawn(string type, string playerID)
    {
        GameObject spawnItem = GameObject.Instantiate(GameObject.Find("ItemManager").GetComponent<ItemContainer>().FindItemToSpawn(type), rightSlot.transform.position, rightSlot.transform.rotation) as GameObject;
        spawnItem.transform.parent = rightSlot.transform;
        currentEquip = spawnItem;
        RpcInventorySpawn(type, playerID);
    }

    [ClientRpc]
    private void RpcInventorySpawn(string type, string playerID)
    {
        if (isServer)
            return;
        GameObject spawnItem = GameObject.Instantiate(GameObject.Find("ItemManager").GetComponent<ItemContainer>().FindItemToSpawn(type), rightSlot.transform.position, rightSlot.transform.rotation) as GameObject;
        spawnItem.transform.parent = rightSlot.transform;
        currentEquip = spawnItem;
    }

    [Command]
    public void CmdDestroyEquip()
    {
        if (currentEquip != null)
        {
            Destroy(currentEquip);
            RpcDestroyEquip();
        }
    }

    [ClientRpc]
    private void RpcDestroyEquip()
    {
        if (currentEquip != null && !isServer)
        {
            Destroy(currentEquip);
        }
    }

    public void RegisterHit(GameObject player, float dmgValue)
    {
        if (!isLocalPlayer)
            return;
        CmdRegisterHit(player, dmgValue);
    }

    [Command]
    public void CmdRegisterHit(GameObject player, float dmgValue)
    {
        player.GetComponent<PlayerStats>().CmdRegisterHit(dmgValue);
    }

    public void OnTriggerEnter(Collider finishLine)
    {
        if (!isLocalPlayer)
            return;
        if (finishLine.name == "FinishLine" && finishLineCounter == 0)
        {
            finishLineCounter++;
            gameManager.GetComponent<GameManager>().IncrementGamesWon();
        }
    }
}