using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimSetPanel : BasePanel
{
    public Slider SldX;
    public Slider SldY;
    public Button BtnBack;

    protected override void Init()
    {
        // 关闭按钮：隐藏面板后恢复游戏进程
        BtnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<AimSetPanel>(false);
            PauseManager.Resume();
        });

        SldX.onValueChanged.AddListener((x) =>
        {
            UpdateCrosshairPos();
        });

        SldY.onValueChanged.AddListener((y) =>
        {
            UpdateCrosshairPos();
        });
    }

    /// <summary>
    /// 重写BasePanel的ShowMe，让滑动条正确回显当前真正的配置
    /// </summary>
    public override void ShowMe()
    {
        base.ShowMe();
        // 面板打开时，先获取一次人物身上存放的原始偏移量
        PlayerObject player = FindObjectOfType<PlayerObject>();
        if (player != null)
        {
            SldX.value = player.crosshairOffset.x;
            SldY.value = player.crosshairOffset.y;
        }
    }

    /// <summary>
    /// 核心同步方法
    /// </summary>
    private void UpdateCrosshairPos()
    {
        float x = SldX.value;
        float y = SldY.value;

        // 调用GamePanel自身的方法移动准星的UI坐标
        GamePanel gamePanel = UIManager.Instance.GetPanel<GamePanel>();
        if (gamePanel != null)
        {
            gamePanel.UpdateAimPos(x, y);
        }

        // 同时更新后台的主角角色判定源，改变真实射击射线的落点和世界目标
        PlayerObject player = FindObjectOfType<PlayerObject>();
        if (player != null)
        {
            player.UpdateCrosshairOffset(x, y);
        }
    }
}
