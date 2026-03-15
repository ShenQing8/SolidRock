using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Text TextHP;
    public Text TextWaves;
    public Text TextMoney;
    public Image ImgHP;
    public Button BtnBack;
    public Transform TowerParent;
    public List<TowerBtn> TowerBtns;
    private TowerPoint nowTowerPoint;
    private bool isBuilding = false;
    private bool isUIMode;
    private bool wantUIMode = false;
    private CanvasGroup canvasGroup;
    private const float HPWidth = 580;
    private const float HPHeight = 60;

    protected override void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        BtnBack.onClick.AddListener(()=>
        {
            UIManager.Instance.ShowPanel<TipPanel>().ShowTip("确定要退出游戏吗？", ()=>
            {
                // 解锁鼠标
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // 隐藏自己
                UIManager.Instance.HidePanel<GamePanel>(false);
                // 显示BeginPanel
                UIManager.Instance.ShowPanel<BeginPanel>();
                // 切换场景
                SceneManager.LoadScene("BeginScene");
            });
        });
        // 隐藏塔防建造面板
        TowerParent.gameObject.SetActive(false);
        SetUIMode(false);
    }

    private void SetUIMode(bool value)
    {
        isUIMode = value;
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        if(canvasGroup != null)
        {
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }
    }

    /// <summary>
    /// 造塔点按钮更新
    /// </summary>
    /// <param name="towerPoint">当前造塔点挂载的脚本</param>
    public void UpdateTowerBtns(TowerPoint towerPoint)
    {
        if(towerPoint == null)
        {
            TowerParent.gameObject.SetActive(false);
            isBuilding = false;
            return;
        }
        TowerParent.gameObject.SetActive(true);
        isBuilding = true;
        nowTowerPoint = towerPoint;
        TowerInfo towerInfo = towerPoint.nowTowerInfo;
        // 如果当前塔的信息为空，则可以建造3个塔
        if(towerInfo == null)
        {
            for(int i = 0; i < TowerBtns.Count; ++i)
            {
                // 激活
                TowerBtns[i].gameObject.SetActive(true);
                // 初始化塔防建造按钮
                TowerBtns[i].Init(towerPoint.ids[i], $"数字键{i + 1}");
            }
        }
        // 如果不为空，则可以升级当前塔
        else
        {
            for(int i = 0; i < TowerBtns.Count; ++i)
            {
                TowerBtns[i].gameObject.SetActive(false);
            }
            TowerBtns[1].gameObject.SetActive(true);
            TowerBtns[1].Init(towerInfo.id, "空格键");
        }
    }

    public void UpdateHP(int hp, int maxHp)
    {
        TextHP.text = $"{hp}/{maxHp}";
        ImgHP.rectTransform.sizeDelta = new Vector2(HPWidth * hp / maxHp, HPHeight);   
    }

    public void UpdateWaves(int wave, int maxWave)
    {
        TextWaves.text = $"{wave}/{maxWave}";
    }

    public void UpdateMoney(int money)
    {
        TextMoney.text = $"{money}";
    }

    protected override void Update()
    {
        base.Update();
        if(PauseManager.IsPaused)
            return;

        wantUIMode = Input.GetKey(KeyCode.LeftAlt);
        if(wantUIMode != isUIMode)
            SetUIMode(wantUIMode);
        if(isUIMode)
            return;

        if(!isBuilding)
            return;
        // 没造过塔
        if(nowTowerPoint.nowTowerInfo == null)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowTowerPoint.BuildTower(nowTowerPoint.ids[0]);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowTowerPoint.BuildTower(nowTowerPoint.ids[1]);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowTowerPoint.BuildTower(nowTowerPoint.ids[2]);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                nowTowerPoint.BuildTower(nowTowerPoint.nowTowerInfo.next);
            }
        }
    }
}
