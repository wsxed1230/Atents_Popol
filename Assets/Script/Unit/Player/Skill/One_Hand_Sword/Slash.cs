using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Slash : PlayerSkill
{
    public float moveSpeed = 10f; 
    public float destroyDelay = 1f;
    public UnityEvent onHitAct;
    
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null && rb.mass > 0)
        {
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);
        }

        Destroy(gameObject, destroyDelay);
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster_Body"))
        {
            IDamage iDamage = other.GetComponent<IDamage>();
            if(iDamage != null)
            {
                onHitAct?.Invoke();
                iDamage.TakeDamage(1000);
            }
        }
    }
}
