using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bullet))]
public class ParryBox : Hurtbox {
    private bool _parried = false;

    public override void TakeDamage(int Damage)
    {
        if (!_parried)
        {
            GetComponent<Bullet>().velocity *= -1;
            _parried = true;
        }
    }
}
