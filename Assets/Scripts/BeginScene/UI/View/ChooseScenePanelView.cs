using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseScenePanelView : BasePanel
{
    public Button BtnStart;
    public Button BtnBack;
    public Button BtnLeft;
    public Button BtnRight;
    public Text TextContent;
    public Image ImgScene;

    public event System.Action OnStartClick;
    public event System.Action OnBackClick;
    public event System.Action OnLeftClick;
    public event System.Action OnRightClick;

    protected override void Init() { }
    
    public void InitializeView()
    {
        BtnStart.onClick.AddListener(()=> OnStartClick?.Invoke());
        BtnBack.onClick.AddListener(()=> OnBackClick?.Invoke());
        BtnLeft.onClick.AddListener(()=> OnLeftClick?.Invoke());
        BtnRight.onClick.AddListener(()=> OnRightClick?.Invoke());
    }

    public void UpdateSceneDisplay(SceneInfo sceneInfo)
    {
        // 改预览图片
        ImgScene.sprite = Resources.Load<Sprite>(sceneInfo.imgRes);
        // 改文本内容
        TextContent.text = $"名称:\n{sceneInfo.name}\nTip:\n{sceneInfo.tips}\n塔血量:\n{sceneInfo.towerHp}\n默认金币:\n{sceneInfo.money}";
    }
}
