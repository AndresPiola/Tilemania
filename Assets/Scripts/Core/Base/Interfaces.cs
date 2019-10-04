using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
      bool  ApplyDamage(int Damage,Transform Instigator);

    void ForceDestroy(Vector3 _hitPosition);

}
 

public interface IUsable
{
     
    void Deactivate( );

    void Activate();

    void ActivateOnce();

}
