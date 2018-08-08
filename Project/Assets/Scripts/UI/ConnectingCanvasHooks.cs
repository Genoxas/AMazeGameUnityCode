using UnityEngine;
using UnityEngine.UI;

public class ConnectingCanvasHooks : MonoBehaviour
{

    //Canvas that appears when connecting to a game.
    //Appears when the user is attempting to join a online match

    public delegate void CanvasHook();

    public CanvasHook OnExitHook;

    public Button firstButton;
    public Text titleText;
    public Text messagText;

    public void UIExit()
    {
        if (OnExitHook != null)
            OnExitHook.Invoke();
    }
}

