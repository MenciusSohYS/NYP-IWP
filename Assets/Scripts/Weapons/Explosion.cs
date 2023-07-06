using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource ExplosionAudioSource;
    SpriteRenderer SpriteRendererComponent;

    private void Start()
    {
        ExplosionAudioSource.Play();
        SpriteRendererComponent = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        SpriteRendererComponent.color = SpriteRendererComponent.color - new Color(0, 0, 0, 0.5f * Time.deltaTime);
        if (SpriteRendererComponent.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
