using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes
 * Description: This script handles the win and loose conditions for players in the game as well as provides them a
 * time limit for match completion
 */

public class GameManager : NetworkBehaviour
{
    [SerializeField]
    GameObject finishLine;
    [SyncVar]
    private float timeLimit = 1200;
    private float maxTimeLimit = 1200;
    [SyncVar]
    private string message;
    [SyncVar]
    public bool gameOver = false;
    private GameObject timer;
    private GameObject networkManager;
    private string playerName;
    private string playerWonName;

    private bool IsOptionMenuActive = false;
    // Use this for initialization

    void Start()
    {
        GameObject.Find("NameText").GetComponent<Text>().text = GameObject.Find("NetworkManager").GetComponent<AccountRetriever>().username;
        timer = GameObject.Find("TimerText");
        Invoke("CmdEnableFinishLine", 7);
        StartCoroutine(SetNetworkManager());
    }

    IEnumerator SetNetworkManager()
    {
        yield return new WaitForSeconds(0.3f);
        networkManager = GameObject.Find("NetworkManager");
    }

    // Update is called once per frame
    void Update()
    {
        ServerUpdateTimer();
        DisplayTimer();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        }
    }

    [Command]
    private void CmdEnableFinishLine()
    {
        finishLine.SetActive(true);
        RpcEnableFinishLine();
    }

    [ClientRpc]
    private void RpcEnableFinishLine()
    {
        finishLine.SetActive(true);
    }

    [Server]
    private void ServerUpdateTimer()
    {
        if (gameOver == true)
        {
            return;
        }
        else
        {
            timeLimit -= Time.deltaTime;
            GameObject.Find("TimerBar").GetComponent<Image>().fillAmount = (timeLimit / maxTimeLimit);
            if (timeLimit > 0.0f && gameOver == false)
            {
                message = string.Format("{0}:{1:00}", (int)timeLimit / 60, (int)timeLimit % 60);
                timer.GetComponent<Text>().text = message;
            }
            else
            {
                gameOver = true;
                message = "Game Over";
                timer.GetComponent<Text>().text = message;
                Invoke("CmdReturnToLobbyScene", 3);
            }
        }
    }

    private void DisplayTimer()
    {
        if (gameOver == false)
        {
            GameObject.Find("TimerBar").GetComponent<Image>().fillAmount = (timeLimit / maxTimeLimit);
            timer.GetComponent<Text>().text = message;
        }
        else
        {
            timer.GetComponent<Text>().text = message;
        }
    }

    [Server]
    public void CmdFinishLine(Collider playerCollider)
    {
        gameOver = true;
        //message = playerCollider.transform.name + " wins the match!";
        message = "A player has completed the maze!";
        timer.GetComponent<Text>().text = message;
        Invoke("CmdReturnToLobbyScene", 3);
    }

    [Command]
    public void CmdReturnToLobbyScene()
    {
        RpcReturnToLobbyScene();
    }

    [ClientRpc]
    public void RpcReturnToLobbyScene()
    {
        if (networkManager.GetComponent<AccountRetriever>().username != "")
        {
            UpdateAccount();
        }
        GameObject.Find("NetworkManager").GetComponent<Test>().ServerReturnToLobby();
    }

    public void IncrementGamesWon()
    {
        if (networkManager.GetComponent<AccountRetriever>().username == "")
            return;
        networkManager.GetComponent<AccountRetriever>().account.GamesWon += 1;
    }

    public void IncrementKills()
    {
        Debug.Log("KILLED CALLED");
        if (networkManager.GetComponent<AccountRetriever>().username == "")
            return;
        networkManager.GetComponent<AccountRetriever>().account.Kills += 1;
        Debug.Log("Kills: " + networkManager.GetComponent<AccountRetriever>().account.Kills);
    }

    public void IncrementDeaths()
    {
        if (networkManager.GetComponent<AccountRetriever>().username == "")
            return;
        networkManager.GetComponent<AccountRetriever>().account.Deaths += 1;
    }

    public void IncrementItemsUsed()
    {
        if (networkManager.GetComponent<AccountRetriever>().username == "")
            return;
        networkManager.GetComponent<AccountRetriever>().account.ItemsUsed += 1;
    }

    public void IncrementPuzzlesCompleted()
    {
        if (networkManager.GetComponent<AccountRetriever>().username == "")
            return;
        networkManager.GetComponent<AccountRetriever>().account.PuzzlesCompleted += 1;
    }

    private void UpdateAccount()
    {
        if (networkManager.GetComponent<AccountRetriever>().username == "")
            return;
        networkManager.GetComponent<AccountRetriever>().updateStats();
    }
}