using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    // 炮台头部，用来旋转
    public Transform head;
    // 开火点
    public Transform firePoint;
    // 旋转速度
    private int rotateSpeed = 20;
    // 关联的数据
    private TowerInfo info;
    // 攻击的目标
    private MonsterObject target;
    private List<MonsterObject> targets;
    // 记录时间
    private float nowTime = 0;
    // 怪物位置
    private Vector3 targetPos;

    public void Init(TowerInfo info)
    {
        this.info = info;
    }

    // Update is called once per frame
    void Update()
    {
        if(info.atkType == 1)// 单体攻击
        {
            // 如果没有目标，或者目标死亡，或者目标超出攻击范围，则重新寻找目标
            if(target == null || target.isDead || Vector3.Distance(transform.position, target.transform.position) > info.atkRange)
            {
                target = GameLevelMge.Instance.FindMonster(transform.position, info.atkRange);
            }
            // 如果没有找到攻击目标，直接返回
            if(target == null) 
                return;
            targetPos = target.transform.position;
            targetPos.y = head.position.y;
            head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(targetPos - head.position), Time.deltaTime * rotateSpeed);
            // 计算夹角，并判断攻击间隔
            if(Vector3.Angle(head.forward, targetPos - head.position) < 5 && Time.time - nowTime > info.offsetTime)
            {
                // 怪物受伤
                target.Wound(info.atk);
                // 播放攻击音效
                DataManager.Instance.PlaySound(info.soundRes);
                // 创建开火特效
                GameObject eff = Instantiate(Resources.Load<GameObject>(info.effRes), firePoint.position, firePoint.rotation);
                // 延迟移除特效
                Destroy(eff, 0.2f);
                nowTime = Time.time;
            }
        }   
        else// 群体攻击
        {
            // 控制攻击间隔
            if(Time.time - nowTime < info.offsetTime)
                return;
            nowTime = Time.time;
            // 找目标
            targets = GameLevelMge.Instance.FindMonsters(transform.position, info.atkRange);
            if(targets.Count == 0) 
                return;
            // 怪物受伤
            foreach(MonsterObject monster in targets)
            {
                monster.Wound(info.atk);
            }
            // 播放攻击音效
            DataManager.Instance.PlaySound(info.soundRes);
            // 创建开火特效，并延迟移除特效
            Destroy(Instantiate(Resources.Load<GameObject>(info.effRes), firePoint.position, firePoint.rotation), 0.2f);
        }
    }
}
