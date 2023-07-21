using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEScript : MonoBehaviour
{
    int DamageToDo;
    public GameObject DamageNumber;
    bool hasDoneDamage = false;
    public void AssignDTD(int DTD)
    {
        DamageToDo = DTD;
        Invoke("DealDamage", 0.01f);
    }

    void DealDamage()
    {
        if (!hasDoneDamage)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * 2.3f);
            //Debug.Log(colliders.Length);

            foreach (Collider2D collider in colliders)
            {
                //Debug.Log(collider.tag + " " + collider.name);
                // Check if the collider belongs to a target you want to deal damage to
                if (collider.CompareTag("Enemy"))
                {
                    float DR = collider.transform.GetComponent<EnemyMechanics>().ReturnDR();
                    if (DR > 0)
                    {
                        //Debug.Log("Old Damage: " + DamageToDo);
                        float tempfloat = DamageToDo * DR;
                        DamageToDo = (int)tempfloat;
                        //Debug.Log("New Damage: " + DamageToDo);
                    }

                    collider.transform.GetComponent<EnemyMechanics>().MinusHP(DamageToDo);
                    if (Globalvariables.FlamingBullet)
                    {
                        collider.transform.GetComponent<EnemyMechanics>().SetDOT((int)(DamageToDo * 0.3)); //do 1/3 of original damage
                    }

                    GameObject numberobject = Instantiate(DamageNumber, collider.transform.position, Quaternion.identity);

                    numberobject.GetComponent<DamageNumbers>().SetNumber(DamageToDo.ToString());
                }
            }

            hasDoneDamage = true;
        }
    }
}
