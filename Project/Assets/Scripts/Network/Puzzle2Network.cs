using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * Author: Brian Tat
 * Description: Script for the Logic of Puzzle2 - (Sequence Puzzle)
 */

public class Puzzle2Network : NetworkBehaviour {

	private int[] randomnumbers;
	private int[] playerSelectedNumbers;
	private int playerSelectedIndex;
	private int sequenceLength;
	private int round;
	private int roundToComplete;
	public GameObject puzzleWall;
	public GameObject puzzleDialog;
	public GameObject puzzleText;
	private GameObject player;
	public bool ActualPlayerDoingPuzzle = false;
	[SyncVar]
	public bool IsCompleted;
	[SyncVar]
	public  bool playerDoingPuzzle;
	[SyncVar]
	public Vector3 syncPos;


	//private float timeLimit;
	private Button[] buttons;
	private Text[] texts;

	void Awake()
	{

	}

	// Use this for initialization
	void Start () 
  {
    IsCompleted = false;
    roundToComplete = Random.Range(1, 4);
    buttons = puzzleDialog.GetComponentsInChildren<Button>();
    texts = puzzleDialog.GetComponentsInChildren<Text>();
    round = 1;
		playerSelectedIndex = 0;
	}

	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(KeyCode.Q))
    {
      string playerID = GameObject.Find("NetworkManager").GetComponent<Test>().playerNetworkID;
      if (ActualPlayerDoingPuzzle)
      {
        if (player.name.Equals(playerID))
        {
          puzzleText.SetActive(false);
          puzzleDialog.SetActive(false);
          player.gameObject.GetComponent<PlayerActions>().CmdEndPuzzle(this.gameObject);
          endPuzzle();
        }
      }
    }

	}
	
	//When player enters the parameter of the puzzle wall the "Start Puzzle" message will pop up if a player isn't doing the puzzle already
	public void OnCollisionEnter(Collision slot)
	{
		player = slot.gameObject;
		string playerID = GameObject.Find ("NetworkManager").GetComponent<Test> ().playerNetworkID;
		Debug.Log (player.name + " Enter");
		if (player.name.Equals (playerID)) 
		{
			if (!IsCompleted && !playerDoingPuzzle) 
			{ 
				puzzleText.SetActive (true);
			}
		}
	}

	//When player hits the E key with the "Start Puzzle" message up will initate the puzzle
	public void OnCollisionStay (Collision slot)
	{
		player = slot.gameObject;
		string playerID = GameObject.Find ("NetworkManager").GetComponent<Test> ().playerNetworkID;
		if (player.name.Equals (playerID))  {
            if (Input.GetKeyDown(KeyCode.P))
            {
                completePuzzle();
            }
			if (!IsCompleted && !playerDoingPuzzle) 
			{ 	
				if (Input.GetKeyDown (KeyCode.E)) 
				{
					ActualPlayerDoingPuzzle = true;
					playerDoingPuzzle = true;
					player.gameObject.GetComponent<PlayerActions> ().CmdStartPuzzle (this.gameObject);
					//completePuzzle();

					//show puzzle Dialog to user
					showPuzzleDialog ();
				
					//start puzzle method to process the puzzle
					startPuzzle ();
				}
			}
		}
	}

	//When player leaves the puzzles parameter hides Puzzle Dialog and Message from player UI
	public void OnCollisionExit(Collision slot)
	{
		StopCoroutine ("Pause");
		player = slot.gameObject;
		string playerID = GameObject.Find ("NetworkManager").GetComponent<Test> ().playerNetworkID;
		if (ActualPlayerDoingPuzzle) 
		{	
			if (player.name.Equals (playerID)) 
			{
			puzzleText.SetActive(false);
			puzzleDialog.SetActive(false);
			player.gameObject.GetComponent<PlayerActions> ().CmdEndPuzzle(this.gameObject);
			endPuzzle ();
			}
		}
		if (player.name.Equals (playerID)) 
		{
			puzzleText.SetActive (false);
		}
	}
	
	//show puzzle dialog with the 9 buttons
	public void showPuzzleDialog()
	{
		puzzleDialog.SetActive(true);
	}
	
	//method called when player starts the puzzle
	private void startPuzzle()
	{
		
		IsCompleted = false;
		
    	sequenceLength = Random.Range(5, 8);
    	randomnumbers = new int[sequenceLength];
    	playerSelectedNumbers = new int[sequenceLength];
		buttons = puzzleDialog.GetComponentsInChildren<Button>();
		texts = puzzleDialog.GetComponentsInChildren<Text>();
		round = 1;

		Text title = texts[0];
		title.text = "Repeat the sequence";
		playerDoingPuzzle = true;
		
		StartCoroutine("Pause");
		//calls method to create a delay
		playerSelectedIndex = 0;
		//EndPuzzle();
	}
	
	//method to end puzzle and restart values
	private void endPuzzle()
	{
		ActualPlayerDoingPuzzle = false;
		playerDoingPuzzle = false;
		round = 0;
		puzzleDialog.SetActive(false);
		StopCoroutine ("Pause");
		foreach (Button crtButton in buttons) 
		{
			crtButton.image.color = Color.white;
		}
	}
	
	//method used make a delay
	IEnumerator Pause()
	{
		for (int i = 0; i < sequenceLength; i++)
		{
			int generatedNumber = Random.Range(0, 9);
			randomnumbers[i] = (generatedNumber + 1);
			buttons[generatedNumber].image.color = Color.red;
			yield return new WaitForSeconds(1);
			buttons[generatedNumber].image.color = Color.white;
			yield return new WaitForSeconds(1);
		}

	}
	
	//process Puzzle checks if players answers are correct and advances puzzle to the next round or end it if they completed the puzzle
	private void processPuzzle()
	{
		for (int i = 0; i < round; i++)
		{
			if (playerSelectedNumbers[i] != randomnumbers[i])
			{
				endPuzzle();
			}
		}
		round++;
		if (round > roundToComplete)
		{
			completePuzzle();
		}
		else
		{
			Text title = texts[0];
			title.text = "Round : " + round + " - Repeat the sequence";
			playerSelectedIndex = 0;
			sequenceLength = Random.Range(5, 8);
			randomnumbers = new int[sequenceLength];
    		playerSelectedNumbers = new int[sequenceLength];
			StartCoroutine("Pause");
		}
	}

	//method for when the puzzle is completed
	private void completePuzzle()
	{
		IsCompleted = true;
		player.gameObject.GetComponent<PlayerActions> ().CmdUpdatePuzzleWall (this.gameObject);
	}


	//methods below are for when buttons are pressed they are attached to buttonlisteners in the Puzzle UI and will be called when the correponding buttons are pressed
	public void Button1Pressed ()
	{
		if (playerSelectedIndex < sequenceLength)
		{
			playerSelectedNumbers[playerSelectedIndex] = 1;
			playerSelectedIndex++;
			if (playerSelectedIndex == sequenceLength)
			{
				processPuzzle();
			}
		}
	}
	
	public void Button2Pressed()
	{
		if (playerSelectedIndex < sequenceLength)
		{
			playerSelectedNumbers[playerSelectedIndex] = 2;
			playerSelectedIndex++;
			if (playerSelectedIndex == sequenceLength)
			{
				processPuzzle();
			}
		}
	}
	
	public void Button3Pressed()
	{
		if (playerSelectedIndex < sequenceLength)
		{
			playerSelectedNumbers[playerSelectedIndex] = 3;
			playerSelectedIndex++;
			if (playerSelectedIndex == sequenceLength)
			{
				processPuzzle();
			}
		}
	}
	
	public void Button4Pressed()
	{
		if (playerSelectedIndex < sequenceLength)
		{
			playerSelectedNumbers[playerSelectedIndex] = 4;
			playerSelectedIndex++;
			if (playerSelectedIndex == sequenceLength)
			{
				processPuzzle();
			}
		}
	}
	
	public void Button5Pressed()
	{
		if (playerSelectedIndex < sequenceLength)
		{
			playerSelectedNumbers[playerSelectedIndex] = 5;
			playerSelectedIndex++;
			if (playerSelectedIndex == sequenceLength)
			{
				processPuzzle();
			}
		}
	}
	
	public void Button6Pressed()
	{
		if (playerSelectedIndex < sequenceLength)
		{
			playerSelectedNumbers[playerSelectedIndex] = 6;
			playerSelectedIndex++;
			if (playerSelectedIndex == sequenceLength)
			{
				processPuzzle();
			}
		}
	}
	
	public void Button7Pressed()
	{
		if (playerSelectedIndex < sequenceLength)
		{
			playerSelectedNumbers[playerSelectedIndex] = 7;
			playerSelectedIndex++;
			if (playerSelectedIndex == sequenceLength)
			{
				processPuzzle();
			}
		}
	}
	
	public void Button8Pressed()
	{
		if (playerSelectedIndex < sequenceLength)
		{
			playerSelectedNumbers[playerSelectedIndex] = 8;
			playerSelectedIndex++;
			if (playerSelectedIndex == sequenceLength)
			{
				processPuzzle();
			}
		}
	}
	
	public void Button9Pressed()
	{
		if (playerSelectedIndex < sequenceLength)
		{
			playerSelectedNumbers[playerSelectedIndex] = 9;
			playerSelectedIndex++;
			if (playerSelectedIndex == sequenceLength)
			{
				processPuzzle();
			}
		}
	}


}