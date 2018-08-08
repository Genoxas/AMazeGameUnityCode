using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvasHooks : MonoBehaviour
{
    
    //Lobby player canvas that shows the number of plays currently in the game lobby

    public delegate void CanvasHook();

    public CanvasHook OnAddPlayerHook;

    public Button firstButton;

    public void UIAddPlayer()
    {
        if (OnAddPlayerHook != null)
            OnAddPlayerHook.Invoke();
    }
}
