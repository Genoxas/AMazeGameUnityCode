using UnityEngine;
using System.Collections;

/*
* Author: Christian
* Description: Sound controller used by the monster in order to play monster sounds (foot steps, growl, hit impact).
*/
public class MonsterSoundController : MonoBehaviour {

    [SerializeField]
    private AudioSource monsterVocals;
    [SerializeField]
    private AudioSource monsterImpact;
    [SerializeField]
    private AudioSource monsterFootStep;

    public AudioClip monsterRoar;
    public AudioClip monsterGrowl;
    public AudioClip monsterClawHit;

    public void PlayMonsterRoar()
    {
        monsterVocals.clip = monsterRoar;
        SoundMixer(monsterVocals, 0.33f, 0.4f, 0.9f, 1f);
        monsterVocals.Play();
    }

    public void PlayMonsterGrowl()
    {
        monsterVocals.clip = monsterGrowl;
        SoundMixer(monsterVocals, 0.3f, 0.4f, 0.9f, 0.9f);
        monsterVocals.Play();
    }

    public void PlayMonsterImpact()
    {
        monsterImpact.clip = monsterClawHit;
        SoundMixer(monsterImpact, 0.6f, 0.75f, 0.6f, 0.8f);
        monsterImpact.Play();
    }

    public void PlayMonsterWalking()
    {
        SoundMixer(monsterFootStep, 0.4f, 0.6f, 0.5f, 0.7f);
        monsterFootStep.Play();
    }

    private void SoundMixer(AudioSource audioSource, float minVol, float maxVol, float minPitch, float maxPitch)
    {
        float randomVolume = Random.Range(minVol, maxVol);
        float randomPitch = Random.Range(minPitch, maxPitch);

        audioSource.volume = randomVolume;
        audioSource.pitch = randomPitch;
    }
}
