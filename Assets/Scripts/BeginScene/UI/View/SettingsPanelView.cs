using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelView : BasePanel
{
    public Button BtnClose;
    public Toggle TogMusic;
    public Toggle TogSound;
    public Slider SldMusic;
    public Slider SldSound;

    public event System.Action OnCloseClick;
    public event System.Action<bool> OnMusicToggleChanged;
    public event System.Action<bool> OnSoundToggleChanged;
    public event System.Action<float> OnMusicVolumeChanged;
    public event System.Action<float> OnSoundVolumeChanged;

    protected override void Init() { }

    public void InitializeView()
    {
        BtnClose.onClick.AddListener(()=> OnCloseClick?.Invoke());
        TogMusic.onValueChanged.AddListener((isOn)=> OnMusicToggleChanged?.Invoke(isOn));
        TogSound.onValueChanged.AddListener((isOn)=> OnSoundToggleChanged?.Invoke(isOn));
        SldMusic.onValueChanged.AddListener((value)=> OnMusicVolumeChanged?.Invoke(value));
        SldSound.onValueChanged.AddListener((value)=> OnSoundVolumeChanged?.Invoke(value));
    }

    public void DisplaySettings(MusicData musicData)
    {
        TogMusic.isOn = musicData.musicOpen;
        TogSound.isOn = musicData.soundOpen;
        SldMusic.value = musicData.musicVolume;
        SldSound.value = musicData.soundVolume;
    }
}
