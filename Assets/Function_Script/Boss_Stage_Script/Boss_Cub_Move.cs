using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class Boss_Cub_Move : MonoBehaviour
{
    [Header("땅 체크 설정")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("벽 체크 설정")]
    [SerializeField] private LayerMask wallLayer;

    [Header("움직임 설정")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float minDirChangeDelay = 1f;
    [SerializeField] private float maxDirChangeDelay = 8f;

    [Header("점프 설정")]
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private float minJumpDelay = 1f;
    [SerializeField] private float maxJumpDelay = 5f;

    public Audio_Manager_Script AMS;
    public Boss BS;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private float randomDirection = 1f;
    private float Up_Cup_Speed = 0;
    Color32 hiddenColor = new Color32(0x55, 0x00, 0xFF, 0xFF);
    Vector2 Original_Pos;

    private void Awake()
    {
        Original_Pos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (groundCheck == null)
            Debug.LogError("Boss_Cub_Move: groundCheck Transform이 할당되지 않았습니다.");
        if (groundLayer == 0)
            Debug.LogWarning("Boss_Cub_Move: groundLayer Mask가 설정되지 않았습니다.");
        if (wallLayer == 0)
            Debug.LogWarning("Boss_Cub_Move: wallLayer Mask가 설정되지 않았습니다.");

        ScheduleDirectionChange();

        StartCoroutine(RandomJumpRoutine());
    }

  
    private IEnumerator RandomJumpRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minJumpDelay, maxJumpDelay);
            yield return new WaitForSeconds(waitTime);

            if (IsGrounded())
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }


    private void ScheduleDirectionChange()
    {
        float delay = Random.Range(minDirChangeDelay, maxDirChangeDelay);
        Invoke(nameof(ChangeDirection), delay);
    }

  
    private void ChangeDirection()
    {
        randomDirection *= -1f;

        rb.velocity = new Vector2(moveSpeed * randomDirection, rb.velocity.y);
        ScheduleDirectionChange();
    }

 
    private bool IsGrounded()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
        return hit != null;
    }

    private void Update()
    {
        bool isWalking = Mathf.Abs(rb.velocity.x) > 0.1f;
        anim.SetBool("isWalking", isWalking);
        if (isWalking)
            sr.flipX = rb.velocity.x > 0;
    }

    private void FixedUpdate()
    {
        if (!isBouncing)
            rb.velocity = new Vector2((moveSpeed + Up_Cup_Speed) * randomDirection, rb.velocity.y);
    }


    private void OnCollisionEnter2D(Collision2D col)
    {

        if ((wallLayer.value & (1 << col.gameObject.layer)) != 0)
            ChangeDirection();
    }


    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }


    public void Set_Off()
    {
        gameObject.SetActive(false);
    }
    public void Set_On()
    {
        gameObject.layer = 20;
        gameObject.SetActive(true);
        transform.position = new Vector2(BS.transform.position.x, BS.transform.position.y);
    }

    public bool isBouncing = false;

    public void Bounce_Off_Cub()
    {
        var allCubs = FindObjectsOfType<Boss_Cub_Move>(false);
        foreach (var cub in allCubs)
        {
            if (!cub.gameObject.activeInHierarchy)
                continue;
            cub.gameObject.layer = LayerMask.NameToLayer("Ghost");
            cub.isBouncing = true;
            var cr = cub.GetComponent<Rigidbody2D>();
            float dir = cub.transform.position.x < BS.transform.position.x ? -1f : 1f;
            cr.AddForce(new Vector2(500 * dir, 1800), ForceMode2D.Impulse);
            cub.StartCoroutine(cub.WaitAndDisable(2f));
        }
    }

    public IEnumerator WaitAndDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        Set_Off();
        isBouncing = false;
    }

    public void Speed_Up_Cup_and_Color()
    {
        var allCubs = FindObjectsOfType<Boss_Cub_Move>(true);
        foreach (var cub in allCubs)
        {
            var rend = cub.GetComponent<SpriteRenderer>();
            if (rend != null)
            {
                rend.color = hiddenColor;
            }
            else
            {
                Debug.LogWarning($"{cub.name}에 SpriteRenderer가 없습니다!");
            }
            cub.Up_Cup_Speed = 2.5f;
        }
    }
}
