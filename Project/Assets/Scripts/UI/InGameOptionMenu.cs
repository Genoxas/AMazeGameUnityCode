using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InGameOptionMenu : MonoBehaviour {

	[SerializeField]
	private GameObject SoundManager;
	[SerializeField]
	private GameObject MusicVolumeBar;
	[SerializeField]
	private GameObject SoundEffectBar;
	[SerializeField]
	private GameObject LightingBar;
	// Use this for initialization
	void Start () {
		//GameObject.Find ("SoundManager");
		//MusicVolumeBar.GetComponent<Scrollbar> ().value = (SoundManager.GetComponent<InGameSoundManager> ().backgroundMusic.volume * 2);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Awake(){

	}

	public void CloseButtonPressed()
	{
		GameObject.Find("NetworkManager").GetComponent<Test>().exitToLobbyCanvas.Hide();
	}

	public void MusicVolumeSliderChanged()
	{
		SoundManager.GetComponent<InGameSoundManager> ().backgroundMusic.volume = (MusicVolumeBar.GetComponent<Scrollbar> ().value / 2);
	}

	public void LightingSliderChanged()
	{
		RenderSettings.fogDensity = ((LightingBar.GetComponent<Scrollbar> ().value / 10) + 0.2f);
	}

	public void setSoundManager (GameObject sm)
	{
		SoundManager = sm;
		MusicVolumeBar.GetComponent<Scrollbar> ().value = (SoundManager.GetComponent<InGameSoundManager> ().backgroundMusic.volume * 2);
		LightingBar.GetComponent<Scrollbar> ().value = ((RenderSettings.fogDensity - 0.2f) * 10);
	}


}
