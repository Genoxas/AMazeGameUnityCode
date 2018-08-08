using UnityEngine;
using System.Collections;

public class MainMenuSoundManager : MonoBehaviour {

	public AudioSource backgroundMusic;
	public static MainMenuSoundManager instance = null;

	// Use this for initialization
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle(AudioClip clip)
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
