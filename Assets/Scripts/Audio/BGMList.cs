using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMList : MonoBehaviour
{
    [SerializeField] AudioClip[] ListOfMusics;
    [SerializeField] AudioSource audioSource;
    private void Start()
    {
        audioSource.time = Globalvariables.timerforsound;
    }

    public void PlayBattleMusic()
    {
        audioSource.clip = ListOfMusics[1];
        audioSource.Play();
    }
    public void PlayBGM()
    {
        audioSource.clip = ListOfMusics[0];
        audioSource.Play();
    }
    public void PlayBossBattleMusic()
    {
        audioSource.clip = ListOfMusics[3];
        audioSource.Play();
    }
    public void PlayBossBGM()
    {
        audioSource.clip = ListOfMusics[2];
        audioSource.Play();
    }
}
