using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public enum SoundType { Music, SFX };
    public SoundType soundType;

    void Awake()
    {
        var slider = gameObject.GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("volume" + soundType.ToString(), 1);
    }

    public void SoundSlider(string s)
    {
        float volume = gameObject.GetComponent<Slider>().value;
        AudioManager.instance.audioMixer.SetFloat("volume" + s, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume" + s, volume);
    }
}
