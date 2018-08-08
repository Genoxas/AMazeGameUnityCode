using UnityEngine;
using System.Collections;

public class TutorialPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  if (Input.GetKeyDown(KeyCode.Escape))
    gameObject.SetActive(false);
	}

  public void closePanel ()
  {
    gameObject.SetActive(false);
  }

}
