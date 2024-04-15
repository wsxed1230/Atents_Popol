using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //will delete later
    //this is for file load test
    private Dictionary<int, WaveData> dicWaveTable;

    private WaveData curWave;
    private Queue<WaveData> waveQueue = new Queue<WaveData>();

    private MonsterFactory mf;

    public UnityEvent<int> deadAct;

    //stage load, int for send total wave count
    public UnityEvent<int> loadStageDataEvent;
    //wave end event
    public UnityEvent waveEndEvent;
    //boss wave start
    public UnityEvent bossWaveStartEvent;
    //every wave is end
    public UnityEvent stageEndEvent;
    //player loss Game
    public UnityEvent lossGameEvent;

    public GameObject Player;
    public HpAndEnergy hpEpBar;
    //public PlayerDetaManager playerdata;


    private Player pl;
    private List<GameObject> monsters = new List<GameObject>();


    private void Awake()
    {
        mf = new MonsterFactory();

        pl = Player.GetComponent<Player>();
        pl.hpbarChangeAct.AddListener(hpEpBar.HpGageTrigger);


        //Monster = mf.CreateMonster(30000);
        //Ms = Monster.GetComponent<Monster>();

        //need stage number
        //for test, static int 1 inside the function
        LoadStageData(1);
        StartCoroutine(WaveRound());

        LoadPlayerStat();
    }

    private void LoadStageData(int stageIndex)
    {
        var Mestiarii_WaveData = Resources.Load<TextAsset>("System/Mestiarii_WaveData_Table").text;
        var arrWaveDatas = JsonConvert.DeserializeObject<WaveData[]>(Mestiarii_WaveData);
        this.dicWaveTable = arrWaveDatas.ToDictionary(x => x.index);

        foreach(var data in dicWaveTable)
        {
            if(data.Value.Stage_Index == stageIndex)
            {
                waveQueue.Enqueue(data.Value);
            }
        }

        //waveQueue.Count is wave number
        //need to send waveQueue.Count to WaveUI class
        loadStageDataEvent?.Invoke(waveQueue.Count);




        /*
        //for debug
        while(waveQueue.Count > 0)
        {
            Debug.Log(waveQueue.Dequeue().index);
        }
        */
    }

    private void LoadPlayerStat()
    {
        int NextLevel;
        BattleStat bs = default;

        var pldata = DataManager.instance.playerData;
        //var unitname = PlayerDetaManager.instance.dicStringData[playerstat.Character_Name]; // UI ������ ���?����
        bs.Exp = pldata.Character_CurrentExp;
        bs.Level = pldata.Character_CurrentLevel;
        bs.ATK = pldata.Character_AttackPower;
        bs.HP = pldata.Character_Hp;
        bs.EnergyGage = pldata.Character_EnergyGage; // int
        bs.Speed = pldata.Character_MoveSpeed; // float
        bs.AttackDelay = pldata.Character_AttackSpeed; // float

        NextLevel = bs.Level + 1;

        var plLvstat1 = PlayerDetaManager.instance.dicPlayerLevelData[NextLevel];
        bs.MaxExp = plLvstat1.Total_Exp;

        pl.battlestat = bs;
    } 
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F4))
        {
            OnGameEnd(1, gameObject);
        }
    }

    public void OnGameEnd(int UnitType, GameObject deadUnit)
    {
        //pl.enabled = false;
        var playerdata = DataManager.instance.playerData;
        if (UnitType == 0) // Player Dead
        {
            lossGameEvent?.Invoke();
        }
        else // Monstar Dead
        {
            //pl.Exp += Ms.Exp;
            //playerdata.Character_CurrentExp += Ms.Exp;
            if (monsters.Contains(deadUnit))
            {
                monsters.Remove(deadUnit);
                if(monsters.Count == 0)
                {
                    //RoundEnd
                    //new Round Start
                    StartCoroutine(WaveRound());
                    waveEndEvent?.Invoke();
                }
            }

            StartCoroutine(LevelUp());
        }
        deadAct?.Invoke(UnitType);
        DataManager.instance.SaveData();
        if (curWave.index / 10000 >= 2 && monsters.Count <= 0) //Boss Wave End
        {
            Debug.Log("StageEnd");
            StartCoroutine(tempDebug());
            stageEndEvent?.Invoke();
        }
    }
    public void UpdateUI()
    {
        //DataManager.instance.playerData.Armor_Duration = 
    }

    IEnumerator tempDebug()
    {
        yield return new WaitForSeconds(12);
        SceneLoading.SceneNum(2);
        SceneManager.LoadScene(1);
    }

    IEnumerator LevelUp()
    {
        var playerstat = PlayerDetaManager.instance.dicPlayerData[10000];
        var playerdata = DataManager.instance.playerData;
        while(playerdata.Character_CurrentExp >= PlayerDetaManager.instance.dicPlayerLevelData[playerdata.Character_CurrentLevel + 1].Total_Exp)
        {
            playerdata.Character_CurrentLevel++;
            if (playerdata.Character_CurrentLevel >= 30)
            {
                playerdata.Character_CurrentLevel = 30;
            }
            yield return null;
            var plLvstat = PlayerDetaManager.instance.dicPlayerLevelData[playerdata.Character_CurrentLevel];
            playerdata.Character_AttackPower = playerstat.Character_AttackPower + plLvstat.Total_AttackPower;
            playerdata.Character_Hp = playerstat.Character_Hp + plLvstat.Total_Hp;
        }
    }
    
    private IEnumerator WaveRound()
    {
        int spawnOffset = 15;
        if(waveQueue.Count > 0)
        {
            monsters.Clear();
            curWave = waveQueue.Dequeue();

            int count = 0;
            count += curWave.Wave_Monster_Count1;
            count += curWave.Wave_Monster_Count2;
            count += curWave.Wave_Monster_Count3;
            count += curWave.Wave_Monster_Count4;
            count += curWave.Wave_Monster_Count5;

            float angle = 360 / (float)count;
            Vector3 spawnPoint = Vector3.back * spawnOffset;

            for(int i = 0; i < curWave.Wave_Monster_Count1 && curWave.Wave_Monster1 != 0; ++i)
            {
                monsters.Add(mf.CreateMonster(curWave.Wave_Monster1));
                spawnPoint = Quaternion.Euler(0, angle, 0) * spawnPoint;
                monsters.Last().transform.position = spawnPoint;
                yield return new WaitForSeconds(1);
            }
            for (int i = 0; i < curWave.Wave_Monster_Count2 && curWave.Wave_Monster2 != 0; ++i)
            {
                monsters.Add(mf.CreateMonster(curWave.Wave_Monster2));
                spawnPoint = Quaternion.Euler(0, angle, 0) * spawnPoint;
                monsters.Last().transform.position = spawnPoint;
                yield return new WaitForSeconds(1);
            }
            for (int i = 0; i < curWave.Wave_Monster_Count3 && curWave.Wave_Monster3 != 0; ++i)
            {
                monsters.Add(mf.CreateMonster(curWave.Wave_Monster3));
                spawnPoint = Quaternion.Euler(0, angle, 0) * spawnPoint;
                monsters.Last().transform.position = spawnPoint;
                yield return new WaitForSeconds(1);
            }
            for (int i = 0; i < curWave.Wave_Monster_Count4 && curWave.Wave_Monster4 != 0; ++i)
            {
                monsters.Add(mf.CreateMonster(curWave.Wave_Monster4));
                spawnPoint = Quaternion.Euler(0, angle, 0) * spawnPoint;
                monsters.Last().transform.position = spawnPoint;
                yield return new WaitForSeconds(1);
            }
            for (int i = 0; i < curWave.Wave_Monster_Count5 && curWave.Wave_Monster5 != 0; ++i)
            {
                monsters.Add(mf.CreateMonster(curWave.Wave_Monster5));
                spawnPoint = Quaternion.Euler(0, angle, 0) * spawnPoint;
                monsters.Last().transform.position = spawnPoint;
                yield return new WaitForSeconds(1);
            }


            yield return null;
        }
    }
    
    public void EndGame()
    {
        var filePath = "Assets/Resources/Player/PlayerStat/Playerlv.json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
