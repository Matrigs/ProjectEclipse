using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour {

	public TextMesh lifeText;
	public float recoverTime;
	public int HP;
	[SerializeField]
	private _state hurtboxState;

	void OnEnable()
	{
		ChangeHPText();
	}

	public void TakeDamage(int Damage)
	{
		if(hurtboxState == _state.Open)	
		{
			HP -= Damage;
			hurtboxState = _state.Closed;
			ChangeColorState();
			ChangeHPText();
			StartCoroutine(Recover(recoverTime));
		}
		if(HP <= 0)
		{
			Destroy(this.gameObject);
		}
	}


	private IEnumerator Recover(float time)
	{
		yield return new WaitForSeconds(time);
		hurtboxState = _state.Open;
		ChangeColorState();
	}
	private void ChangeHPText()
	{
		lifeText.text = HP.ToString();
	}
	private void ChangeColorState()
	{
		SpriteRenderer sprend = GetComponent<SpriteRenderer>();

		switch(hurtboxState)
		{
			case _state.Open:
				sprend.color = Color.white;
			break;
			case _state.Closed:
				sprend.color = Color.red;
			break;
			default:
			sprend.color = Color.white;
			break;
		}
	}

	public enum _state
	{
		Open,
		Closed
	}
}
