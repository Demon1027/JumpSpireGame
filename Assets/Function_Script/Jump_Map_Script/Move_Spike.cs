using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Spike : MonoBehaviour
{
    Rigidbody2D rigid;

    [Header("Speed Range")]
    [SerializeField] private float MinSpeed = -0.1f;
    [SerializeField] private float MaxSpeed = 0.1f;

    [Header("Speed Change Delay Range (sec)")]
    [SerializeField] private float MinDelay = 1f;
    [SerializeField] private float MaxDelay = 5f;

    public float Speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(SetRandomSpeedRoutine());
    }

    private IEnumerator SetRandomSpeedRoutine()
    {
        while (true)
        {
            Speed = Random.Range(MinSpeed, MaxSpeed);             
            float delay = Random.Range(MinDelay, MaxDelay);         
            yield return new WaitForSeconds(delay);
        }
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + new Vector2( 100 * Speed * Time.fixedDeltaTime, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            Speed = -Speed;
        }
    }
}
