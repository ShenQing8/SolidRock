using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseScenePanelController
{
    private ChooseScenePanelView view;
    private ChooseSceneModel model;

    public void Initialize(ChooseScenePanelView panelView)
    {
        view = panelView;
        model = new ChooseSceneModel();

        model.Initialize();
        view.InitializeView();

        // 订阅Model事件
        model.OnSceneChanged += view.UpdateSceneDisplay;

        // 订阅View事件
        view.OnLeftClick += () => model.SwitchScene(-1);
        view.OnRightClick += () => model.SwitchScene(1);
        view.OnBackClick += OnBackClick;
        view.OnStartClick += OnStartClick;

        // 初始显示
        view.UpdateSceneDisplay(model.CurrentSceneInfo);
    }

    private void OnBackClick()
    {
        // 显示选角面板
        UIManager.Instance.ShowPanel<ChoosePanel>();
        // 隐藏自己
        UIManager.Instance.HidePanel<ChooseScenePanel>();
    }

    private void OnStartClick()
    {
        // 隐藏自己
        UIManager.Instance.HidePanel<ChooseScenePanel>();
        // 异步加载，进入对应的游戏场景
        AsyncOperation ao = SceneManager.LoadSceneAsync(model.CurrentSceneInfo.sceneName);
        // 场景加载完成后进入游戏
        ao.completed += (obj) =>
        {
            // 初始化游戏关卡
            GameLevelMge.Instance.Init(model.CurrentSceneInfo);
        };
    }
}
