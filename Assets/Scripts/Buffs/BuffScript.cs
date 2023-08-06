using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffScript : MonoBehaviour
{
    protected GameObject Player;
    bool TriggeredOnce;
    public AudioSource ASource;
    bool Alreadyused;
    void Start()
    {
        Alreadyused = false;
        TriggeredOnce = false;
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    public virtual void ApplyBuffs()
    {
        Debug.Log("Default");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player && TriggeredOnce && !Alreadyused)
        {
            Debug.Log("used");
            Alreadyused = true;
            ApplyBuffs();
            ASource.Play();
            TellPickup();
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, ASource.clip.length);
        }
        else
            TriggeredOnce = true;
    }

    protected virtual void TellPickup()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>().SetText("", 1);
    }
}
