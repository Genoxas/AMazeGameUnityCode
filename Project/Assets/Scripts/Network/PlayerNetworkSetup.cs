using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Joseph Ha
 * Description: Upon spawning into the game we enable the components of a player that belongs to the local player
 */

public class PlayerNetworkSetup : NetworkBehaviour
{

    [SerializeField]
    Camera camera;
    private GameObject gameManager;
    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            //Enable components if is local player
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<PlayerInventory>().enabled = true;
            GetComponent<PlayerStats>().enabled = true;
            GetComponent<PlayerCombat>().enabled = true;
            GetComponent<AudioListener>().enabled = true;
            camera.enabled = true;
            camera.transform.parent = null;
        }
        StartCoroutine(EnablePlayerSnycPos());
    }

    IEnumerator EnablePlayerSnycPos()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<PlayerSyncPosition>().enabled = true;
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(3, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(4, true);
    }

    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(3, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(4, true);
    }
}
