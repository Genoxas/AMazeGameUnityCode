using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Joseph Ha
 * Description: Used for spawning the monster
 */

public class MonsterSpawn : NetworkBehaviour
{
    [SerializeField]
    GameObject monsterPrefab;
    [SerializeField]
    GameObject monsterSpawn;

    private int counter;
    private int numberOfMonsters = 1;

    [Server]
    public override void OnStartServer()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            //Invoke("SpawnMonsters", 4);
        }
    }

    [Server]
    void SpawnMonsters()
    {
        counter++;
        GameObject go = GameObject.Instantiate(monsterPrefab, monsterSpawn.transform.position, Quaternion.identity) as GameObject;
        go.GetComponent<Monster_ID>().monsterId = "Monster" + counter;
        NetworkServer.Spawn(go);
    }

    [Server]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            SpawnOnCommand();
        }
    }

    [Server]
    void SpawnOnCommand()
    {
        counter++;
        GameObject go = GameObject.Instantiate(monsterPrefab, monsterSpawn.transform.position, Quaternion.identity) as GameObject;
        go.GetComponent<Monster_ID>().monsterId = "Monster" + counter;
        NetworkServer.Spawn(go);
    }
}
