using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Joseph Ha
 * Description: All player actions that deal with combat with either a sword or their fists
 */

public class PlayerCombat : NetworkBehaviour
{

    public Animator anim;
    public bool hasWeapon;
    public GameObject sword;
    public InGameSoundManager igm;

    // Use this for initialization
    void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        if (igm == null)
        {
            igm = GameObject.Find("SoundManager").GetComponent<InGameSoundManager>();
        }
        //WeaponInHand();
    }
    // Update is called once per frame
    void Update()
    {
        Attacking();
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            StopSwing();
        }
    }
    void WeaponInHand()
    {
        hasWeapon = true;
    }

    void Attacking()
    {
        if (!isLocalPlayer)
            return;
        if (anim)
        {
            int num = Random.Range(0, 2);
            if (hasWeapon != false)
            {
                if (Input.GetButtonDown("Attack"))
                {
                    anim.SetInteger("AttackMixer", num);
                    anim.SetBool("SwordAttack", true);
                }
            }
            else
            {
                if (Input.GetButtonDown("Attack"))
                {
                    anim.SetInteger("AttackMixer", num);
                    anim.SetBool("NormalAttack", true);
                }
            }
        }
    }

    public void StopPunch()
    {
        if (!isLocalPlayer)
            return;
        anim.SetBool("NormalAttack", false);
    }

    public void StopSwing()
    {
        if (!isLocalPlayer)
            return;
        anim.SetBool("SwordAttack", false);
    }

    public void playSwordSwingEffect()
    {
        if (!isLocalPlayer)
            return;
        CmdSendServerSound("PlaySwing");
    }

    public void playPunchEffect()
    {
        if (!isLocalPlayer)
            return;
        CmdSendServerSound("PlayPunch");
    }

    public void playPunchImpact()
    {
        if (!isLocalPlayer)
            return;
        CmdSendServerSound("PlayPunchImpact");
    }

    public void playSwordImpact()
    {
        if (!isLocalPlayer)
            return;
        CmdSendServerSound("PlaySwordImpact");
    }

    [Command]
    void CmdSendServerSound(string chosenAttackSound)
    {
        SendMessage(chosenAttackSound);
        RpcSendSoundToClient(chosenAttackSound);
    }

    [ClientRpc]
    void RpcSendSoundToClient(string chosenAttackSound)
    {
        if (isServer)
            return;
        SendMessage(chosenAttackSound);
    }

}
