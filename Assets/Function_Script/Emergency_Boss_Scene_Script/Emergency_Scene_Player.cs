using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emergency_Scene_Player : MonoBehaviour
{
    public Sound_Manager_Script_Emergency SMSE;
    public CircleFadeControllerFlexible CFCF;
    public GameObject GB2;
    public GameObject GB1;
    Rigidbody2D rigid;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("IsCleared", 0) == 1)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.984f, 0.988f, 0.396f);
        }
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        CFCF.Set_On();
        CFCF.StartFade(CircleFadeControllerFlexible.FadeType.Out);
        StartCoroutine(Start_Emergency_Scene_Player());
    }
    private IEnumerator Start_Emergency_Scene_Player()
    {
        yield return new WaitForSeconds(2.1f);
        SMSE.Jump_Sound_Effect();
        rigid.AddForce(new Vector2(0, 120), ForceMode2D.Impulse);
        Invoke("SetActive_GB", 0.8f);
    }

    public void Boss_Impact()
    {
        SMSE.Crush_Sound_Effect();
        rigid.AddForce(new Vector2(-80, 100), ForceMode2D.Impulse);
    }

    void SetActive_GB()
    {
        GB1.SetActive(true);
        GB2.SetActive(true);
    }
}
