using UnityEngine;
using System.Collections;

public class ItemLineTutorial : MonoBehaviour {

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
    gameObject.SetActive(false);

  }

}
