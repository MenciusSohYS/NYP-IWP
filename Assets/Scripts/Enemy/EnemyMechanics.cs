using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMechanics : MonoBehaviour
{
    [SerializeField] int CurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MinusHP(int MinusBy)
    {
        if (CurrentHealth > MinusBy)
            CurrentHealth -= MinusBy;
        else
            Destroy(gameObject);
    }
}
