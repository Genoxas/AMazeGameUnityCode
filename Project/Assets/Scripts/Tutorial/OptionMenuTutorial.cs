using UnityEngine;
using System.Collections;

public class OptionMenuTutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void ExitButtonPressed()
  {
    GameObject.Find("NetworkManager").GetComponent<Test>().offlineCanvas.Show();
    Application.LoadLevel("MainMenuLogin");
  }
}
