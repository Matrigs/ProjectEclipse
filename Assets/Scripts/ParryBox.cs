using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bullet))]
public class ParryBox : Hurtbox {
    [HideInInspector] public bool parried = false;

    public override void TakeDamage(int Damage, PlayerComponent.PlayerCharacter character)
    {
        if (!parried && character == PlayerComponent.PlayerCharacter.Luna)
		{
            GetComponent<Bullet>().velocity *= -1;
            parried = true;
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
