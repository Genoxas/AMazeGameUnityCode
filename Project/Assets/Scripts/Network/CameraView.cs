using UnityEngine;
using System.Collections;

/*
 * Author: Joseph Ha (old)
 * Description: Not used
 */

public class CameraView : MonoBehaviour {

	// Use this for initialization
	void Start () {

        NetworkView nView = GetComponent<NetworkView>();

        
        if(nView.isMine)
        {
            Debug.Log("Hekki");

            GetComponent<Camera>().enabled = true;
            
        }
        else
        {
            GetComponent<Camera>().enabled =  false;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
