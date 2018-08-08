using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerMovementTutorial : MonoBehaviour
{

    /*
     * Author: Christian Reyes
     * Description: Script used to control character movements
     * 
     */

    Animator anim;
    private float vertAxis;
    private bool stop = false;
    public bool inventoryFull;
    public float stamina;
    private GameObject gameManager;
    private float walkSpeed = 1.4f;
    private float sprintSpeed = 1.5f;
    private float rotationSpeed = 120f;

    void Start()
    {
        Invoke("SetStuff", 0.5f);
    }

    public void SetStuff()
    {
        gameManager = GameObject.Find("GameManager");
        anim = GetComponent<Animator>();
        stamina = 100;
        UpdateStamina();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stamina < 100 && Input.GetKey(KeyCode.LeftShift) == false)
        {
            stamina = stamina + 0.3f;
            //GameObject staminaUI = GameObject.Find("StaminaText");
            GameObject.Find("StaminaBar").GetComponent<Image>().fillAmount = (stamina / 100f);
            //Text staminaText = staminaUI.GetComponent<Text>();
            //staminaText.text = string.Format("Stamina : {0}%", Mathf.Round(stamina));
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

    void sprint()
    {
        if (stamina > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                SetAnim(0.6f);
                transform.Translate(0, 0, Time.deltaTime * sprintSpeed);

                //GameObject staminaUI = GameObject.Find("StaminaText");
                stamina -= 0.4f;
                //Text staminaText = staminaUI.GetComponent<Text>();
                //staminaText.text = string.Format("Stamina : {0}%", Mathf.Round(stamina));
                GameObject.Find("StaminaBar").GetComponent<Image>().fillAmount = (stamina / 100f);
            }
        }
    }

    private void SetRotationAndSpeed(Vector3 direction)
    {
        transform.Translate(direction * (Time.deltaTime * walkSpeed));
    }

    public void SetAnim(float speed)
    {
        anim.SetFloat("Speed", speed);
    }

    public void PausePlayer()
    {
        stop = true;
    }

    public void UnpausePlayer()
    {
        stop = false;
    }

    public void CmdAddStamina()
    {
        stamina = stamina + 100;
        if (stamina > 100)
        {
            stamina = 100;
        }
        UpdateStamina();
    }


    public void UpdateStamina()
    {
        GameObject.Find("StaminaBar").GetComponent<Image>().fillAmount = (stamina / 100f);
    }
}