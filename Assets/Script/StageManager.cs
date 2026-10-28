using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int mainStage = 1;  // 메인 스테이지 번호
    public int subStage = 1;   // 서브 스테이지 번호

    public GameObject[] allStages;   // 스테이지들
    public GameObject stageSelectUI; // 스테이지 고르는 창

    public GameObject Logo;
    public GameObject LogoClear;

    public Camera maincamera;

    public static StageManager Instance;
    public ClearStage clearStage;

    public ClearAnim clearAnim;

    // 캐릭터들이 골인지점에 도착했는가
    public bool IsCharaterArrived = false;
    public bool IsCharater2Arrived = false;

    public bool isStageCleared = false;

    public GameObject Character;
    public GameObject Character2;

    GameObject Button;
    GameObject Button2;
    public GameObject wingobj;
    GameObject wingobj2;
    GameObject jetobj;
    GameObject jetobj2;

    void Awake()
    {
        Instance = this;
        AudioManager.instance.PlayBGM(true, 0); // BGM
    }

    public void ChangeCameraColor(int Stage)
    {
        switch (Stage)
        {
            case 1:
                maincamera.backgroundColor = new Color32(255, 236, 189, 255);
                break;

            case 2:
                maincamera.backgroundColor = new Color32(198, 255, 184, 255);
                break;

            case 3:
                maincamera.backgroundColor = new Color32(255, 216, 184, 255);
                break;
            case 4:
                maincamera.backgroundColor = new Color32(71, 71, 71, 255);
                break;

            case 5:
                maincamera.backgroundColor = new Color32(255, 255, 255, 255);
                break;

            case 6:
                maincamera.backgroundColor = new Color32(16, 241, 255, 255);
                break;

            case 7:
                maincamera.backgroundColor = new Color32(255, 185, 141, 255);
                break;

            case 8:
                maincamera.backgroundColor = new Color32(255, 198, 198, 255);
                break;

            case 9:
                maincamera.backgroundColor = new Color32(0, 191, 255, 255);
                break;

            case 10:
                maincamera.backgroundColor = new Color32(255, 195, 0, 255);
                break;

            case 11:
                maincamera.backgroundColor = new Color32(255, 138, 240, 255);
                break;

            case 12:
                maincamera.backgroundColor = new Color32(13, 27, 42, 255);
                break;
        }
    }

    public void ReportGoal(string characterName) // 캐릭터가 도착했는지 확인
    {
        if (characterName == "Charater") IsCharaterArrived = true;
        if (characterName == "Charater2") IsCharater2Arrived = true;

        CheckStageClear(); // 스테이지 클리어 확인 (이상하게 이게 없으면 스테이지 2개씩 넘어감)
    }

    void CheckStageClear()
    {
        if (!isStageCleared && IsCharaterArrived && IsCharater2Arrived)
        {
            isStageCleared = true;
            clearAnim.StageClear();
            ClearStage(); // 스테이지 클리어
        }
    }


    public void ReportNotGoal(string characrerName) // 캐릭터가 도착지점을 나갔는지 확인
    {
        if (characrerName == "Charater") IsCharaterArrived = false;
        if (characrerName == "Charater2") IsCharater2Arrived = false;
    }

    public void ClearStage() // 스테이지 클리어
    {
        findobj();              // 스테이지 리셋
        wingobj = null;
        wingobj2 = null;
        Button = null;
        Button2 = null;
        isStageCleared = false; // 스테이지 클리어 끄기
        if (mainStage == 12 && subStage == 5) // 모든 스테이지를 클리어했다면
        {
            ShowStageSelect();
            Logo.SetActive(false);
            LogoClear.SetActive(true);
            PlayerPrefs.SetInt("ClearedStage", 12);
            PlayerPrefs.Save();
            Debug.Log("AllStageClear");
        }
        else if (subStage < 5) // 서브 스테이지가 5 미만이면 (총5개있음)
        {
            subStage++;         // 다음 서브스테이지로
            ShowCurrentStage(); // 다음 서브스테이지 보여주기
        }
        else              // 스테이지를 클리어했다면
        {
            ShowStageSelect();  // 스테이지 고르기
            clearStage.Clear(mainStage);
        }
    }

    public void ShowCurrentStage() // 다음스테이지 보여주기
    {
        DeactivateAllStages();                                          // 모든스테이지 비활성화
        int index = (mainStage - 1) * 5 + (subStage - 1);               // 활성화할 스테이지
        allStages[index].SetActive(true);                               // 스테이지 활성화
        stageSelectUI.SetActive(false);                                 // 스테이지 고르는창 비활성화

        ChangeCameraColor(mainStage);

        // 캐릭터 시작위치로 이동
        Character.GetComponent<Move>().GoStartPos(); 
        Character2.GetComponent<Move>().GoStartPos();

        findobj();
    }

    public void findobj()
    {
        if (GameObject.Find("WingCharater")) // 날개가 게임안에 있다면
        {
            wingobj = GameObject.Find("WingCharater"); // 날개를 저장
        }
        else if (wingobj != null && !wingobj.activeSelf) // 날개가 스테이지안에 있고, 윙 오브젝트가 활성화 돼있지 않다면 
        {
            wingobj.SetActive(true); // 윙 오브젝트를 다시 킨다
        }

        if (GameObject.Find("WingCharater2")) // 날개가 게임안에 있다면
        {
            wingobj2 = GameObject.Find("WingCharater2"); // 날개를 저장
        }
        else if(wingobj2 != null && !wingobj2.activeSelf) // 날개가 스테이지안에 있고, 윙 오브젝트가 활성화 돼있지 않다면 
        {
            wingobj2.SetActive(true); // 윙 오브젝트를 다시 킨다

        }

        if (GameObject.Find("Button")) // 위와 동일
        {
            Button = GameObject.Find("Button");
        }
        else if(Button != null && !Button.activeSelf)
        {
            Button.GetComponent<ChangePlatform>().ResetMap();
        }

        if (GameObject.Find("Button2")) // 위와 동일
        {
            Button2 = GameObject.Find("Button2");
        }
        else if(Button2 != null && !Button2.activeSelf)
        {
            Button2.GetComponent<ChangePlatform>().ResetMap();
        }

        if (GameObject.Find("jetpackCharater")) // 위와 동일
        {
            jetobj = GameObject.Find("jetpackCharater");
        }
        else if(jetobj != null && !jetobj.activeSelf)
        {
            jetobj.SetActive(true);
        }

        if (GameObject.Find("jetpackCharater2")) // 위와 동일
        {
            jetobj2 = GameObject.Find("jetpackCharater2");
        }
        else if(jetobj2 != null && !jetobj2.activeSelf)
        {
            jetobj2.SetActive(true);
        }
    }

    public void ShowStageSelect() // 스테이지 고르는창 보여주기
    {
        DeactivateAllStages();          // 모든스테이지 비활성화

        // 캐릭터 비활성화
        Character.SetActive(false);
        Character2.SetActive(false);

        stageSelectUI.SetActive(true);  // 스테이지 고르는창 활성화

        maincamera.backgroundColor = new Color32(71, 71, 71, 255);

        AudioManager.instance.PlayBGM(true, 0); // BGM
    }

    public void SelectMainStage(int stageNum) // 메인스테이지 고르기
    {
        // 캐릭터 활성화
        Character.SetActive(true);
        Character2.SetActive(true);

        mainStage = stageNum; // 메인 스테이지 번호 저장
        subStage = 1;         // 서브 스테이지를 1로변경
        ShowCurrentStage();   // 스테이지 보여주기

        int randomInt = Random.Range(0, 3);             // 랜덤으로 BGM 정함
        AudioManager.instance.PlayBGM(true, randomInt); // 그 BGM을 실행
    }

    void DeactivateAllStages() // 모든 스테이지 비활성화
    {
        foreach (GameObject stage in allStages)
        {
            stage.SetActive(false);
        }
    }
    public void Escape() // 레벨고르는 곳으로 돌아가기
    {
        Character.SetActive(false);
        Character2.SetActive(false);
        findobj();
        DeactivateAllStages();
        stageSelectUI.SetActive(true);
        maincamera.backgroundColor = new Color32(71, 71, 71, 255);

        AudioManager.instance.PlayBGM(true, 0);
    }
}
