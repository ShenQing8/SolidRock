using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsModel
{
    public MusicData CurrentMusicData => DataManager.Instance.musicData;

    public void SaveMusicSettings()
    {
        DataManager.Instance.SaveMusicData();
    }

    public void UpdateMusicSwitch(bool isOn)
    {
        DataManager.Instance.musicData.musicOpen = isOn;
        BKMusic.Instance.SwitchBKMusic(isOn);
    }

    public void UpdateSoundSwitch(bool isOn)
    {
        DataManager.Instance.musicData.soundOpen = isOn;
    }

    public void UpdateMusicVolume(int volume)
    {
        DataManager.Instance.musicData.musicVolume = volume;
        BKMusic.Instance.AdjustBKMusicVolume(volume);
    }

    public void UpdateSoundVolume(int volume)
    {
        DataManager.Instance.musicData.soundVolume = volume;
    }
}
