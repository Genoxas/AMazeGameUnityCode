using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Author: Joseph Ha
 * Description: UI for the main menu of the game. Start and exit buttons 
 */

public class MenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Button startText;
    public Button exitText;

	// Use this for initialization
	void Start () 
    {
        //Game objects in the scene
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        quitMenu.enabled = false;
	}

    //Enable and show the quit menu. Disable the main menu buttons
    public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
    }

    //When quit menu appears and the user clicks no. Reenable the main menu buttons and hide the quit menu
    public void NoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
    }

    //If start is pressed, load the next scene
    public void StartPress()
    {
        Application.LoadLevel(2);
    }

    //If yes is pressed on the quit menu, exit the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
