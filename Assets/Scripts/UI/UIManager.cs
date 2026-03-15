using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    // 单例模式
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;
    // 存储当前显示的面板
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    // 新创建的UI的父对象
    private Transform canvasTrans;

    private UIManager()
    {
        // 创建并得到canvas对象
        GameObject canvasPrefab = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvasPrefab.transform;
        // 使canvas过场景不删除
        GameObject.DontDestroyOnLoad(canvasPrefab);
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <returns>面板实例</returns>
    public T ShowPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if(panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        // 加载面板预制体
        GameObject panelPrefab = GameObject.Instantiate(Resources.Load<GameObject>($"UI/{panelName}"));
        // 设置面板父对象为canvasTrans
        panelPrefab.transform.SetParent(canvasTrans, false);
        // 调用面板ShowMe方法
        T panel = panelPrefab.GetComponent<T>();
        panel.ShowMe();
        // 将面板存入字典
        panelDic[panelName] = panel;
        // 返回面板
        return panel;
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="isShade">是否淡出</param>
    public void HidePanel<T>(bool isShade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        // 不存在当前面板时，直接返回
        if(!panelDic.ContainsKey(panelName))
            return;
        if(isShade)
        {
            panelDic[panelName].HideMe(()=>
            {
                // 销毁面板
                GameObject.Destroy(panelDic[panelName].gameObject);
                // 从字典中移除面板
                panelDic.Remove(panelName);
            });
        }
        else
        {
            // 销毁面板
            GameObject.Destroy(panelDic[panelName].gameObject);
            // 从字典中移除面板
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// 获取面板实例
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <returns>面板实例</returns>
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if(panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        return null;
    }
}