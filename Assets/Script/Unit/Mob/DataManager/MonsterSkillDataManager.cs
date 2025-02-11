using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using Unity.VisualScripting;

public class MonsterSkillDataManager
{
    private static MonsterSkillDataManager instance;
    //UI
    public Dictionary<int, UIStringTable> dicStringTable;
    public Dictionary<int, PrefabTable> dicPrefabTable;
    
    public Dictionary<int, SkillImageResourceTable> dicResourceTable;

    //Monster
    public Dictionary<int, MonsterDataTable> dicMonsterDataTable;

    //SkillTable
    public Dictionary<int, SkillDataTable> dicSkillDataTable;
    //SkillDetail
    public Dictionary<int, SkillMovementTypeDataTable> dicSkillMovementTypeDataTable;
    public Dictionary<int, SkillMeleeTypeDataTable> dicSkillMeleeTypeDataTable;
    public Dictionary<int, SkillProjectileDetailDataTable> dicSkillProjectileDetailDataTable;
    //SkillAffect
    public Dictionary<int, SkillDamageAffectDataTable> dicSkillDamageAffectDataTable;
    public Dictionary<int, SkillKnockBackAffectDataTable> dicSkillKnockBackAffectDataTable;
    public Dictionary<int, SkillInflictExtraAffectDataTable> dicSkillInflictExtraAffectDataTable;
    //GameManager
    public Dictionary<int, BossStageTable> dicStageTable;
    public static MonsterSkillDataManager GetInstance()
    {
        if(MonsterSkillDataManager.instance == null)
            MonsterSkillDataManager.instance = new MonsterSkillDataManager();
        return MonsterSkillDataManager.instance;
    }
    public void LoadSkillDatas()
    {
        var Mestiarii_Monster_SkillStringTable = Resources.Load<TextAsset>("Monster/SkillData/Mestiarii_Monster_SkillStringTable").text;
        var Mestiarii_PrefabTable = Resources.Load<TextAsset>("Monster/SkillData/SkillUI/Mestiarii_BossMonster_Display_Prefab").text;
        
        var arrStringDatas = JsonConvert.DeserializeObject<UIStringTable[]>(Mestiarii_Monster_SkillStringTable);

        this.dicStringTable = arrStringDatas.ToDictionary(x => x.index);

    }
    public void LoadSkillUI()
    {
        var Mestiarii_PrefabTable = Resources.Load<TextAsset>("Monster/Mestiarii_PrefabTable").text;
        var Mestiarii_BossStage_Table = Resources.Load<TextAsset>("Monster/SkillData/SkillUI/Mestiarii_BossStage_Table").text;
        var Mestiarii_Skill_ImgaeResource_Table = Resources.Load<TextAsset>("Monster/SkillData/SkillUI/Mestiarii_Skill_ImageResource_Table").text;
        var Mestiarii_Monster_SkillStringTable = Resources.Load<TextAsset>("Monster/SkillData/Mestiarii_Monster_UIStringTable").text;

        var arrDisplayDatas = JsonConvert.DeserializeObject<PrefabTable[]>(Mestiarii_PrefabTable);
        /* foreach(var data in arrDisplayDatas)
        {
            Debug.LogFormat($"{data.index}, {data.Prefab_Name}");
        } */
        var arrStageDatas = JsonConvert.DeserializeObject<BossStageTable[]>(Mestiarii_BossStage_Table);
        var arrResourceDatas = JsonConvert.DeserializeObject<SkillImageResourceTable[]>(Mestiarii_Skill_ImgaeResource_Table);
        var arrStringDatas = JsonConvert.DeserializeObject<UIStringTable[]>(Mestiarii_Monster_SkillStringTable);

        this.dicPrefabTable =  arrDisplayDatas.ToDictionary(x => x.index);
        this.dicStageTable =  arrStageDatas.ToDictionary(x => x.index);
        this.dicResourceTable =  arrResourceDatas.ToDictionary(x => x.index);
        this.dicStringTable = arrStringDatas.ToDictionary(x => x.index);
    }

