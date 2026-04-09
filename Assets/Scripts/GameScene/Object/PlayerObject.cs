using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerObject : MonoBehaviour
{
    private Animator animator;
    // private int money;
    public int money;
    public float rotateSpeed = 15f; // 注意：由于现在是差值平滑旋转模型，这个值控制的是"转身"快慢（建议将旧的80改小到10~20左右）
    private PlayerControl playerControl;

    public void InitPlayerInfo(int money)
    {
        this.money = money;
        UpdateMoney();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
    }

    private void Update()
    {
        if(PauseManager.IsPaused)
            return;
        // 左alt键按下时，显示鼠标
        if(Input.GetKey(KeyCode.LeftAlt))
            return;

        /* ===== 原有代码 注释保留 开始 =====
        // 移动
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        // 旋转
        this.transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X") * rotateSpeed * Time.deltaTime);
        ===== 原有代码 注释保留 结束 ===== */


        // ===== 新的基于相机的自由运动逻辑 开始 =====
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 收集输入的移动向量（主要捕捉玩家想往哪个方向去）
        Vector3 inputDir = new Vector3(h, 0, v).normalized;
        bool hasInput = inputDir.magnitude > 0.1f;

        if (Camera.main != null && hasInput) // 只有玩家按了方向键才产生转身和位移
        {
            // 1. 获取相机当前看哪儿
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            // 抹平Y轴分量，角色只在地面平面移动转身，不往天上往地下钻
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // 2. 根据相机朝向和玩家按键(WASD)算出世界空间的真实移动方向
            Vector3 targetMoveDir = camForward * v + camRight * h;

            if (targetMoveDir != Vector3.zero)
            {
                // 3. 让角色的模型平滑地“转身”面向这个预期方向
                Quaternion targetRotation = Quaternion.LookRotation(targetMoveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime); 
            }
        }

        // 4. 将合成的输入矢量绝对长度(0~1)全部传给前进速度(VSpeed)，不再区分横向。实现“跑到哪里脸就朝向哪里”的效果
        float inputMagnitude = Mathf.Clamp01(new Vector2(h, v).magnitude);
        animator.SetFloat("HSpeed", 0f); // 锁定横向位移在动作游戏中不需要
        animator.SetFloat("VSpeed", inputMagnitude); 
        // ===== 新的基于相机的自由运动逻辑 结束 =====

        // 蹲下
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(1, 1);
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(1, 0);
        }
        // 打滚
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("TrgRoll");
        }
        // 攻击
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Fire");
        }
    }

    private void UpdateMoney()
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    public void AddMoney(int value)
    {
        money += value;
        UpdateMoney();
    }

    public void KnifeEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1, 1 << LayerMask.NameToLayer("Monster"));
        // 播放刀的音效
        DataManager.Instance.PlaySound("Music/Knife");
        foreach(var collider in colliders)
        {
            if(!collider.GetComponent<MonsterObject>().isDead)
            {
                // 得到碰撞器上的第一个怪物脚本，让其受伤
                collider.GetComponent<MonsterObject>().Wound(playerControl.atk);
                break;
            }
        }
    }

    public void ShootEvent()
    {
        // 发射子弹
        RaycastHit[] hits = Physics.RaycastAll(transform.position + transform.forward + transform.up, transform.forward, 1000, 1 << LayerMask.NameToLayer("Monster"));
        // 播放枪的音效
        DataManager.Instance.PlaySound("Music/Gun");
        foreach(var hit in hits)
        {
            if(!hit.collider.GetComponent<MonsterObject>().isDead)
            {
                // 得到碰撞器上的第一个怪物脚本，让其受伤
                hit.collider.GetComponent<MonsterObject>().Wound(playerControl.atk);
                break;
            }
        }
    }
    public void RocketEvent()
    {
        // 发射子弹
        RaycastHit[] hits = Physics.RaycastAll(transform.position + transform.forward + transform.up, transform.forward, 1000, 1 << LayerMask.NameToLayer("Monster"));
        DataManager.Instance.PlaySound("Music/Gun");
        foreach(var hit in hits)
        {
            // 得到碰撞器上的第一个怪物脚本，以此为原点，对半径3以内的怪物造成伤害
            Collider[] colliders = Physics.OverlapSphere(hit.point, 3, 1 << LayerMask.NameToLayer("Monster"));
            foreach(var collider in colliders)
            {
               collider.GetComponent<MonsterObject>().Wound(playerControl.atk);
            }
            break;
        }
    }
}
