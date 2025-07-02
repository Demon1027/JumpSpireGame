using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Emergency_Boss_Camera : MonoBehaviour
{
    public Camera Main_C;
    public Transform Boss_P;
    public float Camera_P = 3;
    public Camera Boss_C;
    bool Stop = false;
    private bool invoked = false;

    private void Update()
    {
        if (transform.position.y <= 65.4)
        {
            Stop = true;
            Main_C.enabled = true;
        }

        if (Stop && !invoked)
        {
            Invoke("Boss_Scene", 2f);
            invoked = true;
        }

        if (Boss_C.orthographicSize <= 4 && Stop)
        {
            Boss_C.orthographicSize += 0.0016f;
        }

        if (transform.position.y < 65.6 && Stop)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.016f, transform.position.z);
        }

        if (Boss_C.rect.height > 0.45f && Stop)
        {
            Rect r = Boss_C.rect;
            r.height -= 0.0016f;
            Boss_C.rect = r;
        }

        if (Boss_C.rect.height > 0.44f && Boss_C.orthographicSize > 4)
        {
            Debug.Log("조건 만족됨 (2.5초 이내 예상)");
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y > 65.4 && !Stop)
        {
            transform.position = new Vector3(Boss_P.position.x, Boss_P.position.y - Camera_P, -10);
        }
    }

    void Boss_Scene()
    {
        SceneManager.LoadScene(3);
    }
}