    public void LoadMonsterMakingDatas()
    {
        var Mestiarii_PrefabTable = Resources.Load<TextAsset>("Monster/Mestiarii_PrefabTable").text;
        var Mestiarii_MonsterDataTable = Resources.Load<TextAsset>("Monster/Character_Ability_Monster").text;
        var Mestiarii_SkillDataTable = Resources.Load<TextAsset>("Monster/SkillData/Mestiarii_Monster_SkillTable").text;
        var Mestiarii_SkillMovementTypeDataTable = Resources.Load<TextAsset>("Monster/SkillData/SkillType/Monster_SkillDetail_Movement").text;
        var Mestiarii_SkillMeleeTypeDataTable = Resources.Load<TextAsset>("Monster/SkillData/SkillType/Monster_SkillDetail_Melee").text; 
        var Mestiarii_SkillProjectileDetailDataTable = Resources.Load<TextAsset>("Monster/SkillData/SkillType/Monster_SkillDetail_Projectile").text;
        var Mestiarii_SkillDamageAffectDataTable = Resources.Load<TextAsset>("Monster/SkillData/SkillAffect/Monster_SkillAffect_DamageAffect").text;
        var Mestiarii_SkillKnockBackAffectDataTable = Resources.Load<TextAsset>("Monster/SkillData/SkillAffect/Monster_SkillAffect_KnockBackAffect").text;
        var Mestiarii_SkillInflictExtraAffectDataTable = Resources.Load<TextAsset>("Monster/SkillData/SkillAffect/Monster_SkillAffect_InflictExtraAffect").text;

        var arrPrefabDatas = JsonConvert.DeserializeObject<PrefabTable[]>(Mestiarii_PrefabTable);
        var arrMonsterDatas = JsonConvert.DeserializeObject<MonsterDataTable[]>(Mestiarii_MonsterDataTable);
        var arrSkillDatas = JsonConvert.DeserializeObject<SkillDataTable[]>(Mestiarii_SkillDataTable);
        var arrSkillTypeMovementDatas = JsonConvert.DeserializeObject<SkillMovementTypeDataTable[]>(Mestiarii_SkillMovementTypeDataTable);
        var arrSkillTypeMeleeDatas = JsonConvert.DeserializeObject<SkillMeleeTypeDataTable[]>(Mestiarii_SkillMeleeTypeDataTable);
        var arrSkillTypeProjectileDatas = JsonConvert.DeserializeObject<SkillProjectileDetailDataTable[]>(Mestiarii_SkillProjectileDetailDataTable);
        var arrSkillAffectDamageDatas = JsonConvert.DeserializeObject<SkillDamageAffectDataTable[]>(Mestiarii_SkillDamageAffectDataTable);
        var arrSkillAffectKnockBackDatas = JsonConvert.DeserializeObject<SkillKnockBackAffectDataTable[]>(Mestiarii_SkillKnockBackAffectDataTable);
        var arrSkillAffectInflictExtraDatas = JsonConvert.DeserializeObject<SkillInflictExtraAffectDataTable[]>(Mestiarii_SkillInflictExtraAffectDataTable);

        this.dicPrefabTable = arrPrefabDatas.ToDictionary(x => x.index);
        this.dicMonsterDataTable = arrMonsterDatas.ToDictionary(x => x.Index);
        this.dicSkillDataTable = arrSkillDatas.ToDictionary(x => x.Index);
        this.dicSkillMovementTypeDataTable = arrSkillTypeMovementDatas.ToDictionary(x => x.Index);
        this.dicSkillMeleeTypeDataTable = arrSkillTypeMeleeDatas.ToDictionary(x => x.Index);
        this.dicSkillProjectileDetailDataTable = arrSkillTypeProjectileDatas.ToDictionary(x => x.Index);
        this.dicSkillDamageAffectDataTable = arrSkillAffectDamageDatas.ToDictionary(x => x.Index);
        this.dicSkillKnockBackAffectDataTable = arrSkillAffectKnockBackDatas.ToDictionary(x => x.Index);
        this.dicSkillInflictExtraAffectDataTable = arrSkillAffectInflictExtraDatas.ToDictionary(x => x.Index);
    }
}

