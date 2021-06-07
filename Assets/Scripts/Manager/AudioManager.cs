using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //when game starts -> create all sound sources wil values
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.output;
        }
    }

    // called from where specific sound is needed
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if( s == null)
        { 
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }
  
        s.source.Play();
    }
}
