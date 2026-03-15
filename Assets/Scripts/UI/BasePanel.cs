using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 面板基类，抽象类，只能被继承，不能被实例化
public abstract class BasePanel : MonoBehaviour
{
    /// <summary>
    /// CanvasGroup组件，用于控制面板的显示与隐藏
    /// </summary>
    private CanvasGroup canvasGroup;
    /// <summary>
    /// 淡入淡出速度
    /// </summary>
    private float ShadeSpeed = 10;
    /// <summary>
    /// 面板是否正在显示
    /// </summary>
    private bool isShowing = false;
    /// <summary>
    /// 隐藏面板时的回调函数
    /// </summary>
    private UnityAction HideCallback = null;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        Init();
    }

    /// <summary>
    /// 初始化面板,start中调用
    /// </summary>
    protected abstract void Init();

    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShowing = true;
    }
    public virtual void HideMe(UnityAction callback = null)
    {
        canvasGroup.alpha = 1;
        isShowing = false;
        HideCallback = callback;
    }

    protected virtual void Update()
    {
        if(isShowing && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += ShadeSpeed * Time.unscaledDeltaTime;
            if(canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        else if(!isShowing && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= ShadeSpeed * Time.unscaledDeltaTime;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                HideCallback?.Invoke();
            }
        }
    }
}
