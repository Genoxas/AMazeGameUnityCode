using UnityEngine;
using System.Collections;

public class ItemPanelPopups : MonoBehaviour {

  [SerializeField]
  private GameObject panel1;
  [SerializeField]
  private GameObject panel2;
  [SerializeField]


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void NextButtonPressed()
  {
    panel1.SetActive(false);
    panel2.SetActive(true);
  }

}
