using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bullet))]
public class ParryBox : Hurtbox {
    private bool _parried = false;

    public override void TakeDamage(int Damage, PlayerComponent.PlayerCharacter character)
    {
        if (!_parried && character == PlayerComponent.PlayerCharacter.Luna)
        {
            GetComponent<Bullet>().velocity *= -1;
            _parried = true;
        }
        else
        {
            if (hurtboxState == _state.Open)
            {
                HP -= Damage;
                hurtboxState = _state.Closed;
                ChangeColorState();
                ChangeHPText();
                StartCoroutine(Recover(recoverTime));
            }
            if (HP <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
