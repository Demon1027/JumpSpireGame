using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_Hidden_Big_Spike : MonoBehaviour
{
    Vector3 OriginalPos;
    public Right_Hidden_Spike_Group LHSG;
    private void Awake()
    {
        OriginalPos = gameObject.transform.position;
    }

    public void SetTrue()
    {
        gameObject.SetActive(true);
    }

    public void SetFalse()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            LHSG.Shoot_Spike();
            transform.position = OriginalPos;
            gameObject.SetActive(false);
        }
    }
}
