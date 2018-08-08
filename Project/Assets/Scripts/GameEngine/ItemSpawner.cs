using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes
 * Description: Script used by the server to spawn items for the players - will spawn keys first randomly and then spawn items
 * afterwords randomly. Works with ItemTracker which tells this script when a spawn point is ready to spawn an item.
 */

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;
    [SyncVar]
    private GameObject itemTracked;
    private GameObject[] itemSpawns;
    private string[] keys;
    private string[] itemNames;
    private string[] itemTypes;
    private bool keysAllSpawned = false;

    [Server]
    private void Start()
    {
        this.keys = new string[5];
        this.itemNames = new string[3] { "Sword", "HealthPotion", "StaminaPotion" };
        this.itemTypes = new string[3] { "Equipment", "HealthPotion", "StaminaPotion" };
        for (int i = 1; i < this.keys.Length + 1; i++)
        {
            this.keys[i - 1] = "Key" + i;
        }
        this.itemSpawns = GameObject.FindGameObjectsWithTag("ItemSpawn");
        Invoke("StartSpawning", .5f);
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnKeys());
    }
    IEnumerator SpawnKeys()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            int num = Random.Range(0, this.itemSpawns.Length);
            if (itemSpawns[num].GetComponent<ItemTracker>().hasItem == false && itemSpawns[num].GetComponent<ItemTracker>().gameStart == false)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(this.itemPrefab, this.itemSpawns[num].transform.position, Quaternion.identity) as GameObject;
                gameObject.GetComponent<PickUpItem>().attachedSpawner = this.itemSpawns[num];
                gameObject.GetComponent<Item>().itemName = keys[i];
                gameObject.GetComponent<Item>().itemType = "PuzzleItem";
                this.itemSpawns[num].GetComponent<ItemTracker>().hasItem = true;
                this.itemSpawns[num].GetComponent<ItemTracker>().gameStart = true;
                NetworkServer.Spawn(gameObject);
                RpcNameItems(gameObject, gameObject.GetComponent<NetworkIdentity>().netId.ToString());
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                i -= 1;
            }
        }
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        for (int i = 0; i < itemSpawns.Length-5; i++)
        {
            int num = Random.Range(0, itemSpawns.Length);
            int num2 = Random.Range(0, itemNames.Length);
            if (!this.itemSpawns[num].GetComponent<ItemTracker>().hasItem && !this.itemSpawns[num].GetComponent<ItemTracker>().gameStart)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(this.itemPrefab, this.itemSpawns[num].transform.position, Quaternion.identity) as GameObject;
                gameObject.GetComponent<PickUpItem>().attachedSpawner = this.itemSpawns[num];
                gameObject.GetComponent<Item>().itemName = this.itemNames[num2];
                gameObject.GetComponent<Item>().itemType = this.itemTypes[num2];
                NetworkServer.Spawn(gameObject);
                RpcNameItems(gameObject, gameObject.GetComponent<NetworkIdentity>().netId.ToString());
                this.itemSpawns[num].GetComponent<ItemTracker>().hasItem = true;
                this.itemSpawns[num].GetComponent<ItemTracker>().gameStart = true;
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                i -= 1;
            }
        }
    }

    [Server]
    public void ReadyToSpawn(GameObject itemTracker)
    {
        int num = UnityEngine.Random.Range(0, this.itemNames.Length);
        GameObject gameObject = GameObject.Instantiate(this.itemPrefab, itemTracker.transform.position, Quaternion.identity) as GameObject;
        gameObject.GetComponent<PickUpItem>().attachedSpawner = itemTracker;
        gameObject.GetComponent<Item>().itemName = this.itemNames[num];
        gameObject.GetComponent<Item>().itemType = this.itemTypes[num];
        itemTracker.GetComponent<ItemTracker>().hasItem = true;
        NetworkServer.Spawn(gameObject);
        RpcNameItems(gameObject, gameObject.GetComponent<NetworkIdentity>().netId.ToString());
    }

    [ClientRpc]
    private void RpcNameItems(GameObject item, string name)
    {
        item.name = "Item "+name;
    }
}
