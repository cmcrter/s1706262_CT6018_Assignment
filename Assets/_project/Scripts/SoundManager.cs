////////////////////////////////////////////////////////////
// File: SoundManager.cs
// Author: Charles Carter
// Brief: To play any sound requested by another script
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //The pool of available AudioSources
    [SerializeField]
    private List<AudioSource> AudioSourcesToUse = new List<AudioSource>();
    [SerializeField]
    private int iMinAudioSourceAmount = 5;

    private void Awake()
    {
        //Trying to set a list based on the AudioSources already on the object
        AudioSourcesToUse = AudioSourcesToUse ?? GetComponents<AudioSource>().ToList();
    }

    private void Start()
    {
        //There was no AudioSources on the object
        if (AudioSourcesToUse.Count == 0)
        {
            //Go through to the minimum audio source num
            for (int i = 0; i < iMinAudioSourceAmount; ++i)
            {
                //Adding audio sources and adding them to the available list
                AudioSourcesToUse.Add(gameObject.AddComponent<AudioSource>());
            }
        }
    }

    //Getting the first free audioSource
    private AudioSource freeSource()
    {
        //Going through the source list
        foreach (AudioSource source in AudioSourcesToUse)
        {
            //If it's not playing anything
            if (!source.isPlaying)
            {
                //Return it
                return source;
            }
        }

        return null;
    }

    //Playing the given audio clip
    public void PlayAudioClip(AudioClip clipToPlay)
    {
        //Looking for a free audiosource
        AudioSource sourceToUse = freeSource();

        //If there isnt one
        if (!sourceToUse)
        {
            //Add one
            sourceToUse = gameObject.AddComponent<AudioSource>();
            AudioSourcesToUse.Add(sourceToUse);
        }

        //Setting the clip and playing it
        sourceToUse.clip = clipToPlay;
        sourceToUse.Play();
    }
}
