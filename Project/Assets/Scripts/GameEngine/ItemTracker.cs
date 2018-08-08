using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes
 * Description: Script used to track the item in the same position as the spawner. If it has been taken by a player
 * the respawn timer will go down and once the timer hits zero then the item tracker will tell the 'ItemSpawner' to spawn
 * an item in their location
 */

public class ItemTracker : NetworkBehaviour
{
    [SyncVar]
    public bool hasItem;
    [SyncVar]
    public bool gameStart;
    [SyncVar]
    public float respawnTimer = 10f;

    [SerializeField]
    private GameObject itemSpawner;

    [Server]
    private void Update()
    {
        this.TrackItem();
    }

    [Server]
    public void TrackItem()
    {
        if (this.gameStart && !this.hasItem)
        {
            this.respawnTimer = this.respawnTimer - Time.deltaTime;
            if (this.respawnTimer <= 0f)
            {
                this.respawnTimer = 10f;
                this.hasItem = true;
                this.itemSpawner.GetComponent<ItemSpawner>().ReadyToSpawn(this.gameObject);
            }
        }
    }
}
