using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake(){
        foreach(Sound s in sounds){
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.audioSource.volume;
            s.audioSource.pitch = s.audioSource.pitch;
        }
    }
    public void Play(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Play();
    }
}


[System.Serializable]
public class Sound{
    public string name;
    public AudioClip clip;

    [Range(0,1)]
    public float volume;
    [Range(0.1f,3)]
    public float pitch;

    [HideInInspector]
    public AudioSource audioSource;
}
