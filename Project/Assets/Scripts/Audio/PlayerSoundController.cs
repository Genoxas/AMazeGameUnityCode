using UnityEngine;
using System.Collections;


//Author: Christian Reyes
//Description: Class used to play player foot step sounds as well as mix sounds so they are dynamic (never hear the same exact sound twice)

public class PlayerSoundController : MonoBehaviour
{

    [SerializeField]
    private AudioSource feetAudioSource;
    [SerializeField]
    private AudioSource fightingAudioSource;
    [SerializeField]
    private AudioSource impactAudioSource;
    [SerializeField]
    private AudioSource playerVocals;

    [SerializeField]
    private AudioClip punch;
    [SerializeField]
    private AudioClip swing;
    [SerializeField]
    private AudioClip punchImpact;
    [SerializeField]
    private AudioClip swordImpact;
    [SerializeField]
    private AudioClip swordImpactAlternate;
    [SerializeField]
    private AudioClip hitGrunt;
    [SerializeField]
    private AudioClip hitGruntAlternate;
    [SerializeField]
    private AudioClip deathGargle;

    public void PlayFootStepLow()
    {
        SoundMixer(feetAudioSource, 0.15f, 0.25f, 0.5f, 0.6f);
        feetAudioSource.Play();
    }

    public void PlayFootStepLoud()
    {
        SoundMixer(feetAudioSource, 0.8f, 0.9f, 0.7f, 0.9f);
        feetAudioSource.Play();
    }

    public void PlayPunch()
    {
        fightingAudioSource.clip = punch;
        SoundMixer(fightingAudioSource, 0.5f, 0.7f, 0.8f, 0.9f);
        fightingAudioSource.Play();
    }

    public void PlaySwing()
    {
        fightingAudioSource.clip = swing;
        SoundMixer(fightingAudioSource, 0.2f, 0.3f, 0.8f, 0.9f);
        fightingAudioSource.Play();
    }

    public void PlayPunchImpact()
    {
        impactAudioSource.clip = punchImpact;
        SoundMixer(impactAudioSource, 0.8f, 0.9f, 0.8f, 0.9f);
        impactAudioSource.Play();
    }

    public void PlaySwordImpact()
    {
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0)
        {
            impactAudioSource.clip = swordImpact;
        } else
        {
            impactAudioSource.clip = swordImpactAlternate;
        }
        SoundMixer(impactAudioSource, 0.8f, 0.9f, 0.8f, 0.9f);
        impactAudioSource.Play();
    }

    public void PlayHitReaction()
    {
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0)
        {
            playerVocals.clip = hitGrunt;
        }
        else
        {
            playerVocals.clip = hitGruntAlternate;
        }
        SoundMixer(playerVocals, 0.8f, 0.9f, 0.8f, 0.9f);
        playerVocals.Play();
    }

    public void PlayDeathReaction()
    {
        playerVocals.clip = deathGargle;
        SoundMixer(playerVocals, 0.8f, 0.9f, 0.8f, 0.9f);
        playerVocals.Play();
    }

    private void SoundMixer(AudioSource audioSource, float minVol, float maxVol, float minPitch, float maxPitch)
    {
        float randomVolume = Random.Range(minVol, maxVol);
        float randomPitch = Random.Range(minPitch, maxPitch);

        audioSource.volume = randomVolume;
        audioSource.pitch = randomPitch;
    }
}
