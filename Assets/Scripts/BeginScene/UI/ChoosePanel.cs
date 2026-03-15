using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoosePanel : ChoosePanelView
{
    private ChoosePanelController controller;
    
    protected override void Init()
    {
        controller = new ChoosePanelController();
        controller.Initialize(this);
    }
}
