using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    // 单例模式
    private static DataManager instance = new DataManager();
    public static DataManager Instance => instance;

    // 音乐数据
    public MusicData musicData;
    // 角色数据
    public List<RoleInfo> roleInfos;
    // 玩家数据
    public PlayerData playerData;
    // 当前角色
    public RoleInfo CurRole;
    // 武器数据
    public List<WeaponInfo> weaponInfos;
    // 场景数据
    public List<SceneInfo> sceneInfos;
    // 怪物数据
    public List<MonsterInfo> monsterInfos;
    // 防御塔数据
    public List<TowerInfo> towerInfos;
    
    private DataManager()
    {
        // 初始化音乐数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        // 初始化角色数据
        roleInfos = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        // 重新保存数据，用来清空之前测试时产生的数据
        // playerData = new PlayerData();
        // SavePlayerData();
        // 初始化玩家数据
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        // 初始化场景数据
        sceneInfos = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        // 初始化怪物数据
        monsterInfos = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        // 初始化武器数据
        weaponInfos = JsonMgr.Instance.LoadData<List<WeaponInfo>>("WeaponInfo");
        // 初始化防御塔数据
        towerInfos = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }

    // 播放音效
    public void PlaySound(string name)
    {
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(name);
        a.volume = musicData.soundVolume;
        a.mute = !musicData.soundOpen;
        a.loop = false;
        a.Play();

        GameObject.Destroy(musicObj, 1);
    }

    // 保存音乐数据
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    // 保存玩家数据
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }
}
