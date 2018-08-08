using UnityEngine;
using System.Collections;

/*
 * Author: Joseph Ha
 * Description: After displaying the logo, load the main menu scene after a couple of seconds
 */

public class LogoLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Debug.Log("hi");
        StartCoroutine("CountDown");

        //Debug.Log("asdf");
	}

    private IEnumerator CountDown()
    {
        //Debug.Log("Goodbye");
        yield return new WaitForSeconds(5);
        Application.LoadLevel(1);
    }
}
