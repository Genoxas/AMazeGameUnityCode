using UnityEngine;
using UnityEngine.UI;

public class OfflineControlHooks : MonoBehaviour
{
    /*
      First canvas the user sees upon loading the game. Showing options for creating/joining a LAN game.
      Or starting the game as a server. Or starting the matchmaker services
    */

    public delegate void CanvasHook();

    public CanvasHook OnStartHostHook;
    public CanvasHook OnStartServerHook;
    public CanvasHook OnStartClientHook;
    public CanvasHook OnStartMMHook;
    public CanvasHook OnStartTutorialHook;

    public Text addressInput;
    public Button firstButton;

    //Gets address the user inputs for the game. Default is "Localhost"
    public string GetAddress()
    {
        return addressInput.text;
    }

    //Calls when the users clicks "Host LAN Game", invokes OnStartHostHook
    public void UIStartHost()
    {
        if (OnStartHostHook != null)
            OnStartHostHook.Invoke();
    }

    //Calls when user clicks "Start Game as Server" button, invokes OnStartServerhook 
    public void UIStartServer()
    {
        if (OnStartServerHook != null)
            OnStartServerHook.Invoke();
    }

    //Calls when user clicks "Start Game as Server" button, invokes OnStartServerhook 
    public void UIStartClient()
    {
        if (OnStartClientHook != null)
            OnStartClientHook.Invoke();
    }

    //Calls when user clicks "Start MatchMaker" button, invokes OnStartMMHook 
    public void UIStartMM()
    {
        if (OnStartMMHook != null)
            OnStartMMHook.Invoke();
    }

    public void UIExit()
    {
      Application.Quit();
    }

    public void UITutorial()
    {
      if(OnStartTutorialHook != null)
        OnStartTutorialHook.Invoke();
    }    

}
