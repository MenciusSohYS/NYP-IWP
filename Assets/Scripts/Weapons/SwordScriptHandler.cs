using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScriptHandler : MonoBehaviour
{
    [SerializeField] SwordScript SwordScriptScript;

    public void ActivateSword()
    {
        SwordScriptScript.SetBoxActive(true);
        SwordScriptScript.MeleeSound();
    }
    public void DeactivateSword()
    {
        SwordScriptScript.SetBoxActive(false);
    }
    private void Update()
    {
        
    }
}
