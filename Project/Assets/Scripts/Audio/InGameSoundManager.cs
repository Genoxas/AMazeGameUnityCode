using UnityEngine;
using System.Collections;

public class InGameSoundManager : MonoBehaviour {

    public AudioSource backgroundMusic;
    [SerializeField]
    private AudioSource hpHealEffect;
    [SerializeField]
    private AudioSource inventoryDialogPopupEffect;
    [SerializeField]
    private AudioSource inventoryDialogPopupYesEffect;
    [SerializeField]
    private AudioSource inventoryDialogPopupNoEffect;
    [SerializeField]
    private AudioSource swordSwingEffect;
    [SerializeField]
    private AudioSource punchEffect;
    [SerializeField]
    private AudioSource flashLightClick;

    public void playHpHealEffect()
    {
        hpHealEffect.Play();
    }

    public void playInventoryDialogPopupEffect()
    {
        inventoryDialogPopupEffect.Play();
    }

    public void playInventoryDialogPopupYesEffect()
    {
        inventoryDialogPopupYesEffect.Play();
    }

    public void playInventoryDialogPopupNoEffect()
    {
        inventoryDialogPopupNoEffect.Play();
    }

    public void playSwordSwingEffect()
    {
        swordSwingEffect.Play();
    }

    public void playPunchEffect()
    {
        punchEffect.Play();
    }

    public void playFlashlightClick()
    {
        flashLightClick.Play();
    }
}
