using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left_Hidden_Big_Spike : MonoBehaviour
{
    Vector3 OriginalPos;
    public Boss_Stage_Camera BSC;
    public Left_Hidden_Spike_Group LHSG;
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
            BSC.Up_Down_Camera_Effect(0.5f, 2f);
            LHSG.Shoot_Spike();
            transform.position = OriginalPos;
            gameObject.SetActive(false);
        }
    }
}
