using UnityEngine;
using System.Collections;

/*
 * Author: Christian Reyes
 * Description: Script used by the 'finish line' trigger that sends the game manager an update, 
 * stating that the game is finished.
 */

public class FinishLine : MonoBehaviour {

    [SerializeField]
    private GameObject gameManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameManager.GetComponent<GameManager>().CmdFinishLine(col);
        }
    }
}
