using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

/*
 * Author: Christian Reyes / Joseph Ha / Brian Tat
 * Description: Stats such as health that is tracked by each player to determine whether they are alive or not. Also sets
 * the spawns for each player
 */

public class PlayerStatTutorial : MonoBehaviour
{
  //[SyncVar(hook = "UpdateHealth")]
  private float playerHealth = 60;
  public Vector3 spawnPoint;
  private NetworkAnimator anim;

  //Christian Reyes
  void Start()
  {
    spawnPoint = gameObject.transform.position;
    anim = GetComponent<NetworkAnimator>();
    UpdateHealth();
  }

  public void RpcRegisterHit(float damageValue)
  {

    if (playerHealth - damageValue < 0)
    {
      playerHealth = 0;
    }
    else
    {
      playerHealth -= damageValue;
    }
    if (playerHealth == 0)
    {
      this.GetComponent<Animator>().SetBool("Death", true);
      this.GetComponent<CapsuleCollider>().enabled = false;
      this.GetComponent<PlayerMovement>().PausePlayer();
      StartCoroutine(Respawn());
    }
    UpdateHealth();
  }

  private void UpdateHealth()
  {
    //anim.SetTrigger("ReceivedHit");
    GameObject.Find("HpBar").GetComponent<Image>().fillAmount = (playerHealth / 100);
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

  public void CmdAddHealth(float addedHealth)
  {
    playerHealth = playerHealth + addedHealth;
    if (playerHealth > 100)
    {
      playerHealth = 100;
    }
    UpdateHealth();
  }

  //Joseph Ha
  //Reenables the player components after death
  //Respawns the player back to the intial position when they entered the map
  IEnumerator Respawn()
  {
    GameObject.Find("FinishLine").GetComponent<BoxCollider>().enabled = false;
    yield return new WaitForSeconds(7);
    this.GetComponent<Animator>().SetBool("Death", false);
    //spawnPoint = GameObject.Find(gameObject.name + " Spawn").transform.position;
    gameObject.transform.position = spawnPoint;
    this.playerHealth = 100;
    UpdateHealth();
    gameObject.GetComponent<PlayerMovement>().stamina = 100;
    this.GetComponent<CapsuleCollider>().enabled = true;
    this.GetComponent<PlayerMovement>().UnpausePlayer();
    yield return new WaitForSeconds(3);
    GameObject.Find("FinishLine").GetComponent<BoxCollider>().enabled = true;
  }
}
