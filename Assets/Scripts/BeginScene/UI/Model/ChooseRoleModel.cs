using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRoleModel
{
    private int curRoleIndex = 0;
    private RoleInfo curRoleInfo;
    public int CurrentRoleIndex => curRoleIndex;
    public RoleInfo CurrentRoleInfo => curRoleInfo;
    
    public event System.Action<RoleInfo> OnRoleChanged;
    public event System.Action<int> OnMoneyChanged;
    
    public void Initialize()
    {
        curRoleIndex = 0;
        UpdateCurrentRole();
    }
    
    public void SwitchRole(int direction) // direction: -1 左, 1 右
    {
        curRoleIndex += direction;
        int count = DataManager.Instance.roleInfos.Count;
        if(curRoleIndex < 0) curRoleIndex = count - 1;
        if(curRoleIndex >= count) curRoleIndex = 0;
        UpdateCurrentRole();
    }
    
    public bool TryBuyRole()
    {
        if(DataManager.Instance.playerData.money >= curRoleInfo.lockMoney)
        {
            DataManager.Instance.playerData.money -= curRoleInfo.lockMoney;
            DataManager.Instance.playerData.ownedRoles.Add(curRoleInfo.id);
            DataManager.Instance.SavePlayerData();
            OnMoneyChanged?.Invoke(DataManager.Instance.playerData.money);
            return true;
        }
        return false;
    }
    
    public bool IsRoleOwned(RoleInfo role)
    {
        return DataManager.Instance.playerData.ownedRoles.Contains(role.id);
    }
    
    private void UpdateCurrentRole()
    {
        curRoleInfo = DataManager.Instance.roleInfos[curRoleIndex];
        OnRoleChanged?.Invoke(curRoleInfo);
    }
}
