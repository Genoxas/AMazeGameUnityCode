using UnityEngine;
using System.Collections;

/*
 * Author: Christian Reyes 
 * Description: Controls camera for the player
 */

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform cameraLerpPos;
    [SerializeField]
    private Transform playerCameraLookPos;
    public float smoothingRatePos = 2.7f;
    public float smoothingRateRot = 6f;
    private float maxDistance;
    private float raycastDistance;
    private RaycastHit raycastPoint;

    //Set the rotation for the raycast from the cameraLookAtPos gameobject to the cameraLerpPos gameobject (which is behind the player
    void Start()
    {
        maxDistance = Vector3.Distance(playerCameraLookPos.position, cameraLerpPos.position);
        Vector3 cameraLerpPosLookPos = playerCameraLookPos.position - cameraLerpPos.position;
        playerCameraLookPos.transform.rotation = Quaternion.LookRotation(cameraLerpPosLookPos, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        SmoothSetPos();
        SmoothLookAtRotation();
    }

    //Set the position of the camera behind the player without going through walls
    private void SmoothSetPos()
    {
        if (Physics.Raycast(playerCameraLookPos.position, playerCameraLookPos.transform.forward * -1, out raycastPoint, 3))
        {
            Debug.DrawRay(playerCameraLookPos.position, playerCameraLookPos.forward * -1, Color.green);
            raycastDistance = Vector3.Distance(playerCameraLookPos.position, raycastPoint.point);
            if (raycastDistance < maxDistance)
            {
                transform.position = Vector3.Lerp(this.transform.position, raycastPoint.point, smoothingRatePos * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(this.transform.position, cameraLerpPos.position, smoothingRatePos * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(this.transform.position, cameraLerpPos.position, smoothingRatePos * Time.deltaTime);
        }
    }

    //Set the rotation of the camera to always look at the player
    private void SmoothLookAtRotation()
    {
        Vector3 relativePlayerPosition = playerCameraLookPos.transform.position - transform.position;
        Quaternion lookAt = Quaternion.LookRotation(relativePlayerPosition, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, smoothingRateRot * Time.deltaTime);
    }
}
