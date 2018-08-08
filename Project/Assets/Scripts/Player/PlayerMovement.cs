using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    /*
     * Author: Christian Reyes
     * Description: Script used to control character movements
     * 
     */

    Animator anim;
    [SerializeField]
    private GameObject flashLight;
    [SerializeField]
    private GameObject pointLight;
    private GameObject gameManager;
    private PlayerSoundController playerSoundController;
    private InGameSoundManager inGameSoundManager;
    private float vertAxis;
    private bool stop = false;
    public bool inventoryFull;
    public float stamina;
    private float walkSpeed = 1.4f;
    private float sprintSpeed = 1.5f;
    private float rotationSpeed = 120f;

    void Awake()
    {
        playerSoundController = GetComponent<PlayerSoundController>();
        Invoke("SetStuff", 0.5f);
    }

    public void SetStuff()
    {
        inGameSoundManager = GameObject.Find("SoundManager").GetComponent<InGameSoundManager>();
        gameManager = GameObject.Find("GameManager");
        anim = GetComponent<Animator>();
        stamina = 100;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            inGameSoundManager.playFlashlightClick();
            CmdTurnFlashLightOnOff();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.GetComponent<GameManager>().gameOver == false)
        {
            if (anim != null)
            {
                if (stop == false)
                {
                    if (stamina < 100 && Input.GetKey(KeyCode.LeftShift) == false)
                    {
                        stamina = stamina + 0.3f;
                        GameObject.Find("StaminaBar").GetComponent<Image>().fillAmount = (stamina / 100f);
                    }
                    if (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.S) || !Input.GetKey(KeyCode.D))
                    {
                        vertAxis = 0;
                        anim.SetFloat("Speed", vertAxis);
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        transform.Rotate(0, -1 * rotationSpeed * Time.deltaTime, 0);
                        SetAnim(0.3f);
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.Rotate(0, 1 * rotationSpeed * Time.deltaTime, 0);
                        SetAnim(0.3f);
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        transform.Translate(0, 0, Time.deltaTime * -1);
                        SetAnim(1f);
                        sprint();
                    }
                    if (Input.GetKey(KeyCode.W))
                    {
                        transform.Translate(0, 0, Time.deltaTime * walkSpeed);
                        SetAnim(0.3f);
                        sprint();
                    }
                }
            }
        }
        else
        {
            SetAnim(0);
        }
    }

    void sprint()
    {
        if (stamina > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                SetAnim(0.6f);
                transform.Translate(0, 0, Time.deltaTime * sprintSpeed);
                stamina -= 0.4f;
                GameObject.Find("StaminaBar").GetComponent<Image>().fillAmount = (stamina / 100f);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                walkSpeed = 1.4f;
            }
        }
    }

    private void SetRotationAndSpeed(Vector3 direction)
    {
        transform.Translate(direction * (Time.deltaTime * walkSpeed));
    }

    public void SetAnim(float speed)
    {
        if (speed == 0)
        {
            anim.SetFloat("Speed", speed);
        }
        else
        {
            anim.SetFloat("Speed", speed);
        }
    }

    public void PausePlayer()
    {
        stop = true;
    }

    public void UnpausePlayer()
    {
        stop = false;
    }

    [Command]
    public void CmdAddStamina()
    {
        stamina = stamina + 100;
        if (stamina > 100)
        {
            stamina = 100;
        }
        RpcAddStamina();
        UpdateStamina();
    }

    [ClientRpc]
    public void RpcAddStamina()
    {
        if (isServer)
            return;
        stamina = stamina + 100;
        if (stamina > 100)
        {
            stamina = 100;
        }
        UpdateStamina();
    }

    public void UpdateStamina()
    {
        if (!isLocalPlayer)
            return;
        GameObject.Find("StaminaBar").GetComponent<Image>().fillAmount = (stamina / 100f);
    }

    public void PlayFootStep()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            CmdSendServerSound("PlayFootStepLoud");
        }
        else
        {
            CmdSendServerSound("PlayFootStepLow");
        }
    }

    [Command]
    void CmdSendServerSound(string chosenFootStepSound)
    {
        SendMessage(chosenFootStepSound);
        RpcSendSoundToClient(chosenFootStepSound);
    }

    [ClientRpc]
    void RpcSendSoundToClient(string chosenFootStepSound)
    {
        if (isServer)
            return;
        SendMessage(chosenFootStepSound);
    }

    [Command]
    private void CmdTurnFlashLightOnOff()
    {
        if (flashLight.activeSelf == false)
        {
            flashLight.SetActive(true);
            pointLight.SetActive(true);
            RpcTurnFlashLightOnOff();
        }
        else
        {
            flashLight.SetActive(false);
            pointLight.SetActive(false);
            RpcTurnFlashLightOnOff();
        }
    }

    [ClientRpc]
    private void RpcTurnFlashLightOnOff()
    {
        if (isServer)
            return;
        if (flashLight.activeSelf == false)
        {
            flashLight.SetActive(true);
            pointLight.SetActive(true);
        }
        else
        {
            flashLight.SetActive(false);
            pointLight.SetActive(false);
        }
    }
}