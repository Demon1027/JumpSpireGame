using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Camera : MonoBehaviour
{
    public Transform Player;
    bool stop = false;
    float fixedX;
    void Start()
    {
        fixedX = transform.position.x;
    }


    void FixedUpdate()
    {
        if(!stop)
        {
            Vector3 newPos = new Vector3(fixedX, Player.position.y + 4f, -10);
            transform.position = newPos;
        }
        if (Player.position.y >= 78.5)
        {
            stop = true;
            Vector3 Fixed = new Vector3(fixedX, 82.5f, -10);
        }
        if (Player.position.y < 78.5)
        {
            stop = false;
        }
    }
}
