using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterObject : MonoBehaviour
{
    private Animator animator;
    private MonsterInfo monsterInfo;
    private NavMeshAgent agent;
    private int nowhp;
    public bool isDead = false;
    private float lastAtkTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // 初始化
    public void Init(MonsterInfo info)
    {
        monsterInfo = info;
        // 设置血量
        nowhp = info.hp;
        // 设置状态机
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animatorRes);
        // 移动速度，加速度
        agent.speed = agent.acceleration = monsterInfo.moveSpeed;
        // 旋转速度
        agent.angularSpeed = monsterInfo.roundSpeed;
    }

    void Update()
    {
        if(isDead)
            return;
        // 移动速度为0时，停止移动动画
        animator.SetBool("Run", agent.velocity != Vector3.zero);
        // if(与主塔距离小于3，并且距离上次攻击时间超过攻击间隔)
        if(Vector3.Distance(transform.position, MainTowerObject.Instance.transform.position) < 3 && Time.time - lastAtkTime > monsterInfo.atkOffst)
        {
            // 攻击
            animator.SetTrigger("Attack");
            lastAtkTime = Time.time;
        }
    }

    // 出生后移动
    // 移动-寻路组件
    public void BornOver()
    {
        animator.SetBool("Run", true);
        // 设置目的地为主塔位置
        agent.SetDestination(MainTowerObject.Instance.transform.position);
    }

    // 受伤
    public void Wound(int demage)
    {
        if(isDead)
            return;
        nowhp -= demage;
        // 播放受伤动画
        animator.SetTrigger("Damage");
        // 检测死亡
        if(nowhp <= 0)
        {
            Dead();
            return;
        }
        // 播放受伤音效
        DataManager.Instance.PlaySound("Music/Wound");
    }

    // 死亡
    public void Dead()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        agent.enabled = false;
        // 播放死亡音效
        DataManager.Instance.PlaySound("Music/Dead");
        // 加钱，通过关卡管理器
        GameLevelMge.Instance.playerObject.AddMoney(monsterInfo.value);
    }

    public void DeadEvent()
    {
        // 关卡管理器使怪物数量-1
        GameLevelMge.Instance.RemoveMonster(this);
        // 销毁对象
        Destroy(gameObject);
        // 检测游戏是否结束
        if(GameLevelMge.Instance.CheckOver())
        {
            // 显示结束面板
            EndPanel endPanel = UIManager.Instance.ShowPanel<EndPanel>();
            // 初始化结束面板
            endPanel.InitInfo(true, GameLevelMge.Instance.playerObject.money);
        }
    }

    // 攻击-伤害检测
    public void AtkEvent()
    {
        // 范围检测
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1, LayerMask.GetMask("MainTower"));
        DataManager.Instance.PlaySound("Music/Eat");
        // 循环检测到的对象
        foreach(Collider collider in colliders)
        {
            // if(检测到主塔)
            if(MainTowerObject.Instance.gameObject == collider.gameObject)
            {
                // 造成伤害
                MainTowerObject.Instance.Wound(monsterInfo.atk);
            }
        }
    }
}
