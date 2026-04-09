using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0, -5f); // z值作为相机的默认跟踪距离
    public float bodeHeight = 2f;
    public float MoveSpeed = 5f;
    public float RotateSpeed = 150f; // 修改：调大旋转速度用于匹配鼠标灵敏度

    // ===== 新增：用于自由旋转相机(轨道视角)的记录值 =====
    private float currentYaw = 0f;
    private float currentPitch = 15f; 
    [Header("相机轨道限制")]
    public float minPitch = -15f; // 防止镜头钻入地下
    public float maxPitch = 70f;  // 防止镜头越过头顶翻转
    private float currentDistance;
    // ====================================================

    private Vector3 TargetPos;
    private Quaternion TargetRot;

    void Start()
    {
        // 记录初始偏移的距离作为轨道半径
        currentDistance = Mathf.Abs(offset.z) > 0.1f ? Mathf.Abs(offset.z) : 5f;
        Vector3 angles = transform.eulerAngles;
        currentYaw = angles.y;
        currentPitch = angles.x;
    }

    /* ===== 原有代码 注释保留 开始 =====
    void Update()
    {
        if(player == null)
            return;
        TargetPos = player.position + player.forward * offset.z + player.up * offset.y + player.right * offset.x;
        // 插值运算，相机逐渐接近目标位置
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * MoveSpeed);
        // 目标角度
        TargetRot = Quaternion.LookRotation(player.position + Vector3.up * bodeHeight - transform.position);
        // 插值运算，逐渐接近目标角度
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRot, Time.deltaTime * RotateSpeed);
    }
    ===== 原有代码 注释保留 结束 ===== */


    // ===== 新的自由视角/轨道相机逻辑 开始 =====
    // 放进LateUpdate中可以确保玩家移动（Update）做完后，再更新相机，避免镜头与角色之间发生小幅抖动
    void LateUpdate()
    {
        if(player == null) return;
        
        // 只有在没按左Alt键（没显示鼠标）的时候，才允许转动视角
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            // 读取鼠标X和Y方向的输入累加到旋转角中
            currentYaw += Input.GetAxis("Mouse X") * RotateSpeed * Time.deltaTime;
            currentPitch -= Input.GetAxis("Mouse Y") * RotateSpeed * Time.deltaTime;
            // 限制俯仰角，防止穿模
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }

        // 使用欧拉角将其转换成四元数旋转
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        // 确定相机在玩家背后的反向偏移向量
        Vector3 direction = new Vector3(0, 0, -currentDistance);
        
        // 确定相机注视点（玩家脚底坐标向上偏移，也就是胸口/头部位置）
        Vector3 targetLookAt = player.position + Vector3.up * bodeHeight;

        // 计算相机的最终相对位置并赋值
        transform.position = targetLookAt + rotation * direction;
        transform.LookAt(targetLookAt); // 强行让相机盯住目标点
    }
    // ===== 新的自由视角/轨道相机逻辑 结束 =====

    // 设置跟随目标
    public void SetTarget(Transform target)
    {
        player = target;
        if(target != null)
        {
            currentYaw = target.eulerAngles.y;
        }
    }
}
