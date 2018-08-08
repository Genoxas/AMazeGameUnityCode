using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManager : NetworkLobbyManager {
    /*
      public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
      {
          Debug.Log("Added player!");
          base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
      }
     */
    // Use this for initialization

    public string playerNetworkID;
    //public uint matchSize = 10;

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }

    void Start()
    {
        base.matchSize = 5;
        base.maxConnections = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
