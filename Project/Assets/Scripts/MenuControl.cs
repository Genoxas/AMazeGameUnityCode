using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MenuControl : MonoBehaviour
{
    Test nm;
    public GameObject offlineCanvas;
    public GameObject lobbyCanvas;

    void Start()
    {
        if(nm == null)
        {
            nm = GameObject.Find("NetworkManager").GetComponent<Test>();
        }
        offlineCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);
    }

    public void onHostClick()
    {
        offlineCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);
    }

    public void onClientClick()
    {
        nm.StartClient();
    }

    public void onServerClick()
    {
        nm.StartServer();
    }
}

