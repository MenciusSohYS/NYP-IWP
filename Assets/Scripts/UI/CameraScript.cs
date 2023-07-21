using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject Player;
    private bool PauseCamera;
    bool OpenedBigMap;
    // Start is called before the first frame update
    void Start()
    {
        OpenedBigMap = false;
        PauseCamera = false;
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseCamera)
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
        }
    }

    public void StopCamera()
    {
        PauseCamera = true;
    }
    public void ResumeCamera()
    {
        PauseCamera = false;
    }

    public void ChangeOpenedBigMap(bool SetTo)
    {
        OpenedBigMap = SetTo;
    }

    public bool ReturnOpenedBigMap()
    {
        return OpenedBigMap;
    }
    public bool ReturnPauseCamera()
    {
        return PauseCamera;
    }

    public void SetPlayer(GameObject SetThis)
    {
        //Debug.Log("Setting player");
        Player = SetThis;
    }
}
