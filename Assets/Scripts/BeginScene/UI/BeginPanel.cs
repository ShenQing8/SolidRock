using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPanel : BeginPanelView
{
    private BeginPanelController controller;

    protected override void Init()
    {
        controller = new BeginPanelController();
        controller.Initialize(this);
    }
}
