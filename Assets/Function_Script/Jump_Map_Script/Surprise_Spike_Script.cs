using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surprise_Spike_Script : MonoBehaviour
{
    Rigidbody2D rigid;
    bool Trigger = false;
    [SerializeField] float Wait_Time_S = 5f;
    [SerializeField] float My_Y_Position = 0;
    [SerializeField] float My_X_Position = 0;
    [SerializeField] float D_Spike = 0;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(Trigger)
        {
            rigid.MovePosition(new Vector2(rigid.position.x + 0.7f, rigid.position.y));
            if (rigid.position.x > 8 - D_Spike)
            {
                Trigger = false;
                transform.position = new Vector2(-9.57f + My_X_Position, 47.82f + My_Y_Position);
            }            
        }
    }

    public void Surprise_Moving()
    {
        Invoke("WaitTime", Wait_Time_S);
    }

    void WaitTime()
    {
        Debug.Log("함수 실행");
        Trigger = true;
    }
}
