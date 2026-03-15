using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    // 当前塔对象
    private GameObject towerObj;
    // 当前塔的信息
    public TowerInfo nowTowerInfo;
    // 可以建造的塔的id列表
    public List<int> ids;

    public void BuildTower(int id)
    {
        TowerInfo info = DataManager.Instance.towerInfos[id];
        if(GameLevelMge.Instance.playerObject.money < info.money)
            return;
        
        // 扣钱
        GameLevelMge.Instance.playerObject.AddMoney(-info.money);
        // 更新当前塔信息
        nowTowerInfo = info;
        // 实例化塔对象
        if(towerObj != null)
        {
            Destroy(towerObj);
        }
        towerObj = Instantiate(Resources.Load<GameObject>(info.res), transform.position, Quaternion.identity);
        towerObj.GetComponent<TowerObject>().Init(info);
        // 更新当前显示的信息
        if(info.next == 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateTowerBtns(null);
        }
        else
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateTowerBtns(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(nowTowerInfo != null && nowTowerInfo.next == 0)
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerBtns(this);
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerBtns(null);
    }
}
