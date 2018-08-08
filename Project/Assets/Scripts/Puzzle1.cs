using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/*
    Description: Item Puzzle (Old)

    This puzzle is for finding out if a player has a registered item in their inventory.
    They would need to give up the item to complete the puzzle
*/

public class Puzzle1 : MonoBehaviour
{

    public GameObject puzzleWall;
    private GameObject[] playerArray;
    public GameObject puzzleText;
    public GameObject puzzleDialog;
    private bool IsCompleted;
    private GameObject playerDoingPuzzle;
    private int itemIndex;
    private bool playerClose;

    // Use this for initialization
    void Start()
    {
        //initialize player array to get all player in the game.
        if (playerArray == null)
        {
            playerArray = GameObject.FindGameObjectsWithTag("Player");
        }
        IsCompleted = false;
        playerClose = false;

    }

    // Update is called once per frame
    void Update()
    {
            //goes through the array of players and check their positions if they are close to the puzzle.
            foreach (GameObject player in playerArray)
            {

                var distance = Vector3.Distance(player.transform.position, puzzleWall.transform.position);

                //when the player is close enough to the puzzle and it is not completed let player start the puzzle.
                if (distance < 1.5 && IsCompleted == false)
                {
                    playerClose = true;
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
                    //puzzleText.SetActive(false);
                    //puzzleDialog.SetActive(false);
                }

            }
        

    }

    //display puzzle window box on screen 
    public void showPuzzleDialog()
    {
        puzzleDialog.SetActive(true);
    }

    public void startPuzzle()
    {
        Text [] texts = puzzleDialog.GetComponentsInChildren<Text>();
        Text title = texts[0];
        Text content = texts[1];
        Text button = texts[2];

        title.text = "Puzzle - Find the Key";
        content.text = "Traveler, find the key to unlock the door.";
        button.text = "Find Key";

        Button puzzleButton = puzzleDialog.GetComponentInChildren<Button>();


        //Get players items in their inventory
        Item[] playerItems = playerDoingPuzzle.GetComponent<PlayerInventory>().getItemList();
        string itemToCompletePuzzle = "Key";

        //check if the player has the item in their inventory 
        bool playerHasItem = false;
        itemIndex = 0;
        foreach (Item item in playerItems)
        {
          //  Debug.Log(item);
            if (item != null)
            {
                //if item is in inventory change condition to true and remove item from inventory
                if (itemToCompletePuzzle.Equals(item.itemName))
                {
                    playerHasItem = true;
                    button.text = "Give Key";
                    break;
                }
            }
            itemIndex++;
        }

        //if player has item enable button for user to complete puzzle
        if (playerHasItem)
        {
            puzzleButton.interactable = true;
        }
        

    }

    //Complete the puzzle and remove the item from player inventory
    public void completePuzzle()
    {
        playerDoingPuzzle.GetComponent<PlayerInventory>().removeItem(itemIndex);

        while (transform.position.x >= -33.0)
        {
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        IsCompleted = true;
        puzzleText.SetActive(false);
        puzzleDialog.SetActive(false);
    }
}

