using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes / Joseph Ha / Brian Tat
 * Description: Stats such as health that is tracked by each player to determine whether they are alive or not. Also sets
 * the spawns for each player
 */

public class PlayerStats : NetworkBehaviour
{
    //[SyncVar(hook = "UpdateHealth")]
    [SyncVar]
    public float playerHealth = 100;
    public Vector3 spawnPoint;
    private Animator anim;
    private float counter = 0;
    [SerializeField]
    private PlayerSoundController playerSoundController;
    private GameObject gameManager;

    //Christian Reyes
    void Start()
    {
        StartCoroutine(SetVariables());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            CmdRespawn();
        }
    }

    IEnumerator SetVariables()
    {
        yield return new WaitForEndOfFrame();
        spawnPoint = gameObject.transform.position;
        anim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");
    }

    [Command]
    public void CmdRegisterHit(float damageValue)
    {
        if (playerHealth - damageValue < 0)
        {
            playerHealth = 0;
        }
        else
        {
            playerHealth -= damageValue;
        }
        RpcRegisterHit();
    }

    [ClientRpc]
    public void RpcRegisterHit()
    {
        playerSoundController.PlayHitReaction();
        if (!isLocalPlayer)
            return;
        StartCoroutine(CheckDamageAndUpdateHealth());
    }

    IEnumerator CheckDamageAndUpdateHealth()
    {
        //yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("ReceivedHit", true);
        if (playerHealth <= 0)
        {
            gameManager.GetComponent<GameManager>().IncrementDeaths();
            CmdRespawn();
        }
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        if (!isLocalPlayer)
            return;
        GameObject.Find ("HpBar").GetComponent<Image> ().fillAmount = (playerHealth / 100);
    }

    //Place holder for using hooks on syncvars
    //Christian Reyes 
    //private void UpdateHealth(float currentHealth)
    //{
    //    if (!isLocalPlayer)
    //        return;
    //    GameObject.Find("ScreenUI/HpText").GetComponent<Text>().text = string.Format("Health : {0}%", Mathf.Round(currentHealth));
    //    Debug.Log(playerHealth);
    //}
    //Christian Reyes
    public float GetHealth()
    {
        return playerHealth;
    }

    public bool CheckLocalAuthority()
    {
        if (isLocalPlayer)
        {
            return true;
        } else
        {
            return false;
        }
    }

    [Command]
    public void CmdAddHealth(float addedHealth)
    {
        playerHealth = playerHealth + addedHealth;
        if (playerHealth > 100)
        {
            playerHealth = 100;
        }
        RpcAddHealth(addedHealth);
        UpdateHealth();
    }

    //Brian Tat
    [ClientRpc]
    public void RpcAddHealth(float addedHealth)
    {
        if (isServer)
            return;
        playerHealth = playerHealth + addedHealth;
        if (playerHealth > 100)
        {
            playerHealth = 100;
        }
        UpdateHealth();
    }

    [Command]
    private void CmdRespawn()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Animator>().SetBool("Death", true);
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<PlayerMovement>().PausePlayer();
        StartCoroutine(Respawn());
        RpcRespawn();
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        playerSoundController.PlayDeathReaction();
        if (isServer)
            return;
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Animator>().SetBool("Death", true);
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<PlayerMovement>().PausePlayer();
        StartCoroutine(Respawn());
    }

    //Joseph Ha
    //Reenables the player components after death
    //Respawns the player back to the intial position when they entered the map
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(8);
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Animator>().SetBool("Death", false);
        //spawnPoint = GameObject.Find(gameObject.name + " Spawn").transform.position;
        gameObject.transform.position = spawnPoint;
        this.playerHealth = 100;
        UpdateHealth();
        gameObject.GetComponent<PlayerMovement>().stamina = 100;
        gameObject.GetComponent<PlayerMovement>().UpdateStamina();
        this.GetComponent<CapsuleCollider>().enabled = true;
        this.GetComponent<PlayerMovement>().UnpausePlayer();
    }
    
    public void SetReceivedHitFalse()
    {
        if (!isLocalPlayer)
            return;
        anim.SetBool("ReceivedHit", false);
    }


}
