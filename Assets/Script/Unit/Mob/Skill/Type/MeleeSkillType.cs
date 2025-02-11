﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//히트박스가 고정되어있는 근접공격등을 위한 클래스
public class MeleeSkillType : HitCheckSkillType
{
    //변수 영역
    #region Properties / Field

    //private 변수 영역
    #region Private
    private bool isSkillActivated;
    #endregion

    //protected 변수 영역
    #region protected
    #endregion

    //Public 변수영역
    #region public
    #endregion

    //이벤트 함수들 영역
    #region Event
    #endregion
    #endregion


    #region Method
    //private 함수들 영역
    #region PrivateMethod
    #endregion

    //protected 함수들 영역
    #region ProtectedMethod
    #endregion

    //public 함수들 영역
    #region PublicMethod
    #endregion
    #endregion



    #region Coroutine
    protected override IEnumerator HitChecking(GameObject hitBox)
    {
        hitBox.SetActive(true);

        HashSet<BattleSystem> calculatedObject = new HashSet<BattleSystem>();

        remainDuration = hitDuration;

        //콜라이더 사이즈를 위해서 콜라이더를 구한다.
        Collider hitBoxCol = hitBox.GetComponent<Collider>();

        while (remainDuration >= 0.0f && isSkillActivated)
        {
            remainDuration -= Time.deltaTime;
            Collider[] tempcol = Physics.OverlapBox(hitBox.transform.position, hitBoxCol.bounds.extents , hitBox.transform.rotation, targetMask);


            for (int i = 0; i < tempcol.Length; i++)
            {
                BattleSystem temp = tempcol[i].GetComponentInParent<BattleSystem>();
                //지금껏 충돌 해보지 못한 오브젝트와 충돌했을시
                //스킬이 맞았다고 이벤트 발생
                if (!calculatedObject.Contains(temp))
                {
                    //Debug.Log For check
                    //Debug.Log(tempcol[i].gameObject.name);
                    //맞췄을때 이펙트를 넣어줌
                    HitEffectPlay(hitBox.transform.position, tempcol[i].gameObject.transform.position);
                    calculatedObject.Add(temp);

                    if (Physics.Raycast(hitBox.transform.position, tempcol[i].bounds.center - hitBox.transform.position, out RaycastHit hit, hitBoxCol.bounds.extents.magnitude, targetMask))
                        onSkillHitEvent?.Invoke(tempcol[i], hit.point);
                }
            }
            yield return null;
        }

        hitBox.SetActive(false);
        yield return null;
    }
    #endregion


    //이벤트가 일어났을때 실행되는 On~~함수
    #region EventHandler
    public override void OnSkillHitCheckStartEventHandler()
    {
        if (!isSkillActivated)
        {
            isSkillActivated = true;
            for (int i = 0; i < maxIndex; i++)
            {
                StartCoroutine(HitChecking(areaOfEffect[i]));
            }
        }
    }

    public override void OnSkillHitCheckEndEventHandler()
    {
        isSkillActivated = false;
    }
    #endregion


    //유니티 함수들 영역
    #region MonoBehaviour
    #endregion
}
