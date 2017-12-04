using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;
    [SerializeField]
    AudioClip bigBen;
    [SerializeField]
    AudioClip help;
    [SerializeField]
    AudioClip hello;
    [SerializeField]
    AudioClip citizenSave;

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Multiple instances of SoundEffects");
        }
        instance = this;
    }

    void MakeSound(AudioClip originalClip)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position);
    }
    public void BigBen()
    {
        MakeSound(bigBen);
    }

    public void Help()
    {
        MakeSound(help);
    }

    public void Hello()
    {
        MakeSound(hello);
    }

    public void CitizenSave()
    {
        MakeSound(citizenSave);
    }
}
