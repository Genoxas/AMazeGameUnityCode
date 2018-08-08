using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCanvasHooks : MonoBehaviour
{
    /*
    The player canvas that gets loaded for each player in  the a lobby. This canvas controls the "ready" status of each player in the game lobby. Depending on the game status of the players, 
    will determine if the game will start. 
    */
    public delegate void CanvasHook();

    public CanvasHook OnReadyHook;
    public CanvasHook OnColorChangeHook;
    public CanvasHook OnRemoveHook;

    public Button playButton;
    public Button colorButton;
    public Button removeButton;
    public Text readyText;
    public Text nameText;
    public RectTransform panelPos;

    bool isLocalPlayer;

    void Awake()
    {
        removeButton.gameObject.SetActive(false);
    }

    public void UIReady()
    {
        if (OnReadyHook != null)
            OnReadyHook.Invoke();
    }

    //Changes color in the lobby canvas
    public void UIColorChange()
    {
        if (OnColorChangeHook != null)
            OnColorChangeHook.Invoke();
    }

    //Option to remove your player canvas in the lobby canvas
    public void UIRemove()
    {
        if (OnRemoveHook != null)
            OnRemoveHook.Invoke();
    }

    //For the local player "YOU" will be designator for your player within the lobby cabnvas
    public void SetLocalPlayer()
    {
        isLocalPlayer = true;
        nameText.text = "YOU";
        readyText.text = "Play";
        removeButton.gameObject.SetActive(true);
    }

    //Sets the color of the player ui in the lobby canvas
    public void SetColor(Color color)
    {
        colorButton.GetComponent<Image>().color = color;
    }

    //Sets the status of a player to ready/not ready and changes text accordingly 
    public void SetReady(bool ready)
    {
        if (ready)
        {
            readyText.text = "Ready";
        }
        else
        {
            if (isLocalPlayer)
            {
                readyText.text = "Play";
            }
            else
            {
                readyText.text = "Not Ready";
            }
        }
    }
}
