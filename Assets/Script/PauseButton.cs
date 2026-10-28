using UnityEngine;
using TMPro;

public class PauseButton : MonoBehaviour
{
    [Header("Pause")]
    public TextMeshPro[] PauseobjText;                  // 멈춤 텍스트
    public TextMeshPro PauseText;                       // II 텍스트
    public GameObject Pause;                            // II 오브젝트
    public GameObject PauseObj;                         // esc, re, con 오브젝트
    string[] a = { "escape", "restart", "continue" };   // esc, re, con 검사
    bool[] b = new bool[3];
    bool Pausebool = false;                             // 멈춤버튼 상태
    public bool IsPause = false;                        // 멈춤 상태
    bool lastPause = false;                             // 멈춤 상태 체크
    public StageManager stageManager;

    public GameObject LevelSelect;

    [Header("멈출 오브젝트")]
    public GameObject Charater;
    public GameObject Charater2;

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // 버튼을 눌렀을때 
        {
            if (Pausebool) // 멈춤 버튼
            {
                PauseObj.SetActive(true); // esc, re, con 보이게 하기
                IsPause = true;           // 멈춤
            }
            if (b[0])      // escape
            {
                PauseObj.SetActive(false); // esc, re, con 안보이게
                stageManager.Escape();     // 스테이지 나가기
                IsPause = false;           // 멈춤 풀기
                AudioManager.instance.PlaySFX(AudioManager.Sfx.Choose); // SFX
            }
            else if (b[1]) // restart
            {
                PauseObj.SetActive(false);       // esc, re, con 안보이게 
                stageManager.ShowCurrentStage(); // 다시 시작
                IsPause = false;                 // 멈춤 풀기
                AudioManager.instance.PlaySFX(AudioManager.Sfx.Choose); // SFX
            }
            else if (b[2]) // continue
            {
                PauseObj.SetActive(false); // esc, re, con 안보이게
                IsPause = false;           // 멈춤 풀기
                AudioManager.instance.PlaySFX(AudioManager.Sfx.Choose); // SFX
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Pause.activeSelf) // esc키 눌렀을시
        {
            if (PauseObj.activeSelf)
            {
                PauseObj.SetActive(false); // esc, re, con 안보이게
                IsPause = false;           // 멈춤 풀기
            }
            else
            {
                PauseObj.SetActive(true); // esc, re, con 보이게 하기
                IsPause = true;           // 멈춤
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            PauseObj.SetActive(false);       // esc, re, con 안보이게 
            stageManager.ShowCurrentStage(); // 다시 시작
            IsPause = false;                 // 멈춤 풀기
            AudioManager.instance.PlaySFX(AudioManager.Sfx.Choose); // SFX
        }

        if (LevelSelect.activeSelf) // 레벨 고르는 창이면
        {
            Pause.SetActive(false); //  멈춤버튼 안보이게
        }
        else                        // 스테이지가 시작됐다면   
        {
            Pause.SetActive(true);  // 멈춤버튼 보이게
        }

        if (IsPause != lastPause)   // 멈춤상태가 갱신되었다면
        {
            Pausing();              // 멈춤관리
            lastPause = IsPause;    // 멈춤상태 저장
        }
    }

    void Pausing()
    {
        if (IsPause)
        {
            Charater.GetComponent<Rigidbody2D>().simulated = false;
            Charater2.GetComponent<Rigidbody2D>().simulated = false;

            Charater.GetComponent<Move>().enabled = false;
            Charater2.GetComponent<Move>().enabled = false;
            for (int i = 0; i < 3; i++)
            {
                PauseobjText[i].sortingOrder = 10;
            }
        }
        else
        {
            Charater.GetComponent<Rigidbody2D>().simulated = true;
            Charater2.GetComponent<Rigidbody2D>().simulated = true;

            Charater.GetComponent<Move>().enabled = true;
            Charater2.GetComponent<Move>().enabled = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other) // 마우스와 글자가 닿았다면 글자 커지게 하기
    {
        if (other.gameObject.name == "Pause")
        {
            PauseText.color = Color.yellow;
            PauseText.fontSize = 9;
            Pausebool = true;
            AudioManager.instance.PlaySFX(AudioManager.Sfx.Click); // SFX
        }
        for (int i = 0; i < a.Length; i++)
        {
            if (other.gameObject.name == a[i])
            {
                PauseobjText[i].fontSize = 7;
                PauseobjText[i].color = Color.yellow;
                b[i] = true;
                AudioManager.instance.PlaySFX(AudioManager.Sfx.Click); // SFX
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) // 마우스와 글자가 떨어졌다면
    {
        for (int i = 0; i < a.Length; i++)
        {    
            if (other.gameObject.name == "Pause")
            {
                PauseText.color = Color.black;
                PauseText.fontSize = 8;
                Pausebool = false;
            }
        }
        for (int i = 0; i < a.Length; i++)
        {
            if (other.gameObject.name == a[i])
            {
                PauseobjText[i].fontSize = 5;
                PauseobjText[i].color = Color.black;
                b[i] = false;
            }
        }
    }
}
