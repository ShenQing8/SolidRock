using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class GameLevelMge
{
    private static GameLevelMge instance = new GameLevelMge();
    public static GameLevelMge Instance => instance;
    private GameLevelMge() { }

    // 所有出怪点
    private List<MonsterPoint> monsterPoints = new List<MonsterPoint>();
    // 最大波数
    private int maxWaves = 0;
    // 当前波数
    private int curWaves = 0;
    // 当前场景上的怪物
    private List<MonsterObject> monsters = new List<MonsterObject>();
    // 玩家信息
    public PlayerObject playerObject;

    /// <summary>
    /// 切换到游戏场景时，动态创建游戏对象
    /// </summary>
    public void Init(SceneInfo sceneInfo)
    {
        // 显示游戏界面UI
        UIManager.Instance.ShowPanel<GamePanel>();
        // 获取玩家信息
        RoleInfo roleInfo = DataManager.Instance.CurRole;
        // 查找场景中BornPos对象的位置
        Transform bornPos = GameObject.Find("BornPos").transform;
        // 实例化玩家对象
        GameObject player = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), bornPos.position, bornPos.rotation);
        // 设置相机跟随玩家
        Camera.main.GetComponent<CameraMove>().SetTarget(player.transform);
        // 为玩家添加控制脚本
        playerObject = player.AddComponent<PlayerObject>();
        playerObject.InitPlayerInfo(sceneInfo.money);
        // 设置玩家动画状态机
        player.GetComponent<PlayerControl>().UpdateWeapon(roleInfo.defaultWeaponID);
        // 设置主塔血量
        MainTowerObject.Instance.UpdateHp(sceneInfo.towerHp, sceneInfo.towerHp);
    }
    
    /// <summary>
    /// 添加出怪点
    /// </summary>
    /// <param name="monsterPoint"></param>
    public void AddMonsterPoint(MonsterPoint monsterPoint)
    {
        monsterPoints.Add(monsterPoint);
    }

    /// <summary>
    /// 更新最大波数
    /// </summary>
    /// <param name="num"></param>
    public void UpdateMaxWaveNum(int num)
    {
        maxWaves += num;
        curWaves = maxWaves;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaves(curWaves, maxWaves);
    }

    /// <summary>
    /// 更新当前波数
    /// </summary>
    /// <param name="num"></param>
    public void UpdateCurWaveNum(int num)
    {
        curWaves -= num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaves(curWaves, maxWaves);
    }

    /// <summary>
    /// 添加怪物
    /// </summary>
    /// <param name="monster"></param>
    public void AddMonster(MonsterObject monster)
    {
        monsters.Add(monster);
    }

    /// <summary>
    /// 移除怪物
    /// </summary>
    /// <param name="monster"></param>
    public void RemoveMonster(MonsterObject monster)
    {
        monsters.Remove(monster);
    }


    /// <summary>
    /// 判断游戏是否结束
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        foreach(MonsterPoint monsterPoint in monsterPoints)
        {
            if(!monsterPoint.CheckOver())
                return false;
        }
        if(monsters.Count > 0)
            return false;
        return true;
    }

    public bool CheckDistance(Vector3 pos, int range)
    {
        
        return false;
    }

    /// <summary>
    /// 寻找范围内的单个怪物
    /// </summary>
    /// <param name="pos">防御塔的位置</param>
    /// <param name="range">防御塔的攻击范围</param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 pos, int range)
    {
        foreach(MonsterObject monster in monsters)
        {
            if(!monster.isDead && Vector3.Distance(pos, monster.transform.position) <= range)
                return monster;
        }
        return null;
    }

    public List<MonsterObject> FindMonsters(Vector3 pos, int range)
    {
        List<MonsterObject> ret = new List<MonsterObject>();
        foreach(MonsterObject monster in monsters)
        {
            if(!monster.isDead && Vector3.Distance(pos, monster.transform.position) <= range)
                ret.Add(monster);
        }
        return ret;
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    public void ClearInfo()
    {
        monsterPoints.Clear();
        monsters.Clear();
        maxWaves = curWaves = 0;
    }
}
