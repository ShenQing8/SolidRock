using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSceneModel
{
    private int curSceneIndex = 0;
    private SceneInfo curSceneInfo;

    public int CurrentSceneIndex => curSceneIndex;
    public SceneInfo CurrentSceneInfo => curSceneInfo;

    public event System.Action<SceneInfo> OnSceneChanged;

    public void Initialize()
    {
        curSceneIndex = 0;
        UpdateCurrentScene();
    }

    /// <summary>
    /// 切换场景，direction: -1 左, 1 右
    /// </summary>
    /// <param name="direction">方向</param>
    public void SwitchScene(int direction)
    {
        curSceneIndex += direction;
        int count = DataManager.Instance.sceneInfos.Count;
        
        if(curSceneIndex < 0) curSceneIndex = count - 1;
        if(curSceneIndex >= count) curSceneIndex = 0;
        
        UpdateCurrentScene();
    }

    private void UpdateCurrentScene()
    {
        curSceneInfo = DataManager.Instance.sceneInfos[curSceneIndex];
        OnSceneChanged?.Invoke(curSceneInfo);
    }
}
