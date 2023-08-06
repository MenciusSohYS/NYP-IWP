using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaMachine : MonoBehaviour
{
    private GameObject Player;
    private bool CanMoveOn;
    BoxCollider2D[] colliders;
    public CircleCreator CircleCreatorScript;
    public AudioSource AudioSourceField;
    public GameObject BallPrefab;
    public CanvasScript CVScript;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CanMoveOn = false;
        colliders = GetComponentsInChildren<BoxCollider2D>(true); //true to include those that are inactive
        CVScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>();
        //Debug.Log(colliders.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMoveOn)
        {
            Player.GetComponent<PlayerMechanics>().MessagePlayer("E to spend 200 coins");
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (CVScript.GetCoins() < 200)
                {
                    Player.GetComponent<PlayerMechanics>().MessagePlayer("NOT ENOUGH MONEY!");
                    foreach (BoxCollider2D collider in colliders)
                    {
                        collider.enabled = false;
                        CanMoveOn = false;
                    }
                }
                else
                {
                    PlayFabHandler.DeductCoins(200);
                    CVScript.SetCoins(CVScript.GetCoins() - 200);
                    BoughtGachaBall();
                    Player.GetComponent<PlayerMechanics>().MessagePlayer("SPENT!");
                    foreach (BoxCollider2D collider in colliders)
                    {
                        collider.enabled = false;
                        CanMoveOn = false;
                    }
                }
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

    void BoughtGachaBall()
    {
        GameObject BallGO = Instantiate(BallPrefab, transform.position - new Vector3(0, 0, -0.1f), Quaternion.identity);
        BallGO.transform.SetParent(transform);
    }

    public void ReEnableMachine()
    {
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = true;
        }
    }
}
