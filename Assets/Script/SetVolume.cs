using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioSource MgmAudioVolume;
    public AudioSource SfxAudioVolume;

    public Slider MasterSlider;
    public Slider BgmSlider;
    public Slider SfxSlider;

    private void Start()
    {
        // PlayerPrefs에서 이전 설정값 불러오기 (없으면 기본값 1)
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // 슬라이더 초기화
        MasterSlider.value = masterVolume;
        BgmSlider.value = bgmVolume;
        SfxSlider.value = sfxVolume;

        // 초기 볼륨 반영
        ApplyVolume();
    }

    /// <summary>
    /// 이전 값을 저장시켜주는 메서드
    /// </summary>
    public void AudioControl()
    {
        ApplyVolume();

        // 현재 값을 저장
        PlayerPrefs.SetFloat("MasterVolume", MasterSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", BgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SfxSlider.value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Silder으로 볼륨을 조절시켜주는 메서드
    /// </summary>
    private void ApplyVolume()
    {
        float masterVolume = MasterSlider.value;
        float bgmVolume = BgmSlider.value;
        float sfxVolume = SfxSlider.value;

        MgmAudioVolume.volume = masterVolume * bgmVolume;
        SfxAudioVolume.volume = masterVolume * sfxVolume;
    }

    private void Update()
    {
        AudioControl(); // 슬라이더 값이 변경되면 실시간 반영
    }
}