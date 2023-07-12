using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public void SetLevel(float SliderValue)
    {
        mixer.SetFloat("Volume", Mathf.Log10(SliderValue) * 20);
    }
}