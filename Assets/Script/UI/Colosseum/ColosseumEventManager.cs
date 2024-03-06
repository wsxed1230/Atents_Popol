using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Security.Cryptography;
using System;

public class ColosseumEventManager : MonoBehaviour
{

    public GameObject ColoBtn;
    public GameObject BossState;
    public GameObject Obj_Slime_Lv1;
    public GameObject HideBoss;
    public GameObject BossStage;
    enum BossMonsterType
    {
        None,
        Slime_Lv1,
        TurtleShell_Lv1,
        Slime_Lv2,
        TurtleShell_Lv2,
        Slime_Lv3,
        TurtleShell_Lv3,
    }

    int BossMonsterDisplayer;
    bool[] isClearBoss = new bool[System.Enum.GetValues(typeof(BossMonsterType)).Length];
    // Start is called before the first frame update
    void Start()
    {
        isClearBoss[0] = true;
        BossMonsterDisplayer = 1;
        HideBoss.transform.gameObject.SetActive(false);
        ChangeDisplay();
        Button[] ColoBtnList = ColoBtn.GetComponentsInChildren<UnityEngine.UI.Button>();//MainUI버튼
        for(int i = 0; i < ColoBtnList.Length; i++)
        {
            int index = i;
            ColoBtnList[i].onClick.AddListener(() => ColoBtnManager(index));
        }
    }
    void ChangeDisplay()
    {
        switch(BossMonsterDisplayer)
        {
            case 1:
            DisplayBossMonster(BossMonsterType.Slime_Lv1, isClearBoss[0]);
                break;
            case 2:
            DisplayBossMonster(BossMonsterType.TurtleShell_Lv1, isClearBoss[1]);
                break;
            case 3:
            DisplayBossMonster(BossMonsterType.Slime_Lv2, isClearBoss[2]);
                break;

        }
    }
    void DisplayBossMonster(BossMonsterType B, bool ClearCheck)
    {
        HideBoss.transform.gameObject.SetActive(false);
        if(B == BossMonsterType.Slime_Lv1 && ClearCheck)
        {
            DisplayState(Obj_Slime_Lv1, "슬라임", "가죽", "콜로세움에서 전투용으로 키운 슬라임이다.");
        }
        else if(!ClearCheck)
        {
            HideBoss.transform.gameObject.SetActive(true);
        }
    }
    void DisplayState(GameObject boss, string Bossname, string BossArmor, string BossDesc)
    {
        Instantiate(boss,BossState.transform.GetChild(0));
        BossState.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>().text = //보스 어빌리티
        $"보스 이름 : {Bossname}\n\n방어 타입 : {BossArmor}\n\n 스킬 :\n\n패시브 :";
        BossState.transform.GetChild(1).GetChild(1).gameObject.GetComponent<TMP_Text>().text =
        $"설명 :\n{BossDesc}";
    }

    void ColoBtnManager(int index)
    {
        
        if(index == 0)
        {
            if(BossMonsterDisplayer > 1)
            {
                BossMonsterDisplayer--;
                ChangeDisplay();
            }
        }
        if(index == 1)
        {
            if(BossMonsterDisplayer < System.Enum.GetValues(typeof(BossMonsterType)).Length)
            {
                BossMonsterDisplayer++;
                ChangeDisplay();
            }
        }
        BossStage.gameObject.GetComponent<TMP_Text>().text =
        $"Stage\n{BossMonsterDisplayer}";
    }

    void Update()
    {

    }
    
}
