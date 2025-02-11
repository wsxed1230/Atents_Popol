using System.Collections;
using UnityEngine;

public abstract class DotDamageStatusEffect : StatusEffect
{
    private BattleSystem target;
    private int value;
    protected float dotRate;
    protected bool isHeal = false;


    public DotDamageStatusEffect(string s) : base(s) { }

    protected virtual void Start()
    {
        LoadEffect();
        Initailize();
        target = GetComponentInParent<BattleSystem>();
        value = (int)(target.MaxHP * (dotRate / 100.0f));
        if (value <= 0)
            value = 1;

        
        StartCoroutine(DotDamaging());
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected IEnumerator DotDamaging()
    {
        float temp = 0f;
        int sign = 0;
        if (isHeal)
        {
            sign = 1;
        }
        else
        {
            sign = -1;
        }

        while (true)
        {
            temp += value * Time.deltaTime;
            if (temp > 1)
            {
                target.HP += (int)temp * sign;
                temp -= (int)temp;
            }
            yield return null;
        }
    }

}
