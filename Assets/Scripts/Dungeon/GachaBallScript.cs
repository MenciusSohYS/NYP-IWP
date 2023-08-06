using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaBallScript : MonoBehaviour
{
    Animator Anim;
    float Timer;
    float CurrentTime;
    bool animdone;
    CircleCollider2D[] CircleColliders;
    GameObject Player;
    bool CanMoveOn;
    float TimerBeforeReEnable;
    bool CanStartCountDown;
    CanvasScript CVScript;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CanMoveOn = false;
        Anim = GetComponent<Animator>();
        Anim.SetBool("PlayAnim", true);
        Timer = Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        CurrentTime = 0;
        animdone = false;
        CircleColliders = GetComponents<CircleCollider2D>();
        TimerBeforeReEnable = 2f;
        CanStartCountDown = false;
        CVScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>();
        foreach (CircleCollider2D collider in CircleColliders)
        {
            collider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTime < Timer)
        {
            CurrentTime += Time.deltaTime;
            return;
        }
        else if (!animdone)
        {
            Anim.SetBool("PlayAnim", false);
            animdone = true;
            foreach (CircleCollider2D collider in CircleColliders)
            {
                collider.enabled = true;
            }
        }
        else
        {
            if (CanMoveOn)
            {
                foreach (CircleCollider2D collider in CircleColliders)
                {
                    collider.enabled = false;
                    CanMoveOn = false;
                    CanStartCountDown = true;
                }
                int WhatDidPlayerGet = Random.Range(0, 101);

                if (WhatDidPlayerGet >= 95)
                {
                    Player.GetComponent<PlayerMechanics>().MessagePlayer("YOU GOT 600 COINS!");
                    CVScript.SetCoins(CVScript.GetCoins() + 600);
                }
                else if (WhatDidPlayerGet >= 85)
                {
                    Player.GetComponent<PlayerMechanics>().MessagePlayer("YOU GOT 400 COINS!");
                    CVScript.SetCoins(CVScript.GetCoins() + 400);
                }
                else if (WhatDidPlayerGet >= 65)
                {
                    Player.GetComponent<PlayerMechanics>().MessagePlayer("YOU BROKE EVEN!");
                    CVScript.SetCoins(CVScript.GetCoins() + 200);
                }
                else if (WhatDidPlayerGet >= 30)
                {
                    Player.GetComponent<PlayerMechanics>().MessagePlayer("YOU GOT 100 COINS!");
                    CVScript.SetCoins(CVScript.GetCoins() + 100);
                }
                else
                {
                    Player.GetComponent<PlayerMechanics>().MessagePlayer("YOU GOT NOTHING!");
                }

                CVScript.SendScore();
            }
            else if (TimerBeforeReEnable > 0 && CanStartCountDown)
            {
                TimerBeforeReEnable -= Time.deltaTime;
            }
            else if (CanStartCountDown)
            {
                transform.parent.GetComponent<GachaMachine>().ReEnableMachine();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanMoveOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanMoveOn = false;
        }
    }
}
