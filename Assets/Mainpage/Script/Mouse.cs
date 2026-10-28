using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class Mouse : MonoBehaviour
{
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 이거 UI밑에 커서모양있는데 UI위에 생기게 해줘 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Rigidbody2D rb; // 컴포넌트
    public ScreenSetting screensetting;

    [Header("글자들")]
    public TextMeshPro[] texts;
    public TextMeshPro ScreenText;
    bool ScreenBool = false;

    [Header("UI (어디로 갈껀지)")]
    public GameObject[] UI;

    string[] a =
    {
        "Play",          // 0
        "Setting",       // 1
        "Credit",        // 2
        "Exit",          // 3
        "SMainBack",     // 4
        "CMainBack",     // 5
        "DeleteData",    // 6
        "Volume",        // 7
        "CSettingBack",  // 8
        "VSettingBack",  // 9
        "Video",         // 10
        "ViSettingBack", // 11
        "ViApply",       // 12
        "Charaters",     // 13
        "ChBack",        // 14
        "Yes",           // 15
        "No",            // 16
    };

    // 0 == play / 1 && 8 && 9 && 11 && 16 ==  setting / 2 == credit / 3 == exit / 4 && 5 && 14 && 15 == main / 6 == DeleteData / 7 == Volume / 12 && 16 == Video / 13 == Charater@@ 설정을 추가하면 번호를 늘리기 @@ ** 총17개 ** 
    bool[] b = new bool[17];
    int[] back = {4, 5, 8, 9, 11, 14, 16};
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();          // 컴포넌트
        Cursor.visible = false;                    // 마우스 숨기기
        for (int i = 0; i < UI.Length; i++)        // 메인페이지 빼고 다 비활성화
        {
            UI[i].SetActive(false);
        }
        UI[3].SetActive(true);
        Debug.Log(a.Length);
        Debug.Log(b.Length);
        Debug.Log(UI.Length);
        AudioManager.instance.PlayBGM(true,0); // BGM
    }
    void Update()
    {
        // 마우스
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 따라가기
        mousePosition.z = 0f; // 2D이므로 z값은 고정
        rb.MovePosition(mousePosition);

        // UI체인지
        if (Input.GetMouseButtonUp(0)) // 버튼을 눌렀을때 
        {
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i])                    // 예) 4번과 닿았었더라면
                {
                    for (int ii = 0; ii < UI.Length; ii++)
                    {
                        UI[ii].SetActive(false); // 모든 오브젝트를 끈다
                    }
                    if(i == 0)      // play
                    {
                        AudioManager.instance.PlayBGM(false, 0); // BGM
                        SceneManager.LoadScene("LevelScene");
                    }
                    else if(i == 3) // exit
                    {
                        AudioManager.instance.PlayBGM(false, 0); // BGM
                        Application.Quit(); // 종료
                    }
                    else if (i == 12) // Apply
                    {
                        b[i] = false;
                        screensetting.Apply();
                        AudioManager.instance.PlaySFX(AudioManager.Sfx.Choose);
                        UI[i].SetActive(true);
                    }
                    else if (i == 15) // 데이터 삭제
                    {
                        PlayerPrefs.DeleteAll();
                        PlayerPrefs.Save();
                        UI[i].SetActive(true);
                        b[i] = false;
                    }
                    else if (back.Contains(i))
                    {
                        UI[i].SetActive(true);   // 4번을 킨다 
                        AudioManager.instance.PlaySFX(AudioManager.Sfx.Back); // SFX
                        b[i] = false;            // 다시 4번을 비활성화 한다
                    }
                    else            // exit 제외한 모든것
                    {
                        UI[i].SetActive(true);   // 4번을 킨다 
                        AudioManager.instance.PlaySFX(AudioManager.Sfx.Choose); // SFX
                        b[i] = false;            // 다시 4번을 비활성화 한다
                    }
                }
                else if (ScreenBool) // 스크린 모드
                {
                    screensetting.ChangeScreen();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) // 마우스와 글자가 닿았다면 글자 커지게 하기
    {   
        for(int i = 0; i < a.Length; i++)
        {
            if (other.gameObject.name == a[i])  // 예) 4번과 마우스가 닿았다면 i == 4
            {
                if (i == 6) // DeleteData
                {
                    texts[i].fontSize = 7;                   // 6번의 크기를 7로
                    texts[i].color = new Color(1, 0.147f, 0);  // 6번의 폰트색을 빨간색
                }
                else
                {
                    texts[i].fontSize = 7;          // 4번의 크기를 7로
                    texts[i].color = Color.yellow;  // 4번의 폰트색을 노란색
                }
                b[i] = true;                        // 불값 4을 true로(7번과 닿았음을 알려주기)  << void update확인 ㄱㄱ
                AudioManager.instance.PlaySFX(AudioManager.Sfx.Click);
            }
            else if (other.gameObject.name == "ScreenMode")
            {
                ScreenBool = true;
                ScreenText.color = Color.yellow;
                AudioManager.instance.PlaySFX(AudioManager.Sfx.Click);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) // 마우스와 글자가 떨어졌다면
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (other.gameObject.name == a[i]) // 예) 4번과 닿았다가 떨어짐
            {
                if (i == 6) // DeleteData
                {
                    texts[i].fontSize = 5;                   // 6번의 크기를 7로
                    texts[i].color = new Color(1, 0.147f, 0);  // 6번의 폰트색을 빨간색
                }
                else 
                {
                    texts[i].fontSize = 5;         // 사이즈를 원래대로
                    texts[i].color = Color.white;  // 색을 흰색으로
                }
                b[i] = false;                  // 4번과 떨어짐
            }
            else if(other.gameObject.name == "ScreenMode")
            {
                ScreenBool = false;
                ScreenText.color = Color.white;
                ScreenText.fontSize = 5;
            }
        }
    }
}