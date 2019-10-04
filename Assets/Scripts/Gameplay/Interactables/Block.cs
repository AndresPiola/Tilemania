using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour,IDamageable
{
    

    public bool ApplyDamage(int Damage, Transform Instigator)
    {

        BlockExplosionPooler.Instance.GetPooledObject(transform.position);


        Destroy(gameObject);
        return true;
    }

    public void ForceDestroy(Vector3 _hitPosition)
    {
        throw new System.NotImplementedException();
    }
}
