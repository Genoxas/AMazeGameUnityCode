using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EmoteSystem : NetworkBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
            return;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;
	    if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayEmote(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            PlayEmote(1);
        }
	}

    private void PlayEmote(int chosenEmote)
    {
        anim.SetInteger("EmoteChosen", chosenEmote);
        anim.SetBool("Emote", true);
    }

    private void StopEmote()
    {
        if (!isLocalPlayer)
            return;
        anim.SetBool("Emote", false);
    }
}
