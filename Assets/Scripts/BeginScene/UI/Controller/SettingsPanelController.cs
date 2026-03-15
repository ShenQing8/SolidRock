using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelController
{
    private SettingsPanelView view;
    private SettingsModel model;

    public void Initialize(SettingsPanelView panelView)
    {
        view = panelView;
        model = new SettingsModel();

        // 将初始数据传递给View层显示
        view.DisplaySettings(model.CurrentMusicData);
        
        view.InitializeView();

        // 订阅View层事件，由Model层处理业务
        view.OnCloseClick += OnCloseClick;
        view.OnMusicToggleChanged += (isOn) => model.UpdateMusicSwitch(isOn);
        view.OnSoundToggleChanged += (isOn) => model.UpdateSoundSwitch(isOn);
        view.OnMusicVolumeChanged += (value) => model.UpdateMusicVolume((int)value);
        view.OnSoundVolumeChanged += (value) => model.UpdateSoundVolume((int)value);
    }

    private void OnCloseClick()
    {
        // 为了节约性能，只有在关闭设置面板时才保存数据
        model.SaveMusicSettings();
        // 隐藏设置页面
        UIManager.Instance.HidePanel<SettingsPanel>();
    }
}
