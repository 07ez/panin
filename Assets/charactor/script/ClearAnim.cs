using UnityEngine;
using System.Collections;

public class ClearAnim : MonoBehaviour
{
    public GameObject Charater;
    public GameObject Charater2;
    public GameObject MouseCursor;

    public StageManager stagemanager;
    public Camera cam;

    float camsize = 7.6f; 
    void Start()
    {
        cam.GetComponent<Camera>();
    }
    
    public void StageClear()
    {
        if (stagemanager.subStage != 5)
        {
            StartCoroutine(ZoomInOut());
        }
    }
    IEnumerator ZoomInOut()
    {
        // 자연스럽게 모두 비활성화
        Charater.SetActive(false);
        Charater2.SetActive(false);
        MouseCursor.SetActive(false);

        // 줌 인
        AudioManager.instance.PlaySFX(AudioManager.Sfx.NextStage1); // SFX
        for (int i = 7; i <= 100f; i ++)
        {
            cam.orthographicSize = i;
            yield return new WaitForSeconds(0.01f); // 0.01초 대기
        }

        // 줌 아웃
        AudioManager.instance.PlaySFX(AudioManager.Sfx.NextStage2); // SFX
        for (int i = 100; i >= 7; i --)
        {
            cam.orthographicSize = i;
            yield return new WaitForSeconds(0.01f); // 0.01초 대기
        }

        cam.orthographicSize = camsize; // 다시 카메라 조정

        // 다시 활성화
        Charater.SetActive(true);
        Charater2.SetActive(true);
        MouseCursor.SetActive(true);
    }
}
