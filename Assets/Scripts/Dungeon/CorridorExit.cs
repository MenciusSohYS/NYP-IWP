using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorExit : MonoBehaviour
{
    [SerializeField] int OrientationOfCorridor;
    [SerializeField] Vector3 PositionOfPlayerWhenEntering;
    private void Start()
    {
        OrientationOfCorridor = transform.parent.parent.GetComponent<CorridorScript>().ReturnOrientation();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //player exited corridor, start the enemy ai
        if (collision.transform.tag == "Player")
        {
            PositionOfPlayerWhenEntering = collision.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //player exited corridor, start the enemy ai
        //Debug.Log(collision.transform.tag);
        if (collision.transform.tag == "Player")
        {
            bool ExitedCorrectly = false;
            switch(OrientationOfCorridor) //check if you exited correctly
            {
                case 0:
                    if (PositionOfPlayerWhenEntering.y < collision.transform.position.y)
                    {
                        ExitedCorrectly = true;
                        break;
                    }
                    break;
                case 1:
                    if (PositionOfPlayerWhenEntering.y > collision.transform.position.y)
                    {
                        ExitedCorrectly = true;
                        break;
                    }
                    break;
                case 2:
                    if (PositionOfPlayerWhenEntering.x < collision.transform.position.x)
                    {
                        ExitedCorrectly = true;
                        break;
                    }
                    break;
                case 3:
                    if (PositionOfPlayerWhenEntering.x > collision.transform.position.x)
                    {
                        ExitedCorrectly = true;
                        break;
                    }
                    break;
                default:
                    break;
            }

            if (ExitedCorrectly)
            {
                //Debug.Log("Player exited corridor and entered room");
                collision.transform.GetComponent<PlayerMechanics>().AnnounceRoomEntered();
                transform.parent.parent.GetComponent<CorridorScript>().EnableEnemyAI();
            }
        }
    }
}
