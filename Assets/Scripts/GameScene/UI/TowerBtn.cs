using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    public Image ImgTower;
    public Text TextPrice;
    public Text TextControlBtn;

    public void Init(int id, string inputStr)
    {
        TowerInfo info = DataManager.Instance.towerInfos[id];
        ImgTower.sprite = Resources.Load<Sprite>(info.imgRes);
        TextPrice.text = $"￥{info.money}";
        TextControlBtn.text = inputStr;
        // 如果钱不够，价格显示红色
        if(GameLevelMge.Instance.playerObject.money < info.money)
        {
            TextPrice.color = Color.red;
        }
    }
}
