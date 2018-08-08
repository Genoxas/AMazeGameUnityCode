using System;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Xml;
using System.Collections;
using System.Net;

/*/
    All classes derive from CanvasControl
*/

[Serializable]
public class CanvasControl
{
    [SerializeField]
    public Canvas prefab;
    Canvas m_Canvas;

    public Canvas canvas { get { return m_Canvas; } }

    public virtual void Show()
    {
        if (prefab == null)
            return;
        if (m_Canvas != null)
            return;

        m_Canvas = (Canvas)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        GameObject.DontDestroyOnLoad(m_Canvas.gameObject);
    }

    public void Hide()
    {
        if (m_Canvas == null)
            return;

        GameObject.Destroy(m_Canvas.gameObject);
        m_Canvas = null;
    }

    public virtual void OnLevelWasLoaded()
    {
    }
}

[Serializable]
public class LobbyCanvasControl : CanvasControl
{   
    public override void Show()
    {
        base.Show();

        var hooks = canvas.GetComponent<LobbyCanvasHooks> ();
        if (hooks == null)
            return;

        hooks.OnAddPlayerHook = OnGUIAddPlayer;
    }

    public void OnGUIAddPlayer()
    {
        Test.s_Singleton.popupCanvas.Hide();

        int id = NetworkClient.allClients[0].connection.playerControllers.Count;
        ClientScene.AddPlayer((short)id);
    }

    public void SetFocusToAddPlayerButton()
    {
        var hooks = canvas.GetComponent<LobbyCanvasHooks>();
        if (hooks == null)
            return;

        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }
}

