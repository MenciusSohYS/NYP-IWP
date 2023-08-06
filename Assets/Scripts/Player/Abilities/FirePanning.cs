using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirePanning : MonoBehaviour
{
    SpriteRenderer SpriteRendererField;
    public Sprite[] Pictures;
    [SerializeField] int CurrentImage;
    [SerializeField] float CountDownToChange;
    [SerializeField] int DamageToDo;
    [SerializeField] GameObject DamageNumber;

    private void Start()
    {
        SpriteRendererField = GetComponent<SpriteRenderer>();
        CountDownToChange = 0.2f;
        CurrentImage = 0;
        DamageToDo = (int)(10 * (1 + (Globalvariables.CurrentLevel * 0.3f)));

        Destroy(gameObject, 5);
    }

    private void Update()
    {
        if (CountDownToChange > 0)
        {
            CountDownToChange -= Time.deltaTime;
            return;
        }

        CountDownToChange = 0.2f;
        ++CurrentImage;
        if (CurrentImage > 5)
            CurrentImage = 0;
        SpriteRendererField.sprite = Pictures[CurrentImage];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Enemy"))
        {
            float DR = collision.transform.GetComponent<EnemyMechanics>().ReturnDR();
            if (DR > 0)
            {
                //Debug.Log("Old Damage: " + DamageToDo);
                float tempfloat = DamageToDo * DR;
                DamageToDo = (int)tempfloat;
                //Debug.Log("New Damage: " + DamageToDo);
            }

            collision.transform.GetComponent<EnemyMechanics>().MinusHP(DamageToDo);

            GameObject numberobject = Instantiate(DamageNumber, collision.transform.position, Quaternion.identity);

            numberobject.GetComponent<DamageNumbers>().SetNumber(DamageToDo.ToString(), false);

            collision.transform.GetComponent<EnemyMechanics>().SetDOT(DamageToDo);
        }
    }
}
