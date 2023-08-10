using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScriptHandler : MonoBehaviour
{
    [SerializeField] SwordScript SwordScriptScript;

    public void ActivateSword()
    {
        SwordScriptScript.SetBoxActive(true);
    }
    public void DeactivateSword()
    {
        SwordScriptScript.SetBoxActive(false);
    }
}
