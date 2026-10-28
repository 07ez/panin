using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VolumSetting : MonoBehaviour
{
    [Header("슬라이더")]
    public Slider MusicSlider;
    public Slider SoundSlider;

    [Header("글자들")]
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI soundText;

    public float musicVolume;
    public float soundVolume;
    void Start()
    {
        // 초기 설정
        musicVolume = AudioManager.instance.bgmVolume;
        soundVolume = AudioManager.instance.sfxVolume;

        MusicSlider.value = musicVolume;
        SoundSlider.value = soundVolume;

        UpdateMusicText(musicVolume);
        UpdateSoundText(soundVolume);

        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        SoundSlider.onValueChanged.AddListener(SetSoundVolume);
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume; // 현재 볼륨 값 업데이트
        UpdateMusicText(musicVolume); // 텍스트 업데이트
        AudioManager.instance.SetBGMVolume(musicVolume);

        // AudioManager를 통해 실제 음악 볼륨 조절
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSoundVolume(float volume)
    {
        soundVolume = volume; // 현재 볼륨 값 업데이트
        UpdateSoundText(soundVolume); // 텍스트 업데이트
        AudioManager.instance.SetSFXVolume(soundVolume);

        // 볼륨 값 저장
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
    }

        // 음악 볼륨 텍스트를 업데이트하는 헬퍼 함수
    private void UpdateMusicText(float volume)
    {
        // 볼륨 값을 백분율로 표시 (소수점 없이)
        musicText.text = $"Music:  {(volume * 100):F0}%";
    }

    // 효과음 볼륨 텍스트를 업데이트하는 헬퍼 함수
    private void UpdateSoundText(float volume)
    {
        // 볼륨 값을 백분율로 표시 (소수점 없이)
        soundText.text = $"Sound: {(volume * 100):F0}%";
    }

    // 스크립트가 파괴될 때 리스너를 제거하여 메모리 누수를 방지합니다.
    void OnDestroy()
    {
        if (MusicSlider != null)
        {
            MusicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        }
        if (SoundSlider != null)
        {
            SoundSlider.onValueChanged.RemoveListener(SetSoundVolume);
        }
    }
}
