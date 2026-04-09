using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pools;

public class MonsterPoint : MonoBehaviour
{
    // 剩余波数
    public int lastWaves;
    // 每波怪物有多少只
    public int monsterCountOneWave;
    // 当前波的怪物还有多少没创建
    private int lastMonsterCount;
    // 可供随机的怪物ID
    public List<int> monsterIDs;
    // 记录当前波要创建的怪物ID
    private int nowID;
    // 每两只怪物的创建间隔
    public int createOffsetTime;
    // 波与波之间的间隔时间
    public int delayTime;
    // 第一波怪物的创建时间
    public int firstDelayTime;
    // 避免高频调用 Resources.Load，并且保证 SharedGameObjectPool 获取到的是同一个对象的引用
    private Dictionary<int, GameObject> prefabCache = new Dictionary<int, GameObject>();

    void Start()
    {
        // 创建第一波怪物
        Invoke("CreatWave", firstDelayTime);
        // 将自身添加进游戏关卡管理器的出怪点列表
        GameLevelMge.Instance.AddMonsterPoint(this);
        // 更新UI显示的总波数
        GameLevelMge.Instance.UpdateMaxWaveNum(lastWaves);
    }

    private void CreatWave()
    {
        // 当前要创建的怪物ID
        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];
        // 当前波剩余的怪物数量
        lastMonsterCount = monsterCountOneWave;
        // 创建第一只怪物
        CreatMonster();
        // 波数减一
        lastWaves--;
        // 更新UI显示的当前波数
        GameLevelMge.Instance.UpdateCurWaveNum(1);
    }

    private void CreatMonster()
    {
        // 根据当前怪物ID创建怪物
        MonsterInfo monsterInfo = DataManager.Instance.monsterInfos[nowID];
        
        // =========【改造开始：用池化借出代替立刻生成】=========
        // MonsterObject monsterObject = Instantiate(Resources.Load<GameObject>(monsterInfo.res), transform.position, Quaternion.identity).AddComponent<MonsterObject>();
        
        // 1. 如果缓存字典中没有当前怪物ID的预制体资源，则去Resources查一次并存入字典
        if (!prefabCache.TryGetValue(nowID, out GameObject prefab))
        {
            prefab = Resources.Load<GameObject>(monsterInfo.res);
            prefabCache.Add(nowID, prefab);
        }

        // 2. 利用 SharedGameObjectPool 在当前位置借出一个怪物的 GameObject
        GameObject monsterGo = SharedGameObjectPool.Rent(prefab, transform.position, Quaternion.identity);

        // 3. 动态获取挂在这个游戏对象上的 MonsterObject 脚本
        if (!monsterGo.TryGetComponent<MonsterObject>(out MonsterObject monsterObject))
        {
            monsterObject = monsterGo.AddComponent<MonsterObject>();
        }
        // =================================================
        
        monsterObject.Init(monsterInfo);
        // 更新当前场景上的怪物数量
        GameLevelMge.Instance.AddMonster(monsterObject);
        
        // 当前波剩余怪物数量减一
        if(--lastMonsterCount == 0)
        {
            if(lastWaves > 0)
            {
                // 创建下一波怪物
                Invoke("CreatWave", delayTime);
            }
        }
        else
        {
            // 创建下一波怪物
            Invoke("CreatMonster", createOffsetTime);
        }
    }

    /// <summary>
    /// 检测当前出怪点是否已经没有怪物了
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return lastWaves == 0 && lastMonsterCount == 0;
    }
}
