using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndPanel : BasePanel
{
    public Text TextIsWin;
    public Text TextContent;
    public Text TextMoney;
    public Button BtnSure;

    protected override void Init()
    {
        BtnSure.onClick.AddListener(() =>
        {
            PauseManager.Resume();
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.HidePanel<EndPanel>();
            // 清空场景数据
            GameLevelMge.Instance.ClearInfo();
            // 返回主界面
           SceneManager.LoadScene("BeginScene");
        });
    }

    public void InitInfo(bool isWin, int money)
    {
        TextIsWin.text = isWin ? "Victory" : "Defeat";
        TextContent.text = isWin ? "获得胜利奖励" : "获得失败补偿";
        TextMoney.text = $"￥{money}";

        DataManager.Instance.playerData.money += money;
        DataManager.Instance.SavePlayerData();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        PauseManager.Pause();
        // 解锁鼠标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
