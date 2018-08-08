using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/*
 * Author: Brian Tat (Old)
 */

public class Puzzle2 : MonoBehaviour {



    private int[] randomnumbers = new int[5];
    private int[] playerSelectedNumbers = new int[5];
    private int playerSelectedIndex;
    private int round;
    public GameObject puzzleWall;
    public GameObject puzzleDialog;
    public GameObject puzzleText;   
    private bool IsCompleted;
	private bool playerDoingPuzzle;
    private float timeLimit;
    private Button[] buttons;
    private Text[] texts;
	private GameObject player;

    void Awake()
    {
      IsCompleted = false;

      randomnumbers = new int[5];
      playerSelectedNumbers = new int[5];
      playerSelectedIndex = 10;
      buttons = puzzleDialog.GetComponentsInChildren<Button>();
      texts = puzzleDialog.GetComponentsInChildren<Text>();
      round = 1;
    }

	void OnEnable()
	{
		Debug.Log ("Enabled");
	}


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        /*
        //goes through the array of players and check their positions if they are close to the puzzle.
        foreach (GameObject player in playerArray)
        {
          Debug.Log("Working 1");
            var distance = Vector3.Distance(player.transform.position, puzzleWall.transform.position);

            //when the player is close enough to the puzzle and it is not completed let player start the puzzle.
            if (distance < 1.5 && IsCompleted == false)
            {
                Debug.Log("Working 2");
                //when player is close enough display text "Start Puzzle"
                puzzleText.SetActive(true);

                //if player presses E start the puzzle
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //get the player that is starting the puzzle
                    playerDoingPuzzle = player;

                    //show puzzle Dialog to user
                    showPuzzleDialog();

                    //start puzzle method to process the puzzle
                    startPuzzle();
                }
            }
            else
            {
                //hide puzzle 
                puzzleText.SetActive(false);
                puzzleDialog.SetActive(false);
            }

        }
         * */
    }





  public void OnCollisionEnter(Collision slot)
  {
	player = slot.gameObject;
    //puzzleDialog.SetActive(true);
	if (!IsCompleted)
    { 
		//if (isLocalPlayer)
	//	{
			puzzleText.SetActive(true);
		//}
    }
  }

  public void OnCollisionStay (Collision slot)
  {
    if (!IsCompleted || !playerDoingPuzzle) 
		{ 	
		//	if (isLocalPlayer) 
		//	{
				if (Input.GetKeyDown (KeyCode.E)) 
				{
					
					//playerDoingPuzzle = true;
					//show puzzle Dialog to user
					//showPuzzleDialog ();

					//start puzzle method to process the puzzle
					//startPuzzle ();
				}
			//}
		}
  }

  public void OnCollisionExit(Collision slot)
  {
		//if (isLocalPlayer) 
		//{
	    puzzleText.SetActive(false);
	    puzzleDialog.SetActive(false);
		//}
   
  }

    //show puzzle dialog with the 9 buttons
    public void showPuzzleDialog()
    {
        puzzleDialog.SetActive(true);
    }

    //method called when player starts the puzzle
    public void startPuzzle()
    {

      IsCompleted = false;

      randomnumbers = new int[5];
      playerSelectedNumbers = new int[5];
      playerSelectedIndex = 10;
      buttons = puzzleDialog.GetComponentsInChildren<Button>();
      texts = puzzleDialog.GetComponentsInChildren<Text>();
      round = 1;

      Text title = texts[0];
      title.text = "Round : " + round + " - Repeat the sequence";
      playerDoingPuzzle = true;

       StartCoroutine(Pause(round));
        //calls method to create a delay
        playerSelectedIndex = 0;
        //EndPuzzle();
    }

    //method to end puzzle and restart values
    private void endPuzzle()
    {
        playerDoingPuzzle = false;
        round = 0;
        puzzleDialog.SetActive(false);
    }

    //method used make a delay
    IEnumerator Pause(int round)
    {
        for (int i = 0; i < round; i++)
        {
            int generatedNumber = Random.Range(1, 9);
            //Debug.Log("Number Generated: " + generatedNumber);
            randomnumbers[i] = (generatedNumber + 1);
            //Debug.Log("selectedButton: " + (generatedNumber + 1));
            buttons[generatedNumber].image.color = Color.red;
            yield return new WaitForSeconds(1);
            //Debug.Log("Out of Pause");
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
        if (round > 5)
        {
            IsCompleted = true;
            while (transform.position.x >= -33.0)
            {
                transform.Translate(Vector3.left * Time.deltaTime);
            }
            endPuzzle();
        }
        else
        {
            Text title = texts[0];
            title.text = "Round : " + round + " - Repeat the sequence";
            playerSelectedIndex = 0;
            StartCoroutine(Pause(round));
        }
    }
    

    //Complete the puzzle and remove the item from player inventory
    public void completePuzzle()
    {
        while (transform.position.x >= -33.0)
        {
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        IsCompleted = true;
        puzzleText.SetActive(false);
        puzzleDialog.SetActive(false);
    }

    //methods below are for when buttons are pressed
    public void Button1Pressed ()
    {
        if (playerSelectedIndex < round)
        {
            playerSelectedNumbers[playerSelectedIndex] = 1;
            playerSelectedIndex++;
            if (playerSelectedIndex == round)
            {
                processPuzzle();
            }
        }
    }

    public void Button2Pressed()
    {
        if (playerSelectedIndex < round)
        {
            playerSelectedNumbers[playerSelectedIndex] = 2;
            playerSelectedIndex++;
            if (playerSelectedIndex == round)
            {
                processPuzzle();
            }
        }
    }

    public void Button3Pressed()
    {
        if (playerSelectedIndex < round)
        {
            playerSelectedNumbers[playerSelectedIndex] = 3;
            playerSelectedIndex++;
            if (playerSelectedIndex == round)
            {
                processPuzzle();
            }
        }
    }

    public void Button4Pressed()
    {
        if (playerSelectedIndex < round)
        {
            playerSelectedNumbers[playerSelectedIndex] = 4;
            playerSelectedIndex++;
            if (playerSelectedIndex == round)
            {
                processPuzzle();
            }
        }
    }

    public void Button5Pressed()
    {
        if (playerSelectedIndex < round)
        {
            playerSelectedNumbers[playerSelectedIndex] = 5;
            playerSelectedIndex++;
            if (playerSelectedIndex == round)
            {
                processPuzzle();
            }
        }
    }

    public void Button6Pressed()
    {
        if (playerSelectedIndex < round)
        {
            playerSelectedNumbers[playerSelectedIndex] = 6;
            playerSelectedIndex++;
            if (playerSelectedIndex == round)
            {
                processPuzzle();
            }
        }
    }

    public void Button7Pressed()
    {
        if (playerSelectedIndex < round)
        {
            playerSelectedNumbers[playerSelectedIndex] = 7;
            playerSelectedIndex++;
            if (playerSelectedIndex == round)
            {
                processPuzzle();
            }
        }
    }

    public void Button8Pressed()
    {
        if (playerSelectedIndex < round)
        {
            playerSelectedNumbers[playerSelectedIndex] = 8;
            playerSelectedIndex++;
            if (playerSelectedIndex == round)
            {
                processPuzzle();
            }
        }
    }

    public void Button9Pressed()
    {
        if (playerSelectedIndex < round)
        {
            playerSelectedNumbers[playerSelectedIndex] = 9;
            playerSelectedIndex++;
            if (playerSelectedIndex == round)
            {
                processPuzzle();
            }
        }
    }

}
