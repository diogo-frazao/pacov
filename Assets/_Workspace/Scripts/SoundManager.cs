using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource myAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(AudioClip clip, float volumeScale, bool canOverlapSounds)
    {
        myAudioSource = myAudioSource == null ? GetComponent<AudioSource>() : myAudioSource;

        if (canOverlapSounds)
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.PlayOneShot(clip, volumeScale);
            Destroy(newAudioSource, clip.length + 0.25f);
        }
        else
        {
            myAudioSource.PlayOneShot(clip, volumeScale);
        }
    }
}
