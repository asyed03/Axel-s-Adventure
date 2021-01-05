using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public enum SoundType { Music, SFX, Master };
    public SoundType soundType;

    void Awake()
    {
        GameManager.instance.LoadData();
        Debug.Log(GameManager.instance.masterLevel);
        Debug.Log(Mathf.Log10(GameManager.instance.masterLevel) * 20);
        Debug.Log(GameManager.instance.musicLevel);
        Debug.Log(Mathf.Log10(GameManager.instance.musicLevel) * 20);
        Debug.Log(GameManager.instance.sfxLevel);
        Debug.Log(Mathf.Log10(GameManager.instance.sfxLevel) * 20);

        var slider = gameObject.GetComponent<Slider>();
        if (soundType == SoundType.Music)
            slider.value = GameManager.instance.musicLevel;
        else if (soundType == SoundType.SFX)
            slider.value = GameManager.instance.sfxLevel;
        else if (soundType == SoundType.Master)
            slider.value = GameManager.instance.masterLevel;
    }

    public void SoundSlider()
    {
        float volume = gameObject.GetComponent<Slider>().value;
        if (soundType == SoundType.Music)
        {
            GameManager.instance.musicLevel = volume;
            AudioManager.instance.audioMixer.SetFloat("volumeMusic", Mathf.Log10(GameManager.instance.musicLevel) * 20);
        }
        else if (soundType == SoundType.SFX)
        {
            GameManager.instance.sfxLevel = volume;
            AudioManager.instance.audioMixer.SetFloat("volumeSFX", Mathf.Log10(GameManager.instance.sfxLevel) * 20);
        }
        else if (soundType == SoundType.Master)
        {
            GameManager.instance.masterLevel = volume;
            AudioManager.instance.audioMixer.SetFloat("volumeMusic", Mathf.Log10(GameManager.instance.masterLevel) * 20);

        }
        Debug.Log(GameManager.instance.musicLevel);
        GameManager.instance.SaveData();
    }
}
