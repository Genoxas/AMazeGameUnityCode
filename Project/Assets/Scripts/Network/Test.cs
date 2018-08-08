using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class Test : NetworkLobbyManager
{

    /*
     * Author: Joseph Ha 
     * Description: Settings for the Network Manager
     */
    public LobbyCanvasControl lobbyCanvas;
    public OfflineCanvasControl offlineCanvas;
    public OnlineCanvasControl onlineCanvas;
    public ExitToLobbyCanvasControl exitToLobbyCanvas;
    public ConnectingCanvasControl connectingCanvas;
    public PopupCanvasControl popupCanvas;
    public MatchMakerCanvasControl matchMakerCanvas;
    public JoinMatchCanvasControl joinMatchCanvas;
    public LoginCanvasControl loginCanavas;

    private GameObject[] players;
    public string playerNetworkID;
    public string onlineStatus;


    static public Test s_Singleton;

    // Use this for initialization
    void Start()
    {

        s_Singleton = this;

        //When the game is loaded show the offlineCanvas to the user
        offlineCanvas.Show();

        base.matchSize = 5;
        base.maxConnections = 5;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnLevelWasLoaded()
    {
        if (lobbyCanvas != null) lobbyCanvas.OnLevelWasLoaded();
        if (offlineCanvas != null) offlineCanvas.OnLevelWasLoaded();
        if (onlineCanvas != null) onlineCanvas.OnLevelWasLoaded();
        if (exitToLobbyCanvas != null) exitToLobbyCanvas.OnLevelWasLoaded();
        if (connectingCanvas != null) connectingCanvas.OnLevelWasLoaded();
        if (popupCanvas != null) popupCanvas.OnLevelWasLoaded();
        if (matchMakerCanvas != null) matchMakerCanvas.OnLevelWasLoaded();
        if (joinMatchCanvas != null) joinMatchCanvas.OnLevelWasLoaded();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }

    public void SetFocusToAddPlayerButton()
    {
        if (lobbyCanvas == null)
            return;

        lobbyCanvas.SetFocusToAddPlayerButton();
    }

    public override void OnLobbyStopHost()
    {
        lobbyCanvas.Hide();
        offlineCanvas.Show();
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //This hook allows you to apply state data from the lobby-player to the game-player
        //var cc = lobbyPlayer.GetComponent<ColorControl>();
        //var playerX = gamePlayer.GetComponent<Player>();
        //playerX.myColor = cc.myColor;
        return true;
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        connectingCanvas.Hide();
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        connectingCanvas.Hide();
        StopHost();

        popupCanvas.Show("Client Error", errorCode.ToString());
    }

    public override void OnLobbyClientDisconnect(NetworkConnection conn)
    {
        lobbyCanvas.Hide();
        offlineCanvas.Show();
    }

    public override void OnLobbyStartClient(NetworkClient client)
    {
        if (matchInfo != null)
        {
            connectingCanvas.Show(matchInfo.address);
        }
        else
        {
            connectingCanvas.Show(networkAddress);
        }
    }

    //Calls when the lobby reachers max number of players
    public override void OnLobbyClientAddPlayerFailed()
    {
        popupCanvas.Show("Error", "No more players allowed.");
    }

    public override void OnLobbyClientEnter()
    {
        lobbyCanvas.Show();
        onlineCanvas.Show(onlineStatus);

        exitToLobbyCanvas.Hide();

    }

    public override void OnLobbyClientExit()
    {
        lobbyCanvas.Hide();
        onlineCanvas.Hide();

        if (Application.loadedLevelName == base.playScene)
        {
            //exitToLobbyCanvas.Show();
        }
    }
}
