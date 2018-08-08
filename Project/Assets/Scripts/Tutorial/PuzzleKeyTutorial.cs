using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

/*
 * Author: Brian Tat
 * Description: Script for the Logic of Puzzle Key (Item needed to complete Puzzle)
 */

public class PuzzleKeyTutorial: MonoBehaviour
{

  public GameObject puzzleWall;
  public GameObject puzzleDialog;
  public GameObject puzzleText;
  private GameObject player;
  public string itemToCompletePuzzle;
  private int itemIndex;
  public bool ActualPlayerDoingPuzzle = false;
  public bool IsCompleted;
  public bool playerDoingPuzzle;
  public Vector3 syncPos;

  // Use this for initialization
  void Start()
  {
    IsCompleted = false;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void showPuzzleDialog()
  {
    puzzleDialog.SetActive(true);
  }
  //method to end puzzle and restart values
  private void endPuzzle()
  {
    ActualPlayerDoingPuzzle = false;
    playerDoingPuzzle = false;
    puzzleDialog.SetActive(false);
  }

  //Method to start puzzle 
  private void startPuzzle()
  {
    Text[] texts = puzzleDialog.GetComponentsInChildren<Text>();
    Text title = texts[0];
    Text content = texts[1];
    Text button = texts[2];

    title.text = "Puzzle - Find the Key";
    content.text = "Traveler, find the key to unlock the door.";
    button.text = "Find Key";

    Button puzzleButton = puzzleDialog.GetComponentInChildren<Button>();


    //Get players items in their inventory
    ItemTutorial[] playerItems = player.GetComponent<PlayerInventoryTutorial>().getItemList();

    //check if the player has the item in their inventory 
    bool playerHasItem = false;
    itemIndex = 0;
    foreach (ItemTutorial item in playerItems)
    {
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
    else
    {
      puzzleButton.interactable = false;
    }


  }

  //method called when player hits the give key button in the UI completes the puzzle and removes the key object from game
  public void completePuzzle()
  {
    player.GetComponent<PlayerInventoryTutorial>().removeItem(itemIndex);
    IsCompleted = true;
    while (this.transform.position.y >= -2)
    {
      this.transform.Translate(Vector3.down * Time.deltaTime);
    }
    foreach (Transform child in player.transform.FindChild("Alpha:Hips/Alpha:Spine/Alpha:Spine1/Alpha:Spine2/Alpha:RightShoulder/Alpha:RightArm/Alpha:RightForeArm/Alpha:RightHand"))
    {
      if (child.name.Contains("Item"))
      {
        if (child.GetComponent<ItemTutorial>().itemName.Equals(itemToCompletePuzzle))
        {
          Destroy(child.gameObject);
          break;
        }
      }
    }
  }

  //When player enters the parameter of the puzzle wall the "Start Puzzle" message will pop up if a player isn't doing the puzzle already
  public void OnCollisionEnter(Collision slot)
  {
    player = slot.gameObject;
      if (!IsCompleted && !playerDoingPuzzle)
      {
        puzzleText.SetActive(true);
      }
  }

  //When player hits the E key with the "Start Puzzle" message up will initate the puzzle
  public void OnCollisionStay(Collision slot)
  {
    player = slot.gameObject;
      if (!IsCompleted && !playerDoingPuzzle)
      {
        if (Input.GetKeyDown(KeyCode.E))
        {
          ActualPlayerDoingPuzzle = true;
          playerDoingPuzzle = true;

          //show puzzle Dialog to user
          showPuzzleDialog();

          //start puzzle method to process the puzzle
          startPuzzle();
        }
      }
  }

  //When player leaves the puzzles parameter hides Puzzle Dialog and Message from player UI
  public void OnCollisionExit(Collision slot)
  {
    player = slot.gameObject;
    if (ActualPlayerDoingPuzzle)
    {
        puzzleText.SetActive(false);
        puzzleDialog.SetActive(false);
        endPuzzle();
    }
      puzzleText.SetActive(false);
  }



}
