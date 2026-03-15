using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPanelController
{
    private BeginPanelView view;
    private BeginModel model;

    public void Initialize(BeginPanelView panelView)
    {
        view = panelView;
        model = new BeginModel();

        view.InitializeView();

        view.OnStartClick += OnStartClick;
        view.OnSettingsClick += OnSettingsClick;
        view.OnAboutClick += OnAboutClick;
        view.OnExitClick += OnExitClick;
    }

    private void OnStartClick()
    {
        // 显示游戏界面
        Camera.main.GetComponent<CameraAnimator>().TurnLeft(()=>
        {
            UIManager.Instance.ShowPanel<ChoosePanel>();
        });
        // 隐藏自己
        UIManager.Instance.HidePanel<BeginPanel>(); 
    }

    private void OnSettingsClick()
    {
        // 显示设置页面
        UIManager.Instance.ShowPanel<SettingsPanel>();
    }

    private void OnAboutClick()
    {
        // 显示关于页面
    }

    private void OnExitClick()
    {
        // 退出游戏
        Application.Quit();
    }
}