[Serializable]
public class OnlineCanvasControl : CanvasControl
{
    public void Show(string status)
    {
        base.Show();

        Test.s_Singleton.offlineCanvas.Hide();

        var hooks = canvas.GetComponent<OnlineControlHooks>();

        if (hooks == null)
            return;

        hooks.OnStopHook = OnGUIStop;

        hooks.SetAddress(Test.s_Singleton.networkAddress);
        hooks.SetStatus(status);

        Test.s_Singleton.onlineStatus = status;

        EventSystem.current.firstSelectedGameObject = hooks.firstButton.gameObject;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public void OnGUIStop()
    {
        Test.s_Singleton.popupCanvas.Hide();
        Test.s_Singleton.StopHost();
        GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().username = "";
    }
}

[Serializable]
public class OfflineCanvasControl : CanvasControl
{
    public override void Show()
    {
        base.Show();
        Test.s_Singleton.onlineCanvas.Hide();

        var hooks = canvas.GetComponent<OfflineControlHooks>();
        if (hooks == null)
            return;

        hooks.OnStartHostHook = OnGUIStartHost;
        hooks.OnStartServerHook = OnGUIStartServer;
        hooks.OnStartClientHook = OnGUIStartClient;
        hooks.OnStartMMHook = OnGUIStartMatchMaker;
        hooks.OnStartTutorialHook = OnGUIStartTutorial;

        EventSystem.current.firstSelectedGameObject = hooks.firstButton.gameObject;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public override void OnLevelWasLoaded()
    {
        if (canvas == null)
            return;

        var hooks = canvas.GetComponent<OfflineControlHooks>();
        if (hooks == null)
            return;

        EventSystem.current.firstSelectedGameObject = hooks.firstButton.gameObject;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public virtual void ServerChangeScene()
    {

    }

    public void OnGUIStartHost()
    {
      Hide();
      Test.s_Singleton.loginCanavas.Show("Host");
      //Test.s_Singleton.StartHost();
      //Test.s_Singleton.onlineCanvas.Show("Host");
    }

    public void OnGUIStartServer()
    {
        Test.s_Singleton.StartServer();
        Test.s_Singleton.onlineCanvas.Show("Server");
    }

    public void OnGUIStartClient()
    {
      var hooks = canvas.GetComponent<OfflineControlHooks>();
      if (hooks == null)
        return;

      Test.s_Singleton.networkAddress = hooks.GetAddress();
      //Test.s_Singleton.StartClient();
     // Test.s_Singleton.onlineCanvas.Show("Client");
      Hide();
      Test.s_Singleton.loginCanavas.Show("Client");
    }

    public void OnGUIStartTutorial()
    {
        Application.LoadLevel("Tutorial");
        Hide();
    }

    public void OnGUIStartMatchMaker()
    {
        Hide();
        //Test.s_Singleton.StartMatchMaker();
        //Test.s_Singleton.matchMakerCanvas.Show();
        Test.s_Singleton.loginCanavas.Show("MatchMaker");
    }
}

//Login Canvas
[Serializable]
public class LoginCanvasControl : CanvasControl
{
  //private string username;
  private string canvasLoaded = "";
  public void Show(string buttonPressed)
  {
    base.Show();
    canvasLoaded = buttonPressed;
    var hooks = canvas.GetComponent<LoginControlHooks>();
    if (hooks == null)
      return;
    hooks.OnExitHook = OnGUIExitLogin;
    hooks.OnLoginHook = OnGUILogin;
    hooks.OnRegisterHook = OnGUIRegister;
    hooks.OnGuestHook = OnGUIGuestLogin;
  }

  


  public void OnGUILogin()
  {
    string username = canvas.GetComponent<LoginControlHooks>().GetUserName();
    string password = canvas.GetComponent<LoginControlHooks>().GetPassword();

    bool accountFound = GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().GetData(username,password);
    if(accountFound)
    {
      //GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().updateStats();
      Hide();
      if(canvasLoaded.Equals("MatchMaker"))
      { 
        callStartMatchMakerCanvas();
      }
      else if (canvasLoaded.Equals("Host"))
      {
        callLocalHostCanvas();
      }
      else
      {
        callLocalClientCanvas();
      }
    }
    else
    {
     canvas.GetComponent<LoginControlHooks>().errorMessage.text = "Invalid Credentials";
    }
  }

  public void OnGUIRegister()
  {
    string username = canvas.GetComponent<LoginControlHooks>().GetUserName();
    bool userExist = GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().checkIfAccountExist(username);
    if (userExist)
    {
      canvas.GetComponent<LoginControlHooks>().errorMessage.text = "Username Exists";
    }
    else
    {
      string password = canvas.GetComponent<LoginControlHooks>().GetPassword();
      GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().createAccount(username, password);
      canvas.GetComponent<LoginControlHooks>().errorMessage.text = "Account Created";
    }
  }

  public void OnGUIExitLogin()
  {
    Hide();
    Test.s_Singleton.offlineCanvas.Show();
    GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().username = "";
  }

  public void OnGUIGuestLogin()
  {
    //GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().username = "Guest";
    Hide();
    if (canvasLoaded.Equals("MatchMaker"))
    {
      callStartMatchMakerCanvas();
    }
    else if (canvasLoaded.Equals("Host"))
    {
      callLocalHostCanvas();
    }
    else
    {
      callLocalClientCanvas();
    }
  }

  private void callStartMatchMakerCanvas()
  {
    Test.s_Singleton.StartMatchMaker();
    Test.s_Singleton.matchMakerCanvas.Show();
  }

  private void callLocalHostCanvas()
  {
    Test.s_Singleton.StartHost();
    Test.s_Singleton.onlineCanvas.Show("Host");
  }

  private void callLocalClientCanvas()
  {
    Test.s_Singleton.StartClient();
    Test.s_Singleton.onlineCanvas.Show("Client");
  }

}

[Serializable]
public class ConnectingCanvasControl : CanvasControl
{
    public void Show(string address)
    {
        base.Show();


        var hooks = canvas.GetComponent<ConnectingCanvasHooks>();
        if (hooks == null)
            return;

        hooks.OnExitHook = OnGUICancelConnecting;

        hooks.messagText.text = address;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public void OnGUICancelConnecting()
    {
        GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().username = "";
        Hide();
        Test.s_Singleton.StopHost();
    }
}

[Serializable]
public class MatchMakerCanvasControl: CanvasControl
{
    public override void Show()
    {
        base.Show();

        var hooks = canvas.GetComponent<MatchMakerHooks>();
        if (hooks == null)
            return;

        hooks.OnCreateGameHook = OnGUICreateMatchMakerGame;
        hooks.OnJoinGameHook = OnGUIJoinMatchMakerGame;
        hooks.OnExitHook = OnGUIExitMatchMaker;

        hooks.SetMMServer(Test.s_Singleton.matchHost);

        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }


    public void OnGUICreateMatchMakerGame()
    {
        var hooks = canvas.GetComponent<MatchMakerHooks>();
        if (hooks == null)
            return;

        Test.s_Singleton.matchMaker.CreateMatch(
            hooks.GetGameName(),
            (uint)Test.s_Singleton.maxPlayers,
            true,
            "",
            Test.s_Singleton.OnMatchCreate);

        Test.s_Singleton.onlineStatus = "Host Match";

        Hide();

        var host = Test.s_Singleton.matchMaker.baseUri.ToString();
        Test.s_Singleton.connectingCanvas.Show(host);
    }

    public void OnGUIJoinMatchMakerGame()
    {
        Hide();

        Test.s_Singleton.matchMaker.ListMatches(0, 6, "", OnGUIMatchList);

        var host = Test.s_Singleton.matchMaker.baseUri.ToString();
        Test.s_Singleton.connectingCanvas.Show(host);
    }

    void OnGUIMatchList(ListMatchResponse matchList)
    {
        Test.s_Singleton.connectingCanvas.Hide();

        if (matchList.success)
        {
            Test.s_Singleton.joinMatchCanvas.Show(matchList);
        }
        else if (matchList.matches.Count == 0)
        {
            Debug.LogWarning("No Matched found.");
            Show();
        }
        else
        {
            Debug.LogError("Error finding matches");
            Show();
        }
    }

    public void OnGUIExitMatchMaker()
    {
        GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().username = "";
        Test.s_Singleton.StopMatchMaker();
        Hide();
        Test.s_Singleton.offlineCanvas.Show();
    }

}

[Serializable]
public class JoinMatchCanvasControl : CanvasControl
{
    public void Show(ListMatchResponse matchList)
    {
        base.Show();

        Test.s_Singleton.matches = matchList.matches;

        var hooks = canvas.GetComponent<JoinMatchHooks>();
        if (hooks == null)
            return;

        hooks.OnReturnToMMHook = OnGUIReturnToMatchMaker;
        hooks.OnGameHook = OnGUIJoin;

        for (int i = 0; i < 6; i++)
        {
            hooks.SetMatchName(i, "");
        }

        for (int i = 0; i < matchList.matches.Count; i++)
        {
            var match = matchList.matches[i];
            hooks.SetMatchName(i, match.name);
        }

        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public void OnGUIReturnToMatchMaker()
    {
        Hide();
        Test.s_Singleton.matchMakerCanvas.Show();
    }

    public void OnGUIJoin(int index)
    {

        if (index < 0 || index >= Test.s_Singleton.matches.Count)
            return;

        Test.s_Singleton.onlineStatus = "Joined Match";

        Test.s_Singleton.matchMaker.JoinMatch(
            Test.s_Singleton.matches[index].networkId,
            "",
            Test.s_Singleton.OnMatchJoined);

        Hide();
    }
}

[Serializable]
public class ExitToLobbyCanvasControl : CanvasControl
{
    public override void Show()
    {
        base.Show();

        var hooks = canvas.GetComponent<ExitToLobbyHooks>();
        if (hooks == null)
            return;

        hooks.OnExitHook = OnGUIExitToLobby;

        EventSystem.current.firstSelectedGameObject = hooks.firstButton.gameObject;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public override void OnLevelWasLoaded()
    {
        if (canvas == null)
            return;

        var hooks = canvas.GetComponent<ExitToLobbyHooks>();
        if (hooks == null)
            return;

        EventSystem.current.firstSelectedGameObject = hooks.firstButton.gameObject;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public void OnGUIExitToLobby()
    {
        foreach (var player in Test.s_Singleton.lobbySlots)
        {
            if (player != null)
            {
                var playerLobby = player as PlayerLobby;
                if (playerLobby)
                {
                    playerLobby.CmdExitToLobby();
                }
            }
        }
    }
}

[Serializable]
public class PopupCanvasControl : CanvasControl
{
    public void Show(string title, string message)
    {
        base.Show();

        var hooks = canvas.GetComponent<PopupMessageHooks>();
        if (hooks == null)
            return;

        hooks.OnExitHook = OnGUIExitPopup;

        hooks.titleText.text = title;
        hooks.messagText.text = message;

        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public void OnGUIExitPopup()
    {
        Hide();
    }
}