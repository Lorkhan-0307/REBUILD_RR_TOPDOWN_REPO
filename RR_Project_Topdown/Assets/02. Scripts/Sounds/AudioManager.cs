using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

//How To Use : 
//FindObjectOfType<AudioManager>().Play("AudioName"); 

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioMixer mixer;
    public Sound[] sounds;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        StartCoroutine(ThemeStart());
    }

    private void Update()
    {

    }

    public void Play(string name, int num)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //num이 0일때 SFX 
        if (num == 0)
        {
            s.source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        }
        //num이 1일때 BGSound
        else
        {
            s.source.outputAudioMixerGroup = mixer.FindMatchingGroups("BGsound")[0];
        }
        s.source.Play();
    }

    public void Stop(string name, int num)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //num이 0일때 SFX 
        if (num == 0)
        {
            s.source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        }
        //num이 1일때 BGSound
        else
        {
            s.source.outputAudioMixerGroup = mixer.FindMatchingGroups("BGsound")[0];
        }
        s.source.Stop();
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s;
    }

    private IEnumerator ThemeStart()
    {
        Play("Pre-MainTheme", 1);
        Sound preTheme = GetSound("Pre-MainTheme");
        //Debug.Log(preTheme.clip.length);
        yield return new WaitForSeconds(preTheme.clip.length-1.2f);
        Play("MainTheme", 1);
    }
}
