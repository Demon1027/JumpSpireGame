using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Audio_Manager_Script AMS;
    public Win_Coin_Script WCS;
    public Hidden_Rain_Spike_Script HRSS;
    public Hidden_State_Camera_Shake HSCS;
    public Boss_Camera BC;
    public ShockWave_Script SWS;
    public Boss_Stage_Player PLAYER;
    public Game_Manager GM;
    public Mark_Emergency EM;
    public Boss_Stage_Camera BSC;
    public Boss_Stage_Crush_Spike BSCS;
    public Boss_Cub_Move BCM;
    public Example_Boss EB;
    private Color startColor;
    private Color targetColor;
    SpriteRenderer SR;
    Rigidbody2D rigid;
    Animator Anim;

    bool Start_Boss_State = false;
    bool Boss_Die_State = false;
    bool Hidden_Boss_State = false;
    bool Stop_Boss = false;
    bool ShockWave_Pattern = false;
    bool Big_Effect = false;
    bool Life_Lose_Pattering = false;
    bool Boss_Patterning = false;
    bool Camera_Effect = false;
    bool Boss_Rush_Patterning = false;
    int Rush_Pattern_Count = 0;
    int Boss_Crash_Count = 0;
    int Boss_Cup_Recall_Count = 0;
    int Boss_Pattern_Limit = 4;
    int Boss_Pattern_Count = 0;
    int Hidden_Boss_Crush_Coint = 0;
    int Hidden_Boss_Wave_count_up = 0;
    float Hidden_Boss_Wave_Time_Change = 0;
    float Boss_Rush_Pawer = 0;
    float Boss_ShockWave_Down_Speed = 0;
    float Boss_Rush_Speed = 0;
    float Boss_Speed = 3;
    float Up_Down_Camera_Effect_Scale = 0;
    float Random_Diect = 1;
    private float fadeDuration = 2f;
    Color Original_Color;
    Color CO;
    Color Current_Color;
    private void Awake()
    {
        startColor = new Color(1f, 1f, 1f, 1f);
        targetColor = new Color(1f, 0f, 0f, 1f);
        rigid = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
        CO = new Color(1f, 1f, 1f, 0.4f);
        Original_Color = SR.color;
        SR.color = startColor;
    }
    private void Start()
    {
        Stop_Boss = true;
        StartCoroutine(Start_Boss());
    }
    private IEnumerator Start_Boss()
    {
        yield return new WaitForSeconds(1f);
        Start_Boss_State = true;
        Stop_Boss = false;
        InvokeRepeating(nameof(Random_Dic), 2.5f, 2.5f);
        InvokeRepeating("Boss_Random_Pattern", 5f, 10f);
    }

    private void Update()
    {
        if (rigid.velocity.x < -0.1f || rigid.velocity.x > 0.1f)
        {
            if (rigid.velocity.x < 0)
            {
                SR.flipX = false;
            }
            else
            {
                SR.flipX = true;
            }
            Anim.SetBool("isWalking", true);
        }
        else
        {
            Anim.SetBool("isWalking", false);
        }

        if (Boss_Crash_Count == 2 + Hidden_Boss_Crush_Coint)
        {
            rigid.velocity = Vector2.zero;
            Boss_Patterning = false;
            Boss_Rush_Patterning = false;
            Boss_Crash_Count = 0;
            transform.rotation = Quaternion.identity;
        }
    }

    private void FixedUpdate()
    {
        if (Boss_Patterning == false && Stop_Boss == false)
        {
            rigid.velocity = new Vector2(Boss_Speed * Random_Diect, rigid.velocity.y);
        }
    }

    void Random_Dic()
    {
        if (Random.Range(0, 2) == 1)
        {
            Random_Diect *= -1;
        }
    }
    public void Back_Boss()
    {
        Random_Diect *= -1;
    }

    void Boss_Random_Pattern()
    {
        int Random_Int = Random.Range(1, 5);

        if (Boss_Pattern_Count == Boss_Pattern_Limit)
        {
            Boss_Life_Lose_Crash();
            return;
        }
        if(Hidden_Boss_State == true && Boss_Cup_Recall_Count == 0)
        {
            Boss_Cup_Recall_Count++;
            Boss_Cub_Recall();
            return;
        }

        if (Boss_Pattern_Count != Boss_Pattern_Limit)
        {
            switch (Random_Int)
            {
                case 1:
                    if (Rush_Pattern_Count == 1)
                    {
                        Boss_Random_Pattern();
                        Rush_Pattern_Count = 0;
                        break;
                    }
                    Boss_Rush();
                    break;
                case 2:
                    Big_Spike_Crash();
                    break;
                case 3:
                    if (Boss_Cup_Recall_Count >= 1)
                    {
                        Boss_Random_Pattern();
                        break;
                    }
                    Boss_Cub_Recall();
                    break;
                case 4:
                    Boss_Spike_Shockwave();
                    break;
            }
        }
    }

    void Boss_Rush()
    {
        Boss_Rush_Patterning = true;
        EM.Start_Emergency_Mark_Coroutine(Return_Mark_Direction());
        Debug.Log("보스 돌진 패턴 실행");
        Boss_Patterning = true;
        rigid.velocity = Vector2.zero;
        Boss_Pattern_Count++;
        StartCoroutine(Boss_Rush_Move());
        Rush_Pattern_Count = 1;
    }
    void Big_Spike_Crash()
    {
        Debug.Log("보스 가시 패턴 실행");
        Boss_Patterning = true;
        rigid.velocity = Vector2.zero;
        Boss_Pattern_Count++;
        StartCoroutine(Boss_Big_Crush_Spike());
    }
    void Boss_Cub_Recall()
    {
        EM.Start_Emergency_Mark_Coroutine(2);
        EM.Start_Emergency_Mark_Coroutine(3);
        Boss_Cup_Recall_Count++;
        Boss_Patterning = true;
        rigid.velocity = Vector2.zero;
        Debug.Log("보스 새끼 소환 패턴 실행");
        Boss_Pattern_Count++;
        StartCoroutine(Boss_Cup_Recall_Pattern());
    }
    void Boss_Spike_Shockwave()
    {
        EM.Start_Emergency_Mark_Coroutine(Return_Mark_Direction());
        Boss_Patterning = true;
        rigid.velocity = Vector2.zero;
        Debug.Log("보스 충격파 패턴 실행");
        Boss_Pattern_Count++;
        StartCoroutine(Boss_ShockWave_Spike());
    }
    void Boss_Life_Lose_Crash()
    {
        Boss_Pattern_Count = 0;
        BSC.Up_Down_Camera_Effect(0.3f, 1f);
        Boss_Patterning = true;
        rigid.velocity = Vector2.zero;
        Debug.Log("보스 채력 감소 패턴 실행");
        StartCoroutine(Boss_Life_Lose_Pattern(1.5f));
    }

    int Return_Mark_Direction()
    {
        int Dic_Int = 0;

        if (PLAYER.transform.position.y + 10 < transform.position.y)
        {
            Dic_Int = 1;
            return Dic_Int;
        }
        else if (PLAYER.transform.position.x > transform.position.x)
        {
            Dic_Int = 3;
            return Dic_Int;
        }
        else if (PLAYER.transform.position.x < transform.position.x)
        {
            Dic_Int = 2;
            return Dic_Int;
        }
        else
        {
            return 0;
        }
    }

    private IEnumerator Boss_Rush_Move()
    {
        transform.rotation = Quaternion.identity;
        rigid.position = new Vector3(rigid.position.x, rigid.position.y, 0);
        Boss_Crash_Count = 0;
        if (Return_Mark_Direction() == 3)
        {
            SR.flipX = true;
        }
        else
        {
            SR.flipX = false;
        }
        rigid.AddForce(new Vector2(0, 800), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.7f);
        Camera_Effect = true;
        rigid.AddForce(new Vector2(0, -500), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);

        if (Return_Mark_Direction() == 3)
        {
            rigid.velocity = new Vector2((20 + Boss_Rush_Speed), 0);
        }
        else if (Return_Mark_Direction() == 2)
        {
            rigid.velocity = new Vector2(-(20 + Boss_Rush_Speed), 0);
        }


    }
    private IEnumerator Boss_Life_Lose_Pattern(float Duration)
    {
        transform.rotation = Quaternion.identity;
        float elapsed = 0;
        yield return new WaitForSeconds(0.5f);
        BSC.Up_Down_Camera_Effect(0.5f, 2f);
        rigid.AddForce(new Vector2(0, 2200), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1.5f);
        rigid.constraints = RigidbodyConstraints2D.FreezePositionY;

        while (elapsed < Duration)
        {
            EM.Start_Emergency_Mark_Coroutine(Return_Mark_Direction());
            transform.position = new Vector2(PLAYER.transform.position.x, transform.position.y);
            elapsed += Time.deltaTime;
            yield return null;
        }
        gameObject.layer = 15;
        yield return new WaitForSeconds(0.5f);
        rigid.constraints = rigid.constraints & ~RigidbodyConstraints2D.FreezePositionY;
        Camera_Effect = true;
        ShockWave_Pattern = true;
        Big_Effect = true;
        rigid.AddForce(new Vector2(0, -2800f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1.5f);
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.layer = 11;
        yield return new WaitForSeconds(2f);
        Boss_Cup_Recall_Count = 0;
        Boss_Patterning = false;
        transform.rotation = Quaternion.identity;
    }
    private IEnumerator Boss_Big_Crush_Spike()
    {
        Boss_Patterning = true;
        StartCoroutine(BSC.Shake(2f, 0.3f));
        EM.Start_Emergency_Mark_Coroutine(1);
        yield return new WaitForSeconds(1.5f);
        BSCS.SetTrue(Hidden_Boss_State);
        yield return new WaitForSeconds(3f);
        Boss_Patterning = false;
    }
    private IEnumerator Boss_ShockWave_Spike()
    {
        StartCoroutine(BSC.Shake(0.2f, 0.3f));
        yield return new WaitForSeconds(0.3f);
        transform.rotation = Quaternion.identity;
        rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rigid.velocity = new Vector2(0, 0);
        for (int i = 0; i < 2 + Hidden_Boss_Wave_count_up; i++)
        {
            rigid.AddForce(new Vector2(0, 800), ForceMode2D.Impulse);
            BSC.Up_Down_Camera_Effect(0.3f, 0.6f);
            Camera_Effect = true;
            ShockWave_Pattern = true;
            yield return new WaitForSeconds(0.7f - Hidden_Boss_Wave_Time_Change);
            rigid.AddForce(new Vector2(0, -(2000 + Boss_ShockWave_Down_Speed)), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.4f - Hidden_Boss_Wave_Time_Change);
        }
        rigid.AddForce(new Vector2(0, 1500), ForceMode2D.Impulse);
        BSC.Up_Down_Camera_Effect(0.3f, 1.2f);
        Camera_Effect = true;
        ShockWave_Pattern = true;
        Big_Effect = true;
        yield return new WaitForSeconds(1f - Hidden_Boss_Wave_Time_Change);
        rigid.AddForce(new Vector2(0, -(3000 + Boss_ShockWave_Down_Speed)), ForceMode2D.Impulse);
        transform.rotation = Quaternion.identity;
        rigid.constraints = rigid.constraints & ~RigidbodyConstraints2D.FreezePositionX;
        yield return new WaitForSeconds(1f);
        Boss_Patterning = false;
    }
    private IEnumerator Boss_Cup_Recall_Pattern()
    {
        yield return new WaitForSeconds(0.5f);

        var allCubs = FindObjectsOfType<Boss_Cub_Move>(true);
        foreach (var cub in allCubs)
            cub.Set_On();

        yield return new WaitForSeconds(2f);
        Boss_Patterning = false;
    }

    public void StopAllPatternsButKeepMoving()
    {
        CancelInvoke();
        StopAllCoroutines();
        this.enabled = false;
        Stop_Boss = true;
        PLAYER.Player_Stop_On();
    }

    public void Hidden_Boss()
    {
        CancelInvoke();
        StopAllCoroutines();
        this.enabled = false;
        BSC.Set_Camera_Off();
        Stop_Boss = true;
        BC.Set_Boss_Camera_On();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(Start_Boss_Hidden_Change_Time());
    }
    private IEnumerator Start_Boss_Hidden_Change_Time()
    {
        AMS.Stop_Main_BGM();
        PLAYER.Player_Stop_On();
        yield return new WaitForSeconds(4f);
        StartFadeToRed();
        StartCoroutine(BC.Shake(2, 0.12f));
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < 4; i++)
        {
            rigid.AddForce(new Vector2(0, 1000), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.37f);
            rigid.velocity = Vector2.zero;
            rigid.AddForce(new Vector2(0, -1500), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.4f);
            rigid.velocity = Vector2.zero;
        }
        yield return new WaitForSeconds(0.5f);
        this.enabled = true;
        BC.Set_Boss_Camera_Off();
        BSC.Set_Camera_On();
        Stop_Boss = false;
        Boss_Cup_Recall_Count = 0;
        Boss_Pattern_Limit++;
        Boss_Pattern_Limit++;
        Boss_Pattern_Limit++;
        Boss_Rush_Pawer = 800;
        Boss_Speed += 4;
        Boss_Rush_Speed = 10;
        Boss_ShockWave_Down_Speed = 1200;
        Hidden_Boss_Crush_Coint = 1;
        Hidden_Boss_Wave_count_up = 1;
        Hidden_Boss_Wave_Time_Change = 0.2f;
        BCM.Speed_Up_Cup_and_Color();
        InvokeRepeating(nameof(Random_Dic), 2.5f, 2.5f);
        InvokeRepeating("Boss_Random_Pattern", 2f, 10f);
        HSCS.StartQuake(0.15f, 0.04f);
        yield return new WaitForSeconds(1f);
        Hidden_Boss_State = true;
        HRSS.Start_SpikeRespawnLoop();
        PLAYER.Player_Stop_Off();
        AMS.Start_Hidden_BGM();
    }

    public void StartFadeToRed()
    {
        StopCoroutine(nameof(FadeBackCoroutine));
        StartCoroutine(FadeColorCoroutine());
    }
    private IEnumerator FadeColorCoroutine()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;

            SR.color = Color.Lerp(startColor, targetColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        SR.color = targetColor;
    }
    public void FadeBackToWhite()
    {
        StopCoroutine(nameof(FadeBackToWhite));
        StartCoroutine(FadeBackCoroutine());
    }
    private IEnumerator FadeBackCoroutine()
    {
        float elapsed = 0f;

        Color current = SR.color;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            SR.color = Color.Lerp(current, startColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        SR.color = startColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Boss_Die_State == true && collision.gameObject.layer == 6 && Start_Boss_State == true) 
        {
            rigid.velocity = Vector2.zero;
        }
        Current_Color = SR.color;
        Current_Color = new Color(Current_Color.r, Current_Color.g, Current_Color.b, 1f);
        if (Stop_Boss == true && collision.gameObject.layer == 6 && Boss_Die_State == false && Start_Boss_State == true)
        {
            BC.Up_Down_Camera_Effect(0.5f, 1f);
            BCM.Bounce_Off_Cub();
            rigid.velocity = Vector2.zero;
        }
        if (collision.gameObject.layer == 16 && Boss_Die_State == false && Start_Boss_State == true)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            Boss_Pattern_Limit++;
            if (Hidden_Boss_State == true)
            {
                Boss_Pattern_Limit++;
            }
            SR.color = new Color(Current_Color.r, Current_Color.g, Current_Color.b, 0.5f);
            Life_Lose_Pattering = true;
            GM.Lose_Boss_Life();
            AMS.Boss_Hit_Sound_Effect();
            Camera_Effect = false;
            gameObject.layer = 11;
            rigid.velocity = Vector2.zero;
            rigid.AddForce(new Vector2(0, 600), ForceMode2D.Impulse);
            if(Return_Mark_Direction() == 2)
            {
                rigid.AddForce(new Vector2(300, 0), ForceMode2D.Impulse);
            }
            else
            {
                rigid.AddForce(new Vector2(-300, 0), ForceMode2D.Impulse);
            }
            Camera_Effect = true;
            Big_Effect = false;
            ShockWave_Pattern = false;
        }
        if (collision.gameObject.layer == 6 && Camera_Effect == true && Boss_Die_State == false && Start_Boss_State == true)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            if(gameObject.layer == 15 || Big_Effect == true)
            {
                if (Boss_Cup_Recall_Count == 1 && gameObject.layer == 15)
                {
                    BCM.Bounce_Off_Cub();
                }
                Up_Down_Camera_Effect_Scale = 1f;
            }
            if(Life_Lose_Pattering == true)
            {
                SR.color = Current_Color;
                Life_Lose_Pattering = false;
            }
            if (ShockWave_Pattern == true)
            {
                SWS.ShockWave_Spike(Big_Effect);
            }
            ShockWave_Pattern = false;
            Big_Effect = false;
            gameObject.layer = 11;
            Camera_Effect = false;
            BSC.Up_Down_Camera_Effect(0.65f, 0.8f + Up_Down_Camera_Effect_Scale);
            Up_Down_Camera_Effect_Scale = 0;
        }
        if (collision.gameObject.layer == 13 && Boss_Patterning == true && Boss_Rush_Patterning == true && Boss_Die_State == false) 
        {
            if (transform.position.x < -10)
            {
                Debug.Log("왼쪽 벽 충돌");
                rigid.velocity = Vector2.zero;
                rigid.AddForce(new Vector2(1500 + Boss_Rush_Pawer, 0), ForceMode2D.Impulse);
                BSC.Left_Camera_Effect(0.65f, 0.5f);
                Boss_Crash_Count++;
            }
            else if (transform.position.x > 18) 
            {
                Debug.Log("오른쪽 벽 충돌");
                rigid.velocity = Vector2.zero;
                rigid.AddForce(new Vector2(-(1500 + Boss_Rush_Pawer), 0), ForceMode2D.Impulse);
                BSC.Right_Camera_Effect(0.65f, 0.5f);
                Boss_Crash_Count++;
            }
        }
        transform.rotation = Quaternion.identity;
    }

    public void Boss_Die()
    {
        AMS.Stop_Hidden_BGM();
        AMS.Stop_Loop_Quake_Sound_Effect();
        Boss_Die_State = true;
        BCM.Bounce_Off_Cub();
        HSCS.StopQuake();
        CancelInvoke();
        StopAllCoroutines();
        PLAYER.Player_Stop_On();
        PLAYER.Win_Player();
        BSC.Set_Camera_Off();
        Stop_Boss = true;
        BC.Set_Boss_Camera_On();
        StartCoroutine(Boss_Die_Scene());
        HRSS.StopAllSpikes();
    }

    private IEnumerator Boss_Die_Scene()
    {
        Camera_Effect = false;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(new Vector2(0, 600), ForceMode2D.Impulse);
        if (Return_Mark_Direction() == 2)
        {
            rigid.AddForce(new Vector2(300, 0), ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(new Vector2(-300, 0), ForceMode2D.Impulse);
        }
        Camera_Effect = false;
        FadeBackToWhite();
        Start_Boss_Scale_Down();
        yield return new WaitForSeconds(2.5f);
        SR.flipX = true;
        yield return new WaitForSeconds(0.5f);
        SR.flipX = false;
        yield return new WaitForSeconds(0.5f);
        SR.flipX = true;
        yield return new WaitForSeconds(0.5f);
        SR.flipX = false;
        yield return new WaitForSeconds(0.5f);
        SR.flipX = true;
        yield return new WaitForSeconds(0.5f);
        rigid.AddForce(new Vector2(0, 600), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.6f);
        Camera_Effect = false;
        rigid.AddForce(new Vector2(0, -1000), ForceMode2D.Impulse);
        Camera_Effect = false;
        yield return new WaitForSeconds(0.7f);
        if (Return_Mark_Direction() == 2)
        {
            rigid.AddForce(new Vector2(1600, 3000), ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(new Vector2(-1600, 3000), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(1.5f);
        BC.Set_Boss_Camera_Off();
        BSC.Set_Camera_On();
        gameObject.SetActive(false);
        PLAYER.Player_Stop_Off();
        WCS.Win_Coin_Set_On();
    }

    public void Start_Boss_Scale_Down()
    {
        StartCoroutine(Boss_Scale_Down());
    }
    private IEnumerator Boss_Scale_Down()
    {
        float elapsed = 0f;
        float duration = fadeDuration;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0.5f, 0.5f, originalScale.z);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void Stop_Effect_Boss()
    {
        this.enabled = false;
    }
}
