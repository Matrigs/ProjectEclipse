using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour, ParticleInterface {

	public TextMesh lifeText;
	public float recoverTime;
	public int HP;
	[SerializeField]
	protected _state hurtboxState;
	private int _initialHP;

	public GameObject enemyParts;

    public ParticleSystem ThisObjectParticle
    {
        get
        {
            return this.GetComponent<ParticleSystem>();
        }
    }

	void Awake(){
		_initialHP = HP;
	}

    void OnEnable()
	{
		ChangeHPText();
	}
	public virtual void TakeDamage(int Damage, PlayerComponent.PlayerCharacter character)
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
			this.gameObject.SetActive(false);
			Instantiate (enemyParts, transform.position, transform.rotation * Quaternion.Euler(-180,0,0));
			ParticleSystem parts = enemyParts.GetComponent<ParticleSystem> ();

			//Rever depois pra não ficar tão cagado
			float totalDuration = parts.main.duration * 10;
			Destroy (enemyParts, totalDuration);
		}
	}
	void OnCollisionEnter2D (Collision2D info) {
		if (info.collider.tag == "Danger") {
			//Checa se não foi parryado
			ParryBox p = info.collider.GetComponent<ParryBox>();
			if(p == null || !p.parried) return;

			//Se foi, dá dano
			else{
				this.gameObject.SetActive(false);

				Instantiate (enemyParts, transform.position, transform.rotation * Quaternion.Euler(-180,0,0));
				ParticleSystem parts = enemyParts.GetComponent<ParticleSystem> ();

				//Rever depois pra não ficar tão cagado
				float totalDuration = parts.main.duration * 10;
				Destroy (enemyParts, totalDuration);

				info.gameObject.SetActive(false);
			}
		}
			
	}

	public void Reset(){
		HP = _initialHP;
		StartCoroutine(Recover(0));
	}

	protected IEnumerator Recover(float time)
	{
		yield return new WaitForSeconds(time);
		hurtboxState = _state.Open;
		ChangeColorState();
	}
	protected void ChangeHPText()
	{
		lifeText.text = HP.ToString();
	}
    protected void ChangeColorState()
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

    public void ActivateParticle(ParticleSystem ObjectParticle)
    {
        Debug.Log("Particle ACTIVE");
		if(ObjectParticle != null) ObjectParticle.Play();
    }

    public enum _state
	{
		Open,
		Closed
	}
}
