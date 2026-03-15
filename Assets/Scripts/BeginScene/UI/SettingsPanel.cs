using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : SettingsPanelView
{
    private SettingsPanelController controller;

    protected override void Init()
    {
        controller = new SettingsPanelController();
        controller.Initialize(this);
    }
}
