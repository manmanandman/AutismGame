using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusicSource;
    public AudioSource soundEffectSource;
    public AudioSource voiceOverSource;

    private AudioClip currentBGM;

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AudioManager>();

            if (instance == null)
                Debug.Log("There is no Audio Manager");

            return instance;
        }
    }
    
    public void PlayBGMAudio(AudioClip clip)
    {
        if (currentBGM != clip)
        {
            currentBGM = clip;
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.Play();
        }
    }

    public void Stop()
    {
        backgroundMusicSource.Stop();
    }

    public void PlayOneShot(AudioClip clip)
    {
        soundEffectSource.PlayOneShot(clip);
    }
    public void PlayVoiceOver(AudioClip clip,bool check)
    {
        if(!check)
        {
            voiceOverSource.Stop();
            voiceOverSource.PlayOneShot(clip);
        }

    }

    public void WaitandPlaySound(float time, AudioClip clip)
    {
        StartCoroutine(WaitPlaySound(time, clip));
    }

    IEnumerator WaitPlaySound(float time, AudioClip clip)
    {
        yield return new WaitForSeconds(time);
        backgroundMusicSource.PlayOneShot(clip);
    }
}
