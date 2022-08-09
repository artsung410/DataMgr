using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameData
{
    public int BGM_Volume = 0;
    public int Effect_Volume = 0;

    public int gold = 0;
    public int hp = 10;
    public float moveSpeed = 5f;

    public List<MonsterData> monsterKillDatas;

    public GameData(int _gold, int _hp, float _moveSpeed)
    {
        gold = _gold;
        hp = _hp;
        moveSpeed = _moveSpeed;
        monsterKillDatas = new List<MonsterData>();
    }
}

[Serializable]
public class MonsterData
{
    public int index;
    public string name;
    public float moveSpeed;
    public float rotationSpeed;
    public string description;

    public MonsterData(int index, string name, float moveSpeed, float rotationSpeed, string description)
    {
        this.index = index;
        this.name = name;
        this.moveSpeed = moveSpeed;
        this.rotationSpeed = rotationSpeed;
        this.description = description;
    }
}

public class DataMgr : MonoBehaviour
{
    //public int Index { get; private set; }

    static GameObject container;
    static GameObject Container { get => container; }

    static DataMgr instance;
    public static DataMgr Instance
    {
        get
        {
            if (instance == null)
            {
                container = new GameObject();
                container.name = "DataMgr";
                instance = container.AddComponent(typeof(DataMgr)) as DataMgr;

                //instance.SetMonsterDataFromCSV();

                DontDestroyOnLoad(container);
            }

            Debug.Log("인스턴스 생성");

            return instance;
        }
    }

    GameData gameDatas;
    public GameData GameData
    {
        get
        {
            if (gameDatas == null)
            {
                LoadGameData();
                SaveGameData();
            }

            return gameDatas;
        }
    }

    void InitGameData()
    {
        gameDatas = new GameData(100, 300, 5f);
        gameDatas.monsterKillDatas.Add(new MonsterData(1, "일반 스켈레톤", 2f, 1f, "오늘 안옴"));
        gameDatas.monsterKillDatas.Add(new MonsterData(2, "레드 스켈레톤", 2f, 1f, "오늘 안옴"));
    }

    public void SaveGameData()
    {
        //InitGameData();
        string toJsonData = JsonUtility.ToJson(gameDatas, true);

        Debug.Log(toJsonData);

        // 모바일 : Application.persistentDataPath, 에디터 : DataPath
        // filePath => C:\Users\[UserName]\AppData\LocalLow\DefaultCompany
        string filePath = Application.persistentDataPath + GameDataFileName; 
        File.WriteAllText(filePath, toJsonData);
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);
            Debug.Log(fromJsonData);
            gameDatas = JsonUtility.FromJson<GameData>(fromJsonData);

            if (gameDatas == null)
            {
                InitGameData();
            }
        }

        else
        {
            InitGameData();
        }
    }

    public string GameDataFileName = ".json";

    //[Header("몬스터 관련 DB")]

    //[SerializeField] TextAsset monsterDB;

    //public Dictionary<int, MonsterData> MonsterDataDict { get; set; }

    //private void SetMonsterDataFromCSV()
    //{
    //    monsterDB = Resources.Load<TextAsset>("CSV/GameData - Monster");

    //    if (monsterDB == null)
    //    {
    //        Debug.LogError("CSV/GameData - Monster 파일이 없음!!");
    //        return;
    //    }

    //    if (MonsterDataDict == null)
    //    {
    //        MonsterDataDict = new Dictionary<int, MonsterData>();
    //    }

    //    string[] lines = monsterDB.text.Substring(0, monsterDB.text.Length).Split('\n');
    //    for (int i = 1; i < lines.Length; i++)
    //    {
    //        string[] row = lines[i].Split(',');
    //        MonsterDataDict.Add(int.Parse(row[0]), new MonsterData(
    //            int.Parse(row[0]),      // index
    //            row[1],                 // name
    //            float.Parse(row[2]),    // moveSpeed
    //            float.Parse(row[3]),    // rotationSpeed
    //            row[4]                  // description
    //            ));
    //    }
    //}

    //public MonsterData GetMonsterData(int index)
    //{
    //    if (MonsterDataDict.ContainsKey(index))
    //    {
    //        return MonsterDataDict[index];
    //    }

    //    Debug.LogWarning(index + ", 해당 인덱스 데이터가 없음");
    //    return null;
    //}
}
