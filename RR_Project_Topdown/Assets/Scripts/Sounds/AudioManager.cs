using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

//How To Use : 
//FindObjectOfType<AudioManager>().Play("AudioName");

public class AudioManager : MonoSingleton<AudioManager>
{
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

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s==null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
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

    /*private void ThemeStart()
    {
        Play("Pre-MainTheme");
        Sound preTheme = GetSound("Pre-MainTheme");


    }*/

    private IEnumerator ThemeStart()
    {
        Play("Pre-MainTheme");
        Sound preTheme = GetSound("Pre-MainTheme");
        yield return new WaitForSeconds(preTheme.clip.length-1.2f);
        Play("MainTheme");

    }

}
