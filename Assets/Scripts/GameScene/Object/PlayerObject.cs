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
    private float rotateSpeed = 80f;
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
        // 移动
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        // 旋转
        this.transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X") * rotateSpeed * Time.deltaTime);
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
