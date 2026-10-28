using UnityEngine;

public class LevelMouse : MonoBehaviour
{
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 이거 UI밑에 커서모양있는데 UI위에 생기게 해줘 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    // 컴포넌트
    SpriteRenderer sr;

    [Header("살아 있는것")]
    public SpriteRenderer[] Live;

    [Header("Levels")]
    public GameObject[] Levels;
    public StageManager stageManager;

    // @@@@@@@@@@@ 스테이지 추가하면 숫자 올리기 @@@@@@@@@@ //
    string[] a =
    {
        "1Stage_Live",
        "2Stage_Live",
        "3Stage_Live",
        "4Stage_Live",
        "5Stage_Live",
        "6Stage_Live",
        "7Stage_Live",
        "8Stage_Live",
        "9Stage_Live",
        "10Stage_Live",
        "11Stage_Live",
        "12Stage_Live",
    };
    bool[] b = new bool[12];
    // @@@@@@@@@@ 스테이지 추가하면 숫자 올리기 @@@@@@@@@@ //

    float IdleTimer = 1f; // 몇초뒤에 마우스 사라지기
    float Timer = 0f;     // 타이머
    Vector3 lastMousePos; // 마우스 포지션 계산
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Cursor.visible = false;                        // 마우스 숨기기
    }
    void Update()
    {
        // 마우스
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 따라가기
        mousePosition.z = 0f; // 2D이므로 z값은 고정
        transform.position = mousePosition;

        // UI체인지
        if (Input.GetMouseButtonUp(0)) // 버튼을 눌렀을때 
        {
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i])                    // 예) 4번과 닿았었더라면
                {
                    StageManager.Instance.SelectMainStage(i + 1);   // 4번을 킨다 
                    b[i] = false;                                   // 다시 4번을 비활성화 한다
                }
            }
        }
        if (mousePosition != lastMousePos) // 움직였을시
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1); // 커서를 보이게 함
            Timer = 0f;                                                  // 타이머 리셋
        }
        else                               // 가만히 있을시
        {
            Timer += Time.deltaTime; // 타이머 늘리기
            if (Timer >= IdleTimer)  // 타이머가 1초 지났다면
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0); // 커서를 안보이게 함
            }
        }
        lastMousePos = mousePosition; // 마우스 움직이는지 계산
    }

    void OnTriggerEnter2D(Collider2D other) // 마우스와 글자가 닿았다면 글자 커지게 하기
    {   
        for(int i = 0; i < a.Length; i++)
        {
            if (other.gameObject.name == a[i])  // 예) 4번과 마우스가 닿았다면 i == 4
            {
                Live[i].transform.localScale = new Vector3(1.2f, 1.2f, 1); // 4번의 크기를 10으로
                b[i] = true;                                               // 불값 4을 true로(4번과 닿았음을 알려주기)  << void update확인 ㄱㄱ
                AudioManager.instance.PlaySFX(AudioManager.Sfx.Click); // SFX
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) // 마우스와 글자가 떨어졌다면
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (other.gameObject.name == a[i]) // 예) 4번과 닿았다가 떨어짐
            {
                Live[i].transform.localScale = new Vector3(1, 1, 1); // 크기 원래대로
                b[i] = false;                  // 4번과 떨어짐
            }
        }
    }
}