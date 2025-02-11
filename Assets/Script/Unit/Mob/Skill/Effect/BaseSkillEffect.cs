using UnityEngine;

public abstract class BaseSkillEffect : MonoBehaviour
{
    //변수 영역
    #region Properties / Field
    //private 변수 영역
    #region Private
    #endregion

    //protected 변수 영역
    #region protected
    //공격력 * power로 스킬의 배율
    protected BattleSystem myBattleSystem = null;
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
    public void InitializeBattleSystem(BattleSystem bs)
    {
        myBattleSystem = bs;
    }
    #endregion
    #endregion


    #region Coroutine
    #endregion


    //이벤트가 일어났을때 실행되는 On~~함수
    #region EventHandler
    public abstract void OnSkillHit(Collider target, Vector3 pos);
    #endregion


    //유니티 함수들 영역
    #region MonoBehaviour
    protected virtual void Start()
    {
        if(myBattleSystem == null)
            myBattleSystem = GetComponentInParent<BattleSystem>();
    }
    #endregion

}
