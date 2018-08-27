using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlayer : MonoBehaviour {

	[SerializeField]
	private bool _playerPresence;
	[SerializeField]
	private bool _playerHidden = false;
	private Transform playerTranform;
	[SerializeField]
	private bool _fromTheRight;
	private Vector2 positionToGo;
	private void OnEnable()
    {
        PlayerComponent.ActionButton += Action;
    }
    private void OnDisable()
    {
        PlayerComponent.ActionButton -= Action;
    }
	void Update()
	{
		
	}
	void Action()
	{
		if(_playerPresence && !_playerHidden)
		{
			positionToGo = new Vector2(this.transform.position.x - playerTranform.position.x,playerTranform.position.y);
			if(positionToGo.x < 0) 
			{				
				_fromTheRight = true;
			}
			if(playerTranform.gameObject.layer != LayerMask.NameToLayer("Hide"))
			{
				playerTranform.GetComponent<Collider2D>().enabled = false;
				playerTranform.GetComponent<PlayerComponent>().enabled = false;
				playerTranform.GetComponent<Controller2D>().enabled = false;
				playerTranform.gameObject.layer = LayerMask.NameToLayer("Hide");
				playerTranform.position = this.transform.position;

				_playerHidden = true;
			}
			
		}
		else if(_playerHidden)
		{
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;

			playerTranform.GetComponent<Collider2D>().enabled = true;
			playerTranform.GetComponent<PlayerComponent>().enabled = true;
			playerTranform.GetComponent<Controller2D>().enabled = true;
			playerTranform.gameObject.layer = LayerMask.NameToLayer("Player");
			if(!_fromTheRight)
				playerTranform.position = this.transform.position + Vector3.right;
			else
				playerTranform.position = this.transform.position + Vector3.left;
			_playerHidden = false;
			_fromTheRight = false;
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hero");
        if (collision.transform.CompareTag("Player"))
        {
			playerTranform = collision.transform;
            _playerPresence = true;

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            _playerPresence = false;
        }
    }
}
