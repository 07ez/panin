using UnityEngine;
using TMPro;

public class ScreenSetting : MonoBehaviour
{
    public TextMeshPro ScreenText;
    int ScreenNum = 1;

    void Start()
    {
        InitializeScreenMode();

    }
    private void InitializeScreenMode()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen: // 보더리스 전체화면
                ScreenNum = 0;
                break;
            case FullScreenMode.Windowed: // 창 모드
                ScreenNum = 1;
                break;
            case FullScreenMode.FullScreenWindow: // 독점 전체화면
                ScreenNum = 2;
                break;
            default:
                ScreenNum = 0; // 기본값
                break;
        }
    }
    public void ChangeScreen()
    {
        ScreenNum = (ScreenNum + 1) % 3; // 0, 1, 2를 순환하도록 변경
        UpdateScreenText(); // 텍스트만 업데이트
    }

    public void Apply()
    {
        switch (ScreenNum)
        {
            case 0: // Borderless Fullscreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2: // Exclusive Fullscreen
                // BorderlessWindow는 FullScreenMode.FullScreenWindow와 동일합니다.
                // 일반적인 독점 전체화면을 원하시면 ExclusiveFullScreen을 사용하세요.
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }

    private void UpdateScreenText()
    {
        switch (ScreenNum)
        {
            case 0:
                ScreenText.text = "FullScreen";
                break;
            case 1:
                ScreenText.text = "Windowed";
                break;
            case 2:
                ScreenText.text = "BorderlessWindowed";
                break;
        }
    }
}
