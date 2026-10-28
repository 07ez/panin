using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    // 컴포넌트
    Rigidbody2D rb;
    public Animator anim;
    public Animator legAnimator;
    public Animator armAnimator;

    [Header("메니저")]
    public GameObject GameManager;
    public StageManager StageManager;

    [Header("점프 설정")]
    public float jumpForce = 11f;

    [Header("Ground 체크")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("코요테 / 점프버퍼")] // 코요테: 떨어진 후 잠깐 점프 / 점프버퍼: 점프후 바로 점프 가능(미리 키 입력)
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    float coyoteCounter;
    float jumpBufferCounter;

    [Header("이동")]
    public float moveSpeed = 4f;
    float inputX;
    float inputY;
    int gravity = 1;

    [Header("비행")]
    public float flyMoveSpeed = 3f; // 비행 시 위/아래 속도

    [Header("부위들")]
    public SpriteRenderer[] spriteRenderers;
    public SpriteRenderer wingspriteRenderer;
    public SpriteRenderer jetspritRenderer;
    public GameObject Wing;
    public GameObject jetpack;
    bool GetWing = false;
    bool GetJetpack = false;
    public bool JetJump = false; // 다른 스크립트에서 건드리므로 공개함

    void Start()
    {
        // 컴포넌트 받아오기
        rb = GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 좌우 입력
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        // 바닥 감지
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        // 코요테 타이머 갱신
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        // 점프 버퍼 갱신
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // 제트팩 점프 검사
        if (GetJetpack && Input.GetKeyDown(KeyCode.W) || GetJetpack && Input.GetKeyDown(KeyCode.UpArrow))
        {
            JetJump = true;
        }
    }   
    void FixedUpdate()
    {
        // 이동 처리
        if (inputX != 0)
        {
            rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
            legAnimator.SetBool("IsMoving", true);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            legAnimator.SetBool("IsMoving", false);
        }

        // 점프
        Jump();

        // 방향 반전
        if (inputX < 0)
            SetFlipX(true);
        else if (inputX > 0)
            SetFlipX(false);

        // 애니메이션
        legAnimator.SetBool("IsMoving", inputX != 0);
        legAnimator.SetBool("IsSky", !isGrounded);
        armAnimator.SetBool("IsSky", !isGrounded);
        armAnimator.SetBool("IsJet", GetJetpack);
    }
    void OnDrawGizmosSelected() // 땅체크 디버깅
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
    void SetFlipX(bool flip) // 뒤집기
    {
        foreach (var r in spriteRenderers) // 하위 오브젝트의 spriteRenderer 모든것을 r에 담음
            r.flipX = flip;                // 좌우반전
        if (GetWing){
            wingspriteRenderer.flipX = flip;
        }
        if (GetJetpack){
            jetspritRenderer.flipX = flip;
        }
        if (flip){
            Wing.transform.localPosition = new Vector3(0.3f, Wing.transform.localPosition.y, Wing.transform.localPosition.z);
        }
        else if (!flip){
            Wing.transform.localPosition = new Vector3(-0.3f, Wing.transform.localPosition.y, Wing.transform.localPosition.z);
        }

        if (gravity == -1) // 중력이 반전됐다면
        {
            foreach (var r in spriteRenderers) // 하위 오브젝트의 spriteRenderer 모든것을 r에 담음
            {
                r.flipY = true;                // 상하반전
            }
            groundCheckPoint.transform.localPosition = new Vector3(0, 0.4f, 0);
            if (GetWing)
            {
                wingspriteRenderer.flipY= true;
            }
            if (GetJetpack)
            {
                jetspritRenderer.flipY = true;
            }
        }
        else // 중력이 그대로이라면
        {
            foreach (var r in spriteRenderers) // 하위 오브젝트의 spriteRenderer 모든것을 r에 담음
            {
                r.flipY = false;                // 상하반전
            }
            groundCheckPoint.transform.localPosition = new Vector3(0, -0.4f, 0);
            if (GetWing)
            {
                wingspriteRenderer.flipY = false;
            }
            if (GetJetpack)
            {
                jetspritRenderer.flipY = false;
            }
        }
    }
    void Jump()
    {
        // 점프 실행 조건
        if (jumpBufferCounter > 0 && coyoteCounter > 0) // 땅에서 점프하기
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * gravity);
            jumpBufferCounter = 0;
            coyoteCounter = 0;
            AudioManager.instance.PlaySFX(AudioManager.Sfx.Jump); // SFX
        }

        if (GetWing)                                    // 날개를 얻었다면
        {
            // 비행 중 중력 제거
            rb.gravityScale = 0;

            // 비행 중 상하 입력 반영
            rb.linearVelocity = new Vector2(inputX * moveSpeed, inputY * flyMoveSpeed * gravity);
        }

        if (JetJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * gravity / 2f); // 점프
            AudioManager.instance.PlaySFX(AudioManager.Sfx.Jump); // SFX
            JetJump = false;
        }
    }

    public void GoStartPos() // 시작지점 찾기 (스테이지 시작)
    {
        if (rb == null) // 왜 시발 없다고 뜨지???????? 시작할때 없다고 나와!!!!!!!!!!!!!! ㅅㅂ ㅅㅂ ㅅㅂ
        {
            rb = GetComponent<Rigidbody2D>();
        }
        // 시작지점으로 이동
        GameObject StartposObj = GameObject.Find("StartPos" + gameObject.name);
        transform.position = StartposObj.transform.position;

        // 캐릭터 초기화
        GetWing = false;          // 날개 얻음 false
        GetJetpack = false;       // 제트팩 false
        Wing.SetActive(false);    // 날개 숨김
        jetpack.SetActive(false); // 제트팩 숨김
        rb.gravityScale = 2.7f;   // 중력 그대로
        gravity = 1;              // 중력 그대로
        transform.rotation = Quaternion.Euler(0, 0, 0);

        foreach (var r in spriteRenderers) // 하위 오브젝트의 spriteRenderer 모든것을 r에 담음
        {
            r.flipY = false;                // 상하반전
        }
        groundCheckPoint.transform.localPosition = new Vector3(0, -0.4f, 0);
        if (GetWing)
        {
            wingspriteRenderer.flipY = false;
        }
        if (GetJetpack)
        {
            jetspritRenderer.flipY = false;
        }   
    }

    void OnCollisionEnter2D(Collision2D collision)     // 무언가가 캐릭터와 닿았음 
    {
        if (collision.gameObject.CompareTag("Spike"))    // 가시에 닿았다면
        {
            GetComponent<DieAnim>().Die();    // 죽음 에니메이션 스크립트
            // 중력 그대로
            rb.gravityScale = 2.7f;
            gravity = 1;

            AudioManager.instance.PlaySFX(AudioManager.Sfx.Hit); // SFX
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "End" + gameObject.name) // 골인지점과 닿았으면
        {
            StageManager.Instance.ReportGoal(gameObject.name); // 다음스테이지로
        }
        else if(collision.gameObject.name == "Wing" + gameObject.name) // 날개 오브젝트와 닿았다면
        {
            GameObject wingobj = GameObject.Find("Wing" + gameObject.name); // 날개 오브젝트 찾기
            wingobj.SetActive(false);                     // 날개 오브젝트 안보이게
            Wing.SetActive(true);                         // 캐릭터 날개 활성화 
            GetWing = true;                               // 날개 얻음을 표시  
        }
        else if(collision.gameObject.name == "jetpack" + gameObject.name)
        {
            GameObject jetpackobj = GameObject.Find("jetpack" + gameObject.name); // 제트팩 오브젝트 찾기
            jetpackobj.SetActive(false);                                          // 제트팩 오브젝트 없애기
            jetpack.SetActive(true);                                              // 캐릭터 제트팩 활성화 
            GetJetpack = true;                                                    // 제트팩 얻음을 표시
        }
        else if(collision.gameObject.name == "Reverse")
        {
            rb.gravityScale = -1 * rb.gravityScale;
            gravity = -1 * gravity;
            // 방향 반전
            SetFlipX(true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            StageManager.Instance.ReportNotGoal(gameObject.name);
        }
    }
}
