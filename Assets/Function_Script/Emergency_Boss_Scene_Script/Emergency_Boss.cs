using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emergency_Boss : MonoBehaviour
{
    Rigidbody2D rigid;
    public Emergency_Scene_Player Player;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rigid.velocity = new Vector2(0, -50);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {           
            Player.Boss_Impact();
        }
    }
}
