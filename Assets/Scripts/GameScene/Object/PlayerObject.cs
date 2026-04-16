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
    public float rotateSpeed = 15f;

    // ===== 准星偏移配置 =====
    [Header("瞄准设置")]
    public Vector2 crosshairOffset = new Vector2(140f, 140f);
    private Vector3 aimPos;
    private Vector3 aimDirection;
    private Vector3 origin;
    Vector3 screenPoint;
    // =======================

    private PlayerControl playerControl;
    
    public void InitPlayerInfo(int money)
    {
        this.money = money;
        UpdateCrosshairOffset(crosshairOffset.x, crosshairOffset.y); // 初始化时先算一次并赋值
        UpdateMoney();
    }

    /// <summary>
    /// 供外部调用的接口动态刷新十字准星偏移坐标的方法。
    /// 只有在刷新偏移参数的时候，才会重算并缓存准星（screenPoint）
    /// </summary>
    public void UpdateCrosshairOffset(float x, float y)
    {
        crosshairOffset = new Vector2(x, y);
        // 基于中心位置重算受击射线检测的原始屏幕点
        screenPoint = new Vector3(Screen.width / 2f + crosshairOffset.x, Screen.height / 2f + crosshairOffset.y, 0f);
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

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 收集输入的移动向量
        Vector3 inputDir = new Vector3(h, 0, v).normalized;
        bool hasInput = inputDir.magnitude > 0.1f;

        if (Camera.main != null && hasInput)
        {
            // 获取相机当前看哪儿
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            // 抹平Y轴分量
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // 根据相机朝向和玩家按键算出世界空间的真实移动方向
            Vector3 targetMoveDir = camForward * v + camRight * h;

            if (targetMoveDir != Vector3.zero)
            {
                // 让角色的模型平滑地转身
                Quaternion targetRotation = Quaternion.LookRotation(targetMoveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime); 
            }
        }

        // 将合成的输入矢量绝对长度(0~1)全部传给前进速度(VSpeed)，不再区分横向。
        float inputMagnitude = Mathf.Clamp01(new Vector2(h, v).magnitude);
        animator.SetFloat("VSpeed", inputMagnitude); 

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
            if (Camera.main != null)
            {
                // 基于偏移的准星计算受击点，得到真实水平朝向
                aimPos = GetCrosshairWorldTarget();
                aimDirection = aimPos - transform.position;
                
                // 忽略垂直方向，只取水平分量，防止角色因为镜头朝天上或地下而发生倾斜穿模
                aimDirection.y = 0;
                // 重新归一化向量
                aimDirection.Normalize();

                if (aimDirection != Vector3.zero)
                {
                    // 强制一帧转身
                    transform.rotation = Quaternion.LookRotation(aimDirection);
                }
            }

            // 转向完毕后，触发动画状态机的攻击
            animator.SetTrigger("Fire");
        }
    }

    /// <summary>
    /// 获取准星偏移所投射的真实世界目标点
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCrosshairWorldTarget()
    {
        if (Camera.main == null) return transform.position + transform.forward * 1000f;
        // 在屏幕中心点上加上偏移配置
        // Vector3 screenPoint = new Vector3(Screen.width / 2f + crosshairOffset.x, Screen.height / 2f + crosshairOffset.y, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        
        // 射线射向世界，判断是否命中墙体和怪物：
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            // 若命中目标或建筑，返回真正的落点
            return hit.point;
        }
        // 若朝天空射击，返回射线方向远端的极值点
        return ray.GetPoint(1000f);
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
        // 基于准星偏移的目标点，计算修正判定球位置
        aimPos = GetCrosshairWorldTarget();
        origin = transform.position + transform.up;
        aimDirection = (aimPos - origin).normalized;
        if (aimDirection == Vector3.zero) aimDirection = transform.forward;

        // 让检测球的球心按准星真正指向倾斜伸出
        Collider[] colliders = Physics.OverlapSphere(origin + aimDirection * 1f, 1, 1 << LayerMask.NameToLayer("Monster"));

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
        // 子弹射向偏移准星计算出的落点
        aimPos = GetCrosshairWorldTarget();
        origin = transform.position + transform.up;
        aimDirection = (aimPos - origin).normalized;
        if (aimDirection == Vector3.zero) aimDirection = transform.forward;
        
        RaycastHit[] hits = Physics.RaycastAll(origin, aimDirection, 1000, 1 << LayerMask.NameToLayer("Monster"));
        
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
        // 火箭弹射向偏移准星计算出的落点
        aimPos = GetCrosshairWorldTarget();
        origin = transform.position + transform.up;
        aimDirection = (aimPos - origin).normalized;
        if (aimDirection == Vector3.zero) aimDirection = transform.forward;

        RaycastHit[] hits = Physics.RaycastAll(origin, aimDirection, 1000, 1 << LayerMask.NameToLayer("Monster"));
        
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
