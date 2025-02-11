using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_Skill
{
    QSkill = 0,
    WSkill = 1,
    ESkill = 2,
    RSkill = 3
}

public class Player : BattleSystem, IGetDType, ICinematicStart, ICinematicEnd, IStun
{
    ParticleSystem particle;
    SkillManager sm;

    GameObject DebuffEffect;
    GameObject DebuffScreen;

    public Canvas DeBuffScr;

    public GameObject Effectobj;

    public LayerMask clickMask;

    public DefenceType Dtype;


    //0 : onehand 1: twohand
    public GameObject[] Weapon;

    public UnityEvent<Vector3, float, UnityAction<float>> clickAct;
    public UnityEvent<UnityAction<float>> stopAct;
    public UnityEvent<Vector3, float> dadgeAct;
    public UnityEvent<Vector3, float> rotAct;
    public UnityEvent<int> BuffAct;
    public UnityEvent<int, int> EnergyGageAct;
    public UnityEvent<int> SkillAct;
    public UnityEvent DurabilityAct;
    public float rotSpeed;
    public float DadgeDelay;
    public float dadgePw;
    float FireDelay = 0;

    bool isFireReady = true;
    bool isDadgeReady = true;
    bool isRun = true;
    bool Check;
    bool Death;
    int WeaponType = 0;

    Vector3 dir;
    public enum state
    {
        Fire, Dadge, Idle, Run, Skill, Cinematic, Stun
    }

    [SerializeField] protected state playerstate;
    protected void ChangeState(state s)
    {
        if (playerstate == s) return;
        playerstate = s;
        var emission = particle.emission;

        switch (s)
        {
            case state.Fire:
                emission.rateOverTime = 0;
                break;
            case state.Dadge:
                emission.rateOverTime = 0;
                break;
            case state.Idle:
                emission.rateOverTime = 0;
                break;
            case state.Run:
                emission.rateOverTime = 30f;
                break;
            case state.Skill:
                emission.rateOverTime = 0;
                break;
            case state.Cinematic:
                emission.rateOverTime = 0;
                break;
            case state.Stun:
                myAnim.SetBool("b_Stun", true);
                emission.rateOverTime = 0;
                break;
        }
    }

    protected void ProcessState()
    {
        switch (playerstate)
        {
            case state.Skill:
                break;
            case state.Fire:
                Check = AnimCheck("t_Attack");
                if (!Check)
                {
                    myAnim.SetTrigger("t_Attack");
                }
                else
                {
                    ChangeState(state.Idle);
                }
                break;
            case state.Dadge:
                break;
            case state.Idle:
                MoveToMousePos();
                FireToMousePos();
                DadgeToPos();
                Skill();
                break;
            case state.Run:
                MoveToMousePos();
                FireToMousePos();
                DadgeToPos();
                Skill();
                break;
            case state.Cinematic:
                break;
            case state.Stun:
                break;
        }
    }



    Dictionary<E_Skill, KeyCode> controllKey = new Dictionary<E_Skill, KeyCode>();
    protected override void Start()
    {
        controllKey[E_Skill.QSkill] = KeyCode.Q;
        controllKey[E_Skill.WSkill] = KeyCode.W;
        controllKey[E_Skill.ESkill] = KeyCode.E;
        controllKey[E_Skill.RSkill] = KeyCode.R;

        base.Start();
        WeaponType = DataManager.instance.playerData.Rigging_Weapon_Type;
        switch (WeaponType)
        {
            case 0:
                Weapon[0].SetActive(true);
                myAnim.SetBool("OneHandSword", true);
                battleStat.AttackDelay = 0.1f;
                break;
            case 1:
                Weapon[1].SetActive(true);
                myAnim.SetBool("TwoHandSword", true);
                break;
        }


        particle = GetComponentInChildren<ParticleSystem>();
        ChangeState(state.Idle);
    }

    void Update()
    {
        FireDelay -= Time.deltaTime;
        isFireReady = FireDelay < 0;
        DadgeDelay -= Time.deltaTime;
        isDadgeReady = DadgeDelay < 0;

        ProcessState();
    }

    public void GetStun()
    {
        Debug.Log("IStun Active");
        if(!Death)
        {
            stopAct?.Invoke((float stop) => myAnim.SetFloat("Move", stop));
            ChangeState(state.Stun);
        }
    }

    public void OutStun()
    {
        Debug.Log("IStun DIsActive");
        myAnim.SetBool("b_Stun", false);
        if(!Death)
        {
            ChangeState(state.Idle);
        }
    }

