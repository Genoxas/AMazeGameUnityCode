using System;
using UnityEngine;
using UnityEngine.UI;

public class ExitToLobbyHooks : MonoBehaviour
{
    //Button that gets loaded while playing the game. Exits to lobby when exit button is clicked

    public delegate void CanvasHook();

    public CanvasHook OnExitHook;

    public Button firstButton;

    public void UIExit()
    {
        if (OnExitHook != null)
            OnExitHook.Invoke();
    }
}
