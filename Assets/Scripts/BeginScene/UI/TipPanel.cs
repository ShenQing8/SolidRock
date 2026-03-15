using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TipPanel : TipPanelView
{
    private TipPanelController controller;
    private bool isInit = false; // 防重标记

    protected override void Init()
    {
        if (isInit) return;     // 如果已经初始化过，直接跳过
        isInit = true;

        controller = new TipPanelController();
        controller.Initialize(this);
    }

    public void EnsureInit()
    {
        if (!isInit)
        {
            Init();
        }
    }

    public override void ShowMe()
    {
        EnsureInit(); // 确保此时controller已经存在
        base.ShowMe();
        controller.OnShow();
    }

    public void ShowTip(string content, UnityAction confirmCallback = null)
    {
        EnsureInit(); // 确保此时controller已经存在
        controller.SetTipContent(content, confirmCallback);
    }
}