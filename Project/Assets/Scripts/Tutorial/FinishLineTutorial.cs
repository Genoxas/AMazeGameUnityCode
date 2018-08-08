using UnityEngine;
using System.Collections;

public class FinishLineTutorial : MonoBehaviour {

  [SerializeField]
  private GameObject panel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void OnTriggerEnter(Collider other)
  {
    panel.SetActive(true);
  }

  public void ExitButtonPressed()
  {
    GameObject.Find("NetworkManager").GetComponent<Test>().offlineCanvas.Show();
    Application.LoadLevel("MainMenuLogin");
  }

}
