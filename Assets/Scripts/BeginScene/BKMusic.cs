using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    // 单例模式
    private static BKMusic instance;
    public static BKMusic Instance => instance;

    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        // 过场景不移除
        // DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        MusicData musicData = DataManager.Instance.musicData;
        SwitchBKMusic(musicData.musicOpen);
        AdjustBKMusicVolume(musicData.musicVolume);
    }

    // 开关背景音乐
    public void SwitchBKMusic(bool isOn)
    {
        // 静音
        audioSource.mute = !isOn;
    }
    // 调整音量
    public void AdjustBKMusicVolume(int volume)
    {
        audioSource.volume = volume / 10f;
    }
}
