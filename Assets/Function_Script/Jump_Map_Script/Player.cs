using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public CircleFadeControllerFlexible CFCF;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float LimitJumpForce = 15f;

    private float moveInput;
    private bool isCharging = false;
    private bool buttonUp = false;
    private bool isKnockedback = false;
    private float minKnockback = 50;
    private float maxKnockback = 200;
    private Rigidbody2D rigid;
    public Main_Audio_Manager MAM;

    Animator Anim;
    SpriteRenderer Sprite;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("FirstPlay"))
        {
            PlayerPrefs.DeleteAll();                 
            PlayerPrefs.SetInt("FirstPlay", 1);      
            PlayerPrefs.Save();                    
            Debug.Log("처음 실행: 모든 PlayerPrefs 초기화 완료");
        }
        if (PlayerPrefs.GetInt("IsCleared", 0) == 1)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.984f, 0.988f, 0.396f);
        }
        rigid = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
        gameObject.layer = 12;
        transform.position = new Vector2(1, -0.5f);
        rigid.freezeRotation = true;
    }
    private void Start()
    {
        CFCF.Set_On();
        CFCF.StartFade(CircleFadeControllerFlexible.FadeType.Out);
    }

    void Update()
    {
        if(transform.position.y > 2)
        {
            SceneManager.LoadScene(2);
        }
        if (transform.position.y < -20)
        {
            CFCF.StartFade(CircleFadeControllerFlexible.FadeType.In);
            StartCoroutine(Wait_Load_Scene());
        }
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            isCharging = true;
        }

        if (Input.GetButton("Jump") && isCharging)
        {
            if (jumpForce < LimitJumpForce)
            {
                jumpForce += 14f * Time.deltaTime;
            }
        }

        if (Input.GetButtonUp("Jump") && IsGrounded())
        {
            MAM.Jump_Sound();
            buttonUp = true;
            isCharging = false;
        }

        if (rigid.velocity.x != 0)
        {
            Anim.SetBool("isWalking", true);
            if (rigid.velocity.x < 0)
            {
                Sprite.flipX = true;
            }
            else
            {
                Sprite.flipX = false;
            }
        }
        else
        {
            Anim.SetBool("isWalking", false);
        }

        if ((rigid.velocity.y < -1 || rigid.velocity.y > 1) && rigid.velocity.y != 0)
        {
            Anim.SetBool("isJumping", true);
        }
        else
        {
            Anim.SetBool("isJumping", false);
        }

        if (!IsGrounded())
        {
            Anim.SetBool("isJumping", true);
        }
        else
        {
            Anim.SetBool("isJumping", false);
            if (rigid.velocity.x == 0)
            {
                Anim.Play("Player_Idle");
            }
            else
            {
                Anim.Play("Player_Run");
            }
        }

        Debug.DrawRay(new Vector2(rigid.position.x - 15, rigid.position.y + 0.8f), new Vector2(30, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(new Vector2(rigid.position.x - 15, rigid.position.y + 0.8f), Vector2.right, 30, LayerMask.GetMask("Spike"));
        if (rayHit.collider != null && rayHit.collider.tag == "Surprise")
        {
            Surprise_Spike_Script Sur_Sp = rayHit.collider.GetComponent<Surprise_Spike_Script>();

            if (Sur_Sp != null)
            {
                Sur_Sp.Surprise_Moving();
            }
        }
    }

    private IEnumerator Wait_Load_Scene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    private void FixedUpdate()
    {
        if (!isCharging && !isKnockedback)
        {
            rigid.velocity = new Vector2(moveInput * moveSpeed, rigid.velocity.y);
        }

        if (buttonUp)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            jumpForce = 3f;
            buttonUp = false;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isKnockedback = true;

            Vector2 normal = collision.GetContact(0).normal;
            Vector2 baseDir = normal;
            float strength = Random.Range(minKnockback, maxKnockback);

            rigid.AddForce(baseDir.normalized * strength, ForceMode2D.Impulse);

            StartCoroutine(EndKnockbackAfterDelay(2f));
        }
        if (collision.gameObject.layer == 9)
        {
            StartCoroutine(Change_Scene());
        }
    }

    private IEnumerator EndKnockbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isKnockedback = false;
    }

    private IEnumerator Change_Scene()
    {
        MAM.Next_sound();
        CFCF.Set_On();
        Sprite.enabled = false;
        CFCF.StartFade(CircleFadeControllerFlexible.FadeType.In);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Bose_Emergency_Scene");
    }
}
