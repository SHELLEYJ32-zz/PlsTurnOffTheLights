using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public void SetMusicVolume(float vol)
    {
        GameplayController.instance.audioMixer.SetFloat("MusicVol", vol);
    }

    public void SetSFXVolume(float vol)
    {
        GameplayController.instance.audioMixer.SetFloat("SFXVol", vol);
    }
}
