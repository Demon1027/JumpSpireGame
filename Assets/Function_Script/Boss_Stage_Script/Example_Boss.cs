using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_Boss : MonoBehaviour
{
    Vector2 StartPos;
    Rigidbody2D rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartPos = transform.position;
    }

    public void Start_Simulation_Boss_Move()
    {
        StartCoroutine(Simulation_Boss_Move());
    }

    private IEnumerator Simulation_Boss_Move()
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i<4; i++)
        {
            rigid.AddForce(new Vector2(0, 1000), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.47f);
            rigid.velocity = Vector2.zero;
            rigid.AddForce(new Vector2(0, -1500), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.5f);
            rigid.velocity = Vector2.zero;
        }
    }
}
