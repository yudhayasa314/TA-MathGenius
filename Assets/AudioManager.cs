using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Script ini saya kurang tau ngejelasinnya
    //bisa di tonton penjelasan nya di sini https://www.youtube.com/watch?v=6OT43pvUyfY

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

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //Play("BGM Menu");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.loop)
        {
            //Jika mau puter looping music (yg berarti BGM)
            //Stop other BGM first
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].loop && sounds[i].name != name)
                {
                    sounds[i].source.Stop();
                }
            }
        }

        //Play BGM
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    public void StopAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source.Stop();
        }
    }
}
