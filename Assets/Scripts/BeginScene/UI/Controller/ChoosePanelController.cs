using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePanelController
{
    private ChoosePanelView view;
    private ChooseRoleModel model;
    
    public void Initialize(ChoosePanelView panelView)
    {
        view = panelView;
        model = new ChooseRoleModel();
        
        model.Initialize();
        view.InitializeView();
        
        // 订阅Model事件
        model.OnRoleChanged += OnRoleChanged;
        model.OnMoneyChanged += view.UpdateMoneyDisplay;
        
        // 订阅View事件
        view.OnLeftClick += () => model.SwitchRole(-1);
        view.OnRightClick += () => model.SwitchRole(1);
        view.OnBuyClick += OnBuyClick;
        view.OnStartClick += OnStartClick;
        view.OnBackClick += OnBackClick;
        
        // 初始显示
        OnRoleChanged(model.CurrentRoleInfo);
    }
    
    private void OnRoleChanged(RoleInfo roleInfo)
    {
        view.DisplayRole(roleInfo);
        bool isOwned = model.IsRoleOwned(roleInfo);
        view.UpdateBuyButton(isOwned, roleInfo.lockMoney);
    }
    
    private void OnBuyClick()
    {
        if(model.TryBuyRole())
        {
            UIManager.Instance.ShowPanel<TipPanel>().ShowTip("购买成功");
            OnRoleChanged(model.CurrentRoleInfo);
        }
        else
        {
            UIManager.Instance.ShowPanel<TipPanel>().ShowTip("金钱不足");
        }
    }
    
    private void OnStartClick()
    {
        DataManager.Instance.CurRole = model.CurrentRoleInfo;
        UIManager.Instance.ShowPanel<ChooseScenePanel>();
        UIManager.Instance.HidePanel<ChoosePanel>();
    }
    
    private void OnBackClick()
    {
        Camera.main.GetComponent<CameraAnimator>().TurnRight(()=>
        {
            UIManager.Instance.ShowPanel<BeginPanel>();
        });
        UIManager.Instance.HidePanel<ChoosePanel>();
    }
}
