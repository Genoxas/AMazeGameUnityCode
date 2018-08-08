using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*
 * Author: Joseph Ha
 * Description: All player actions that deal with combat with either a sword or their fists
 */

public class PlayerCombatTutorial : MonoBehaviour
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
  }
  void WeaponInHand()
  {
    hasWeapon = true;
  }

  void Attacking()
  {
    if (anim)
    {
      if (hasWeapon != false)
      {
        //sword.GetComponent<SwordDamage>().doDamage = true;
        if (Input.GetButtonDown("Attack"))
        {
          //StartCoroutine(Swing());
          anim.SetBool("SwordAttack", true);
        }
      }
      else
      {
        if (Input.GetButtonDown("Attack"))
        {
          //StartCoroutine(Punch());
          anim.SetBool("NormalAttack", true);
        }
      }
    }

  }

  IEnumerator Punch()
  {
    anim.SetBool("NormalAttack", true);
    yield return new WaitForSeconds(0.5f);
    anim.SetBool("NormalAttack", false);
  }

  IEnumerator Swing()
  {
    anim.SetBool("SwordAttack", true);
    yield return new WaitForSeconds(1);
    anim.SetBool("SwordAttack", false);
  }

  public void StopPunch()
  {
    anim.SetBool("NormalAttack", false);
  }

  public void StopSwing()
  {
    anim.SetBool("SwordAttack", false);
  }

  public void playSwordSwingEffect()
  {
    igm.playSwordSwingEffect();
  }

  public void playPunchEffect()
  {
    igm.playPunchEffect();
  }

}
