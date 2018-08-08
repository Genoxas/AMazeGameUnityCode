using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes
 * Description: This script handles the win and loose conditions for players in the game as well as provides them a
 * time limit for match completion
 */

public class GameManagerTutorial : MonoBehaviour
{
  private float timeLimit = 500;
  private string message;
  public bool gameOver = false;
  [SerializeField]
  private GameObject OptionPanel;
  private GameObject timer;
  [SerializeField]
  GameObject finishLine;



  private bool IsOptionMenuActive = false;
  // Use this for initialization


  void Start()
  {
    //timer = GameObject.Find("Timer");
    //Invoke("EnableFinishLine", 5);
    GameObject.Find("Camera").transform.parent = null;
  }

  // Update is called once per frame
  void Update()
  {
    //ServerUpdateTimer();
    //DisplayTimer();
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      /*
      if (IsOptionMenuActive)
      {
        GameObject.Find("NetworkManager").GetComponent<Test>().exitToLobbyCanvas.Hide();
        IsOptionMenuActive = false;
      }
      else
      {
        GameObject.Find("NetworkManager").GetComponent<Test>().exitToLobbyCanvas.Show();
        GameObject.Find("ExitToLobbyCanvas2(Clone)").GetComponent<InGameOptionMenu>().setSoundManager(GameObject.Find("SoundManager"));
        IsOptionMenuActive = true;
      }
       */
    }
  }



  private void ServerUpdateTimer()
  {
    if (gameOver == true)
    {
      return;
    }
    else
    {
      timeLimit -= Time.deltaTime;
      if (timeLimit > 0.0f && gameOver == false)
      {
        message = string.Format("{0}:{1:00}", (int)timeLimit / 60, (int)timeLimit % 60);
        timer.GetComponent<Text>().text = "Time Limit: " + message;
      }
      else
      {
        gameOver = true;
        message = "Game Over";
        timer.GetComponent<Text>().text = message;
        //Invoke("ReturnToLobbyScene", 3);
      }
    }
  }

  private void DisplayTimer()
  {
    if (gameOver == false)
    {
      timer.GetComponent<Text>().text = "Time Limit: " + message;
    }
    else
    {
      timer.GetComponent<Text>().text = message;
    }
  }

  public void FinishGame(Collider playerCollider)
  {
    gameOver = true;
    message = playerCollider.transform.name + " wins the match!";
    timer.GetComponent<Text>().text = message;
    //Invoke("ReturnToLobbyScene", 3);
  }

  public void ReturnToLobbyScene()
  {
    //GameObject.Find("NetworkManager").GetComponent<Test>().ServerReturnToLobby();
  }
}