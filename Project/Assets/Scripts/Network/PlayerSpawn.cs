using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSpawn : NetworkBehaviour {

    [SerializeField] private GameObject _playerPrefab;

	// Use this for initialization
	void Start () {
        CreatePlayer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreatePlayer()
    {
        GameObject Player = null;
        Player = (GameObject)GameObject.Instantiate(_playerPrefab, transform.position, Quaternion.identity);

        NetworkServer.Spawn(Player);
    }
}
