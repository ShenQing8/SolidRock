using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    // 当前有的钱
    public int money = 0;
    // 拥有的角色ID列表，初始化为{0，1}
    public List<int> ownedRoles = new List<int>(){0,1};
}
