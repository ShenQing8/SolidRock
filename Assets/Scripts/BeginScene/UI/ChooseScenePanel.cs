using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseScenePanel : ChooseScenePanelView
{
    private ChooseScenePanelController controller;

    protected override void Init()
    {
        controller = new ChooseScenePanelController();
        controller.Initialize(this);
    }
}
