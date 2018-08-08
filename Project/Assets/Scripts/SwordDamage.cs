using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Joseph Ha
 * Description: Calculating actual damage when the sword enters the body of another "player"
 */

public class SwordDamage : NetworkBehaviour
{
    Animator anim;
    [SerializeField]
    private GameObject player;
    public bool doDamage;
    public bool isPickedUp;

    public float swingAnim;
    public float punchAnim;
    private float rightCoolDown = 0.8f;

    // Use this for initialization
    void Start()
    {
        if (anim == null)
        {
            anim = gameObject.GetComponentInParent<Animator>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            swingAnim = anim.GetFloat("Swing");
        }
        else
        {
            punchAnim = anim.GetFloat("Punch");
        }
        if (rightCoolDown >= 0)
        {
            rightCoolDown -= Time.deltaTime;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.GetComponentInParent<PlayerCombat>().hasWeapon == true)
            {
                if (swingAnim > 0 && isPickedUp == true)
                {
                    //Debug.Log(doDamage);
                    //if (doDamage == true)
                    //{
                    //Debug.Log("Collided with player: " + other.gameObject.name);
                    //other.gameObject.GetComponent<PlayerStats>().RpcRegisterHit(20);
                    playSwordImpact();
                    transform.root.GetComponent<PlayerActions>().RegisterHit(other.gameObject, 20);
                    //transform.root.GetComponent<PlayerActions>().CmdRegisterHit(other.gameObject, 20);
                    StartCoroutine(CheckPlayerHealth(other));
                    //Debug.Log(this.gameObject.name);
                    //Debug.Log("Collided with player: " + other.gameObject.name);
                    //}
                }
            }
            else
            {
                if (punchAnim > 0 && rightCoolDown <= 0)
                {
                    //Debug.Log("Collided with player: " + other.gameObject.name);
                    //Debug.Log(this.gameObject.name);

                    //other.gameObject.GetComponent<PlayerStats>().RpcRegisterHit(10);
                    //psc.PlayPunchImpact();
                    rightCoolDown = 0.8f;
                    playImpactSound();
                    transform.root.GetComponent<PlayerActions>().RegisterHit(other.gameObject, 10);
                    //transform.root.GetComponent<PlayerActions>().CmdRegisterHit(other.gameObject, 10);
                    StartCoroutine(CheckPlayerHealth(other));
                    //Debug.Log(this.gameObject.name);
                    //Debug.Log("Collided with player: " + other.gameObject.name);
                }
            }
        }
    }

    private void playImpactSound()
    {
        transform.root.GetComponent<PlayerCombat>().playPunchImpact();
    }

    private void playSwordImpact()
    {
        transform.root.GetComponent<PlayerCombat>().playSwordImpact();
    }

    IEnumerator CheckPlayerHealth(Collider player)
    {
        yield return new WaitForSeconds(0.3f);
        if (player.gameObject.GetComponent<PlayerStats>().GetHealth() == 0 && transform.root.GetComponent<PlayerActions>().isLocalPlayer == true)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().IncrementKills();
        }
    }
}
