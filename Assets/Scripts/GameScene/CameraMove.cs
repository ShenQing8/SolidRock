using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float bodeHeight = 2f;
    public float MoveSpeed = 5f;
    public float RotateSpeed = 5f;

    private Vector3 TargetPos;
    private Quaternion TargetRot;

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

    // 设置跟随目标
    public void SetTarget(Transform target)
    {
        player = target;
    }
}
