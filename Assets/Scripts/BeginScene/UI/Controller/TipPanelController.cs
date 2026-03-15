using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TipPanelController
{
    private TipPanelView view;
    private TipModel model;

    public void Initialize(TipPanelView panelView)
    {
        view = panelView;
        model = new TipModel();

        view.InitView();

        view.OnConfirmClick += () => CloseTip(true);
        view.OnCancelClick += () => CloseTip(false);
    }

    public void OnShow()
    {
        // 仅在游戏场景中使用暂停逻辑
        model.NeedPauseGame = UIManager.Instance.GetPanel<GamePanel>() != null;
        if(!model.NeedPauseGame)
            return;

        PauseManager.Pause();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetTipContent(string content, UnityAction confirmCallback)
    {
        model.Content = content;
        model.ConfirmCallback = confirmCallback;
        view.UpdateContent(content);
    }

    private void CloseTip(bool invokeConfirm)
    {
        if(model.NeedPauseGame)
            PauseManager.Resume();

        if(invokeConfirm)
            model.ConfirmCallback?.Invoke();
            
        if(UIManager.Instance.GetPanel<GamePanel>() != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
            
        UIManager.Instance.HidePanel<TipPanel>();
    }
}
