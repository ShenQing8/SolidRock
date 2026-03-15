using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    private UnityAction TurnOverAction;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TurnLeft(UnityAction act)
    {
        animator.SetBool("IsBegin", true);
        TurnOverAction = act;
    }

    public void TurnRight(UnityAction act)
    {
        animator.SetBool("IsBegin", false);
        TurnOverAction = act;
    }

    // 为什么private也能被Animator调用？因为是通过反射调用的
    private void TurnOver()
    {
        TurnOverAction?.Invoke();
        TurnOverAction = null;
    }
}
