////////////////////////////////////////////////////////////
// File: SoundManager.cs
// Author: Charles Carter
// Brief: To play any sound requested by another script
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //The pool of available AudioSources
    [SerializeField]
    AudioSource VFXSource;

    private void Awake()
    {
        VFXSource = VFXSource ?? GetComponent<AudioSource>();
    }

    private void Start()
    {
        //There was no VFX AudioSource on the object
        if (!VFXSource)
        {
            VFXSource = gameObject.AddComponent<AudioSource>();
        }
    }


    //Playing the given audio clip
    public void PlayVFXSound(AudioClip clipToPlay)
    {
        //Setting the clip and playing it
        VFXSource.clip = clipToPlay;
        VFXSource.Play();
    }
}
