public class SelfSkillType : BaseSkillType
{
    //λ³???μ­
    #region Properties / Field
    //private λ³???μ­
    #region Private
    #endregion

    //protected λ³???μ­
    #region protected
    //λΆλͺ¨μ Unit??μ°Έμ‘°???€λΈ?νΈ
    protected BattleSystem selfBS;
    #endregion

    //Public λ³?μ??
    #region public
    #endregion

    //?΄λ²€???¨μ???μ­
    #region Event
    #endregion
    #endregion


    #region Method
    //private ?¨μ???μ­
    #region PrivateMethod
    #endregion

    //protected ?¨μ???μ­
    #region ProtectedMethod
    #endregion

    //public ?¨μ???μ­
    #region PublicMethod
    #endregion
    #endregion


    #region Coroutine
    #endregion


    //?΄λ²€?Έκ? ?Όμ΄?¬μ???€ν?λ On~~?¨μ
    #region EventHandler
    #endregion


    //? λ???¨μ???μ­
    #region MonoBehaviour
    protected override void Start()
    {
        selfBS = GetComponentInParent<BattleSystem>();
    }
    #endregion
}
