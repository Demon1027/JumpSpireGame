using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Boss_Stage_Player : MonoBehaviour
{

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.1f;

    public Audio_Manager_Script AMS;
    public CircleFadeControllerFlexible CFCF;
    public Infinity_Coin_Script ICS;
    public Boss BS;
    public Boss_Stage_Camera BSC;
    public Boss_Cub_Move BCM;
    public Game_Manager GM;
    public SpriteRenderer SR;
    Rigidbody2D rigid;
    public Animator Anim;

    float PosX;
    float Speed = 4f;
    float Double_Jump_Count = 0;
    bool DownY;

    bool Player_Stop = false;
    bool Player_Die = false;
    bool Player_Damaged = false;
    Color targetColor;
    Color startColor;
    Color Original_Color;
    Color CO;
    Color32 Infinity_Color = new Color32(0x00, 0xDB, 0x7D, 0xFF);
    private void Awake()
    {
        if (PlayerPrefs.GetInt("IsCleared", 0) == 1)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.984f, 0.988f, 0.396f);
        }
        gameObject.layer = 12;
        transform.position = new Vector3(-4.46f, 63.46f, -1);
        targetColor = new Color(248f / 255f, 250f / 255f, 3f / 255f, 1f);
        startColor = SR.color;
        rigid = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        CO = SR.color;
        Original_Color = SR.color;
    }

    private void Start()
    {
        Player_Die = false;
    }
    private void Update()
    {
        if(transform.position.y < 45)
        {
            CFCF.StartFade(CircleFadeControllerFlexible.FadeType.In);
            StartCoroutine(Wait_Load_Scene());
        }
        if(Player_Die == true)
        {
            rigid.position = new Vector2(rigid.position.x, rigid.position.y);
        }

        PosX = Input.GetAxisRaw("Horizontal");
        DownY = Input.GetKey(KeyCode.DownArrow);
        if (Input.GetButtonDown("Jump") && IsGrounded() && Player_Die == false && Player_Stop == false || Input.GetButtonDown("Jump") && Double_Jump_Count == 1 && Player_Die == false && Player_Stop == false) 
        {
            AMS.Jump_Sound_Effect();
            rigid.AddForce(new Vector2(0, 100), ForceMode2D.Impulse);      
            Double_Jump_Count = 0;
        }

        if(rigid.velocity.x < -0.1f || rigid.velocity.x > 0.1f)
        {
            if(rigid.velocity.x < 0)
            {
                SR.flipX = true;
            }
            else
            {
                SR.flipX = false;
            }
            Anim.SetBool("isWalking", true);
        }
        else
        {
            Anim.SetBool("isWalking", false);
        }

        if (!IsGrounded())
        {
            Anim.SetBool("isJumping", true);
        }
        else
        {
            Anim.SetBool("isJumping", false);
            if(rigid.velocity.x == 0)
            {
                Anim.Play("Player_Idle");
            }
            else
            {
                Anim.Play("Player_Run");
            }
        }
    }

    private void FixedUpdate()
    {
        if (Player_Damaged == false && Player_Die == false && Player_Stop == false) 
        {
            rigid.velocity = new Vector2(PosX * Speed, rigid.velocity.y);
        }

        if (Player_Damaged == false && Player_Die == false && Player_Stop == false && DownY == true)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -10);
        }
    }
    private IEnumerator Wait_Load_Scene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }

    public void Set_Double_Jump_Count()
    {
        Double_Jump_Count = 1;
    }
    public void Set_Double_Jump_Count_Reset()
    {
        Double_Jump_Count = 0;
    }

    public void WaitTime()
    {
        Invoke("Set_Double_Jump_Count_Reset", 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 21)
        {
            AMS.Coin_Sound_Effect();
            var portals = GameObject.FindGameObjectsWithTag("Coin_Portal");

            foreach (var p in portals)
                p.SetActive(false);

            if(transform.position.x < -9.5)
            {
                transform.position = new Vector2(19.5f, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(-11.5f, transform.position.y);
            }

            StartCoroutine(Portal_On_Time(portals));
        }
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 11 || collision.gameObject.layer == 17 || collision.gameObject.layer == 20)
        {
            Player_Damaged = true;
            gameObject.layer = 14;
            GM.Lose_Player_Life();
            AMS.Player_Hit_Sound_Effect();
            CO.a = 0.5f;
            SR.color = CO;

            Vector2 normal = collision.GetContact(0).normal;
            rigid.velocity = Vector2.zero;
            rigid.AddForce(normal * 50, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.up * 40, ForceMode2D.Impulse);

            Invoke("Invicibility_Time", 2.5f);
            StartCoroutine(EndKnockbackAfterDelay(0.7f));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 19 && rigid.velocity.y < 0)
        {
            rigid.AddForce(Vector2.up * 150, ForceMode2D.Impulse);
            var cub = collision.GetComponentInParent<Boss_Cub_Move>();
            if (cub != null)
            {
                AMS.Boss_Cub_Die_Sound_Effect();
                cub.Set_Off();
            }
        }
        if(collision.gameObject.tag == "Infinity_Coin")
        {
            AMS.Coin_Sound_Effect();
            ICS.Set_Off();
            SR.color = Infinity_Color;
            gameObject.layer = 14;
            StartCoroutine(Wait_Infinity_Coin_Recall());
        }
    }
    private IEnumerator Wait_Infinity_Coin_Recall()
    {
        yield return new WaitForSeconds(5);
        gameObject.layer = 12;
        SR.color = Original_Color;
        yield return new WaitForSeconds(40);
        ICS.Set_On();
    }
    private IEnumerator Portal_On_Time(GameObject[] portals)
    {
        gameObject.layer = 14;
        CO.a = 0.5f;
        SR.color = CO;
        yield return new WaitForSeconds(1);
        gameObject.layer = 12;
        CO.a = 1;
        SR.color = CO;
        yield return new WaitForSeconds(8);

        foreach (var p in portals)
            p.SetActive(true);
    }
    private IEnumerator EndKnockbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Player_Damaged = false;
    }

    void Invicibility_Time()
    {
        if (Player_Die == false)
        {
            CO.a = 1;
            SR.color = CO;
            gameObject.layer = 12;
        }
    }

    public void Player_Die_()
    {
        AMS.Stop_Main_BGM();
        AMS.Stop_Hidden_BGM();
        Debug.Log("플레이어가 사망했습니다.");
        Player_Die = true;
        SR.flipY = true;
        gameObject.layer = 18;
        BS.StopAllPatternsButKeepMoving();
        StartCoroutine(FreezeAfterDelay(0.4f));
    }
    public void Boss_Die()
    {
        gameObject.layer = 14;
    }

    private IEnumerator FreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        rigid.velocity = Vector2.zero;

        rigid.gravityScale = 0f;

        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        AMS.GameOver_Sound_Effect();
        yield return new WaitForSeconds(0.5f);
        CFCF.Set_On();
        CFCF.StartFade(CircleFadeControllerFlexible.FadeType.In);
        yield return new WaitForSeconds(2.4f);
        SceneManager.LoadScene(1);
    }

    private IEnumerator Wait_Time()
    {
        yield return new WaitForSeconds(1f);

        rigid.velocity = Vector2.zero;
    }

    public void Player_Stop_On()
    {
        rigid.velocity = Vector2.zero;
        Player_Stop = true;
    }
    
    public void Player_Stop_Off()
    {
        Player_Stop = false;
    }

    public void Win_Player()
    {
        gameObject.layer = 18;
    }

    public void Win_Player_Scene()
    {
        BSC.Set_Camera_Pos_Stop();
        Player_Stop_On();
        StartCoroutine(Change_Color());
        StartCoroutine(Scale_Up());
        StartCoroutine(Final_Player_Out_Scene());
    }
    private IEnumerator Scale_Up()
    {
        float elapsed = 0f;
        float scaleSpeed = 1.6f;

        while (elapsed < 4)
        {
            float delta = scaleSpeed * Time.deltaTime;
            transform.localScale += new Vector3(delta, delta, 0f);
            if (transform.localScale.x > 5.5f)
                break;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator Change_Color()
    {
        float elapsed = 0f;
        while (elapsed < 4)
        {
            float t = elapsed / 4;

            SR.color = Color.Lerp(startColor, targetColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        SR.color = targetColor;
    }
    private IEnumerator Final_Player_Out_Scene()
    {
        yield return new WaitForSeconds(5.5f);
        rigid.AddForce(new Vector2(0, 500), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.15f);
        AMS.Clear_Sound_Effect();
        CFCF.Set_On();
        CFCF.StartFade(CircleFadeControllerFlexible.FadeType.In);
        PlayerPrefs.SetInt("IsCleared", 1);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(4);
        gameObject.SetActive(false);
    }
}
