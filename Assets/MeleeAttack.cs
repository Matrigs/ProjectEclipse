using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour 
{
	public LayerMask mask;
	public Vector2 attackBoxSize;
	public Vector2 attackBoxOffset;
	public bool facingRight = true;
	public int Damage;
	private int side;
	[Space]
	public Color inactiveColor;
	public Color collisionOpenColor;
	public Color collidingColor;
	[Space]
	public Controller2D col2D;

	private ColliderState _state;
	
	void Update () 
	{
		UpdateDir();

		if(facingRight) side = -1;
		else side = 1;

		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + ((Vector3)attackBoxOffset * side),attackBoxSize,0f,mask);		
		if (_state == ColliderState.Closed) { return; }
		if(colliders.Length > 0)
		{
			_state = ColliderState.Colliding;
			Debug.Log("Attack Hit:");
			for(int i = 0; i < colliders.Length; i++)
			{
				if(colliders[i].GetComponent<Hurtbox>())
				{
					colliders[i].GetComponent<Hurtbox>().TakeDamage(Damage);
				}
			}

		}
	}

	private void OnDrawGizmos() 
	{
        checkGizmoColor();
        //Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(transform.position + ((Vector3)attackBoxOffset * side), new Vector3(attackBoxSize.x * 2, attackBoxSize.y * 2,0f)); // Because size is halfExtents
		
		//Gizmos.DrawWireCube()
    }
	private void UpdateDir()
	{
		facingRight = col2D.facingRight;
	}

	public enum ColliderState 
	{
        Closed,
        Open,
        Colliding
    }

	private void checkGizmoColor() 
	{
	switch(_state) {
	case ColliderState.Closed:
		Gizmos.color = inactiveColor;
		break;
	case ColliderState.Open:
		Gizmos.color = collisionOpenColor;
		break;
	case ColliderState.Colliding:
		Gizmos.color = collidingColor;
		break;
	}
	}
	public void startCheckingCollision() 
	{
		_state = ColliderState.Open; 
	}

    public void stopCheckingCollision() 
	{
        _state = ColliderState.Closed; 
    }
}
