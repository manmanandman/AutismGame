using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeControl : MonoBehaviour
{

    public AudioSource source;
    private float musicVolume;
    void Awake()
    {
        musicVolume = this.GetComponent<Slider>().value;
    }

    // Update is called once per frame
    void Update()
    {
        source.volume = musicVolume;
    }

    public void updateVolume (float volume)
    {
        musicVolume = volume;
    }
}
