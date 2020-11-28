using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;
    public Sound[] sounds;
    

    // Update is called once per frame
    void Awake()
    {
        MakeSingleton();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(s.soundType.ToString())[0];
        }

    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Play(string name, string option)
    {
        if (option == "Continuous")
        {
            foreach (Sound s in sounds)
            {
                if (s.name != name && s.soundType.ToString() == "Music")
                {
                    s.source.Stop();
                }
            }
        }
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                if (option == "Once" && !s.source.isPlaying)
                {
                    s.source.PlayOneShot(s.source.clip, s.source.volume);
                }
                else if (option == "Continuous" && !s.source.isPlaying)
                {
                    s.source.Play();
                }
            }
        }
    }
}