    public bool GetRaycastHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, clickMask))
        {
            dir = hit.point - transform.position;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FireToMousePos()
    {
        if (Input.GetMouseButtonDown(0) && isFireReady && GetRaycastHit() && !EventSystem.current.IsPointerOverGameObject())
        {

            stopAct?.Invoke((float stop) => myAnim.SetFloat("Move", stop));

            rotAct?.Invoke(dir, rotSpeed);

            ChangeState(state.Fire);
        }
    }

    bool AnimCheck(string Anim)
    {
        // ?�재 ?�니메이?�이 체크?�고???�는 ?�니메이?�인지 ?�인
        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName(Anim) == true)
        {
            // ?�하???�니메이?�이?�면 ?�레??중인지 체크
            float animTime = myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime == 0)
            {
                return false;
                // ?�레??중이 ?�님
            }
            if (animTime > 0 && animTime < 1.0f)
            {
                return true;
                // ?�니메이???�레??�?
            }
            else if (animTime >= 1.0f)
            {
                // ?�니메이??종료
            }
        }
        return false;
    }



    public void MoveToMousePos()
    {
        if (Input.GetMouseButton(1) && GetRaycastHit() && isRun )
        {
            if (dir == null) return;
            ChangeState(state.Run);
            clickAct?.Invoke(dir, GetModifiedStat(E_BattleStat.Speed), (float temp) =>
            {
                myAnim.SetFloat("Move", temp);
                if (temp < 0.05f && playerstate != state.Dadge && playerstate != state.Fire && playerstate != state.Skill)
                {
                    ChangeState(state.Idle);
                }
            });
        }
    }

    public void DadgeToPos()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isDadgeReady)
        {
            GetRaycastHit();
            SoundManager.instance.PlaySfxMusic("Dadge");
            stopAct?.Invoke((float stop) => myAnim.SetFloat("Move", stop));
            ChangeState(state.Dadge);
            myAnim.SetTrigger("t_Dadge");
            dadgeAct?.Invoke(dir, dadgePw);
        }
    }


    public void Skill()
    {
        for(int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(controllKey[(E_Skill)i]))
            {
                var plskill = DataManager.instance.playerData;
                if (!string.IsNullOrWhiteSpace(plskill.InGameSkillList[i]))
                {
                    GameObject effect = Resources.Load($"Player/SkillEffect/{ItemTypeIntToString.IntToStringSkillFileName(WeaponType)}/{plskill.InGameSkillList[i]}") as GameObject;
                    sm = effect.GetComponent<SkillManager>();
                }
                else
                {
                    Debug.Log("false");
                    return;
                }

                if (curBattleStat.EnergyGage >= sm.EnergyGage && !sm.CoolTimeCheck)
                {
                    SkillAct?.Invoke(i);
                    ChangeState(state.Skill);
                    rotSpeed = 200.0f;
                    curBattleStat.EnergyGage -= sm.EnergyGage;
                    EnergyGageCal();
                    GetRaycastHit();
                    stopAct?.Invoke((float stop) => myAnim.SetFloat("Move", stop));
                    myAnim.SetTrigger(plskill.InGameSkillList[i]);
                    rotAct?.Invoke(dir, rotSpeed);
                }
                else
                {
                    ChangeState(state.Idle);
                }
            }
        }
    }

    public void EnergyGageCal()
    {
        EnergyGageAct?.Invoke(battleStat.EnergyGage, curBattleStat.EnergyGage);
    }

    public void OnEnd(int type)
    {
        switch (type)
        {
            case 0:
                FireDelay = battleStat.AttackDelay;
                Check = false;
                break;
            case 1:
                DadgeDelay = 1.0f;
                break;
            case 2:
                break;
        }
        ChangeState(state.Idle);
    }


    public DefenceType GetDType(Collider col)
    {
        return Dtype;
    }

    protected override void OnDead()
    {
        if(!Death)
        {
            deathAlarm?.Invoke(0, gameObject);
            stopAct?.Invoke(null);
            myAnim.SetBool("b_Stun", false);
            myAnim.SetTrigger("t_Death");

            Death = true;
        }
    }

    public void ArmorDurability()
    {
        var plData = DataManager.instance.playerData;

        switch (Random.Range(0, 11))
        {
            case 0:
                plData.Rigging_Armor_Duration--;
                break;
            default:
                break;
        }

        if (plData.Rigging_Armor_Duration <= 0)
        {
            plData.Rigging_Armor_Duration = 0;
        }

        if (plData.Rigging_Armor_Duration == 50 || plData.Rigging_Armor_Duration == 25 || plData.Rigging_Armor_Duration == 0)
        {
            DurabilityAct?.Invoke();
        }
    }

    public void WeaponDurability()
    {
        var plData = DataManager.instance.playerData;
        if (plData.Rigging_Weapon_Duration == 50 || plData.Rigging_Weapon_Duration == 25 || plData.Rigging_Weapon_Duration == 0)
        {
            DurabilityAct?.Invoke();
        }
    }

    public void CinematicStart()
    {
        stopAct?.Invoke((float stop) => myAnim.SetFloat("Move", stop));
        ChangeState(state.Cinematic);
        myAnim.SetTrigger("t_Taunt");

    }

    public void CinematicEnd()
    {
        ChangeState(state.Idle);
    }

    public override void TakeDamage(int dmg, AttackType Atype, DefenceType Dtype)
    {
        int totaldmg;
        float computed = ComputeCompatibility(Atype, Dtype);
        var Armor = DataManager.instance.playerData.Rigging_Armor_Duration;
        float DurationDmg = 1.0f;

        if (Armor <= 50)
        {
            DurationDmg = 1.1f;
            if(Armor <= 25)
            {
                DurationDmg = 1.25f;
                if(Armor <= 0)
                {
                    DurationDmg = 1.4f;
                }
            }
        }

        totaldmg = (int)((float)dmg * computed * DurationDmg);
        /*Debug.Log("BattleSystem.TakeDamage");
        Debug.Log($"Atype : {Atype}, Dtype: {Dtype}");
        Debug.Log($"total : {totaldmg}");*/
        DamageTextController.Instance.DmgTxtPrint(transform.position, totaldmg, transform.name, computed);

        switch (computed)
        {
            case 1.2f:
                SoundManager.instance.HitSfxMusic();
                break;
            case 1.0f:
                SoundManager.instance.HitSfxMusic();
                break;
            case 0.8f:
                SoundManager.instance.HitSfxMusic();
                break;
        }

        curBattleStat.HP -= totaldmg;
        hpbarChangeAct?.Invoke(MaxHP, HP);
        DurationAct?.Invoke();

        if (curBattleStat.HP <= 0.0f)
        {
            //Die
            OnDead();
        }
    }
}