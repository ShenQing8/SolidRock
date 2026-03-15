using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // 武器的父对象
    public Transform weaponPos;
    // 当前拿的武器ID
    private int weaponID;
    // 动画组件
    private Animator animator;
    // 当前武器的攻击力
    public int atk;

    void Awake()
    {
        // 得到当前身上挂载的动画组件
        animator = GetComponent<Animator>();
    }
    
    /// <summary>
    /// 更换武器
    /// </summary>
    /// <param name="id">新武器的ID</param>
    public void UpdateWeapon(int id)
    {
        // 销毁当前武器
        if(weaponPos.childCount > 0)
            Destroy(weaponPos.GetChild(0).gameObject);

        // 由新武器id来读取WeaponData中对应的武器路径
        // 进而实例化武器
        weaponID = id;

        // 获取当前武器信息
        WeaponInfo weaponInfo = DataManager.Instance.weaponInfos[id];
        // 设置攻击力
        atk = weaponInfo.atk;
        // 将实例化的武器放到weaponPos位置
        GameObject weapon = Instantiate(Resources.Load<GameObject>(weaponInfo.res), weaponPos);

        // 设置武器的位置及旋转，都为0,0,0
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localEulerAngles = Vector3.zero;

        // 加载新的动画控制器
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(weaponInfo.animator);
        // 更换当前的动画控制器
        if(controller != null && animator != null)
            animator.runtimeAnimatorController = controller;
    }
}
