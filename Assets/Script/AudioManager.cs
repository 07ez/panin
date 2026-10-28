using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioClip BGMClip;
    public AudioClip[] BGMClips;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] SFXClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx
    {
        Back, Choose, Click, Hit, JetPack, Jump, NextStage1, NextStage2
    }

    void Awake()
    {
        instance = this;
        Init();
    }
    
    void Init()
    {
        // 배경을 플레이어 초기화
        GameObject bgmobject = new GameObject("BGMPlayer");
        bgmobject.transform.parent = transform;
        bgmPlayer = bgmobject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = true; // 들어가자마자 실행
        bgmPlayer.loop = true; // BGM루프
        bgmVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = BGMClip;

        // 효과음 플레이어 초기화
        GameObject sfxobject = new GameObject("SFXPLayer");
        sfxobject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        sfxVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        for (int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxobject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false; // 들어가자마자 실행
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume; // AudioManager의 bgmVolume 변수 업데이트
        bgmPlayer.volume = bgmVolume; // 실제 AudioSource의 볼륨 조절
    }
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume; // AudioManager의 sfxVolume 변수 업데이트
        foreach (var player in sfxPlayers) // 모든 SFX 플레이어의 볼륨을 동일하게 설정
        {
            player.volume = sfxVolume; // 실제 AudioSource의 볼륨 조절
        }
    }

    public void PlayBGM(bool isplay, int BGMnum)
    {
        if (isplay)
        {
            bgmPlayer.clip = BGMClips[BGMnum];
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void PlaySFX(Sfx sfx)
    {
        for (int i = 0; i < channels; i++)
        {
            int loopIndex = (i + channelIndex) % channels;
            if (sfxPlayers[loopIndex].isPlaying){
                continue;
            }
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = SFXClip[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }

    }
}
