using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePanning : MonoBehaviour
{
    SpriteRenderer SpriteRendererField;
    float OffsetX = 0;
    private void Start()
    {
        SpriteRendererField = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        OffsetX += Time.deltaTime;
        //SpriteRendererField.material.mainTextureOffset = new Vector2(OffsetX, 0);
    }
}
