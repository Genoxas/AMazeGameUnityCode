using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]

/*
 * Author: Christian Reyes
 * Description: Script used in order to set the 'IK' for the player when pick up. Shows the player actually trying to touch the 
 * game object upon pick up
 */

public class IKSystemTutorial : MonoBehaviour
{

  protected Animator animator;
  public bool ikActive = false;
  public Transform leftHandObj;
  public Transform rightHandObj = null;
  public Transform lookObj = null;
  public float leftIkWeight;
  public float leftIkRotWeight = 1;
  void Start()
  {
    animator = GetComponent<Animator>();
  }

  public void SetIKRotWeight(int weight)
  {
    leftIkRotWeight = weight;
  }


  public void setIkActive()
  {
    ikActive = true;
  }
  public void setIkUnactive()
  {
    ikActive = false;
  }

  private void FixedUpdate()
  {
    leftIkWeight = animator.GetFloat("FlashLightIKWeight");
  }

  //a callback for calculating IK
  void OnAnimatorIK()
  {
    if (animator)
    {
      animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftIkWeight);
      animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftIkRotWeight);
      animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
      animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
      //if the IK is active, set the position and rotation directly to the goal. 
      if (ikActive)
      {

        // Set the look target position, if one has been assigned
        if (lookObj != null)
        {
          animator.SetLookAtWeight(1);
          animator.SetLookAtPosition(lookObj.position);
        }

        // Set the right hand target position and rotation, if one has been assigned
        if (rightHandObj != null)
        {
          animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
          //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
          animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
          //animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
        }

      }

      //if the IK is not active, set the position and rotation of the hand and head back to the original position
      else
      {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        animator.SetLookAtWeight(0);
      }
    }
  }
}
