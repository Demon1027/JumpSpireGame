using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_Boss_Block : MonoBehaviour
{
    public Boss B;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 11)
        {
            B.Back_Boss();
        }
    }
}
