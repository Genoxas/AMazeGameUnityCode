using UnityEngine;
using UnityEngine.UI;

public class MatchMakerHooks : MonoBehaviour
{
        
    //Shows all options for the matchmaker. Options for creating or joining a matchmaking game

    public delegate void CanvasHook();

    public CanvasHook OnCreateGameHook;
    public CanvasHook OnJoinGameHook;
    public CanvasHook OnExitHook;

    public Button firstButton;
    public Text gameNameInput;
    public Text mmServerInput;

    //Gets the name of the match 
    public string GetGameName()
    {
        return gameNameInput.text;
    }

    //Gets the MatchMaking server address
    public string GetMMServer()
    {
        return mmServerInput.text;
    }

    //Sets the Machmaking server address
    public void SetMMServer(string value)
    {
        mmServerInput.text = value;
    }

    //Creates a matchmaking game with a designated name
    public void UICreateGame()
    {
        if (OnCreateGameHook != null)
            OnCreateGameHook.Invoke();
    }

    //Disables MM canvas and shows JoinMatchHook that lists all available matches
    public void UIJoinGame()
    {
        if (OnJoinGameHook != null)
            OnJoinGameHook.Invoke();
    }

    //Exits MatchMaking canvas and returns to offlinecanvas
    public void UIExit()
    {
        if (OnExitHook != null)
            OnExitHook.Invoke();
    }
}

