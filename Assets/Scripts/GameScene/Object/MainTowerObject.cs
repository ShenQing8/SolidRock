using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;

    private int hp;
    private int maxHp;
    private bool isDead = false;

    private void Awake()
    {
        instance = this;
    }

    public void Wound(int damage)
    {
        if(isDead)
            return;
        hp -= damage;
        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
            // 结束游戏
            EndPanel endPanel = UIManager.Instance.ShowPanel<EndPanel>();
            endPanel.InitInfo(false, GameLevelMge.Instance.playerObject.money / 2);
        }
        UpdateHp(hp, maxHp);
    }

    public void UpdateHp(int newHp, int newMaxHp)
    {
        hp = newHp;
        maxHp = newMaxHp;
        UIManager.Instance.GetPanel<GamePanel>().UpdateHP(hp, maxHp);
    }

    // 过场景销毁，防止占用资源
    private void OnDestroy()
    {
        instance = null;
    }
}
