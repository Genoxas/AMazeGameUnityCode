using UnityEngine;
using System.Collections;

/*
 * Author: Joseph Ha
 * Description: Calculating actual damage when the sword enters the body of another "player"
 */

public class LeftHandDamage : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    private GameObject player;
    private PlayerCombat pc;
    public bool doDamage;
    public bool isPickedUp;

    public float swingAnim;
    public float leftPunchAnim;
    private float leftCoolDown = 0.8f;

    // Use this for initialization
    void Start()
    {
        if (anim == null)
        {
            anim = gameObject.GetComponentInParent<Animator>();
        }
        if (pc == null)
        {
            pc = player.GetComponent<PlayerCombat>();
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
            leftPunchAnim = anim.GetFloat("LeftPunch");
        }
        if (leftCoolDown >= 0)
        {
            leftCoolDown -= Time.deltaTime;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (leftPunchAnim > 0 && leftCoolDown <= 0)
            {
                leftCoolDown = 0.8f;
                playImpactSound();
                transform.root.GetComponent<PlayerActions>().RegisterHit(other.gameObject, 10);
                StartCoroutine(CheckPlayerHealth(other));
            }
        }
    }

    public void playImpactSound()
    {
        pc.playPunchImpact();
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
