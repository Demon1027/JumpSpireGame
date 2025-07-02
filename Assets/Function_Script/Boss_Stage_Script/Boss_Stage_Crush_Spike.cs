using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss_Stage_Crush_Spike : MonoBehaviour
{
    public Right_Hidden_Big_Spike RHBS;
    public Left_Hidden_Big_Spike LHBS;
    public Boss_Stage_Camera BSC;
    public Crush_Spike_Group CSG;
    Rigidbody2D rigid;
    bool Hidden_Boss_State = false;
    Vector3 OriginalPos;
    private void Awake()
    {
        OriginalPos = gameObject.transform.position;
        rigid = GetComponent<Rigidbody2D>();
    }
    public void SetTrue(bool Hidden_Boss)
    {
        if(Hidden_Boss == true)
        {
            Hidden_Boss_State = true;
        }
        gameObject.SetActive(true);
    }

    public void SetFalse()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            BSC.Up_Down_Camera_Effect(0.5f, 1.6f);
            CSG.Shoot_Spike();
            transform.position = OriginalPos;
            if(Hidden_Boss_State == true)
            {
                RHBS.SetTrue();
                LHBS.SetTrue();
            }
            gameObject.SetActive(false);
        }
    }
}
