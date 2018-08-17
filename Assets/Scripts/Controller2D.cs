using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController {

	public float maxSlopeAngle;
	public bool facingRight;

	public CollisionInfo collisionsInf;

	public override void OnEnable()
	{
		base.OnEnable();
	}

    public void Move(Vector3 deltaMove,bool standingOnPlatform = false)
    {	
		UpdateRaycastOrigins();
		collisionsInf.ResetColl();

		collisionsInf.velocityOld = deltaMove;

		if(deltaMove.y < 0)
		{
			DescendSlope(ref deltaMove);
		}
		if(deltaMove.x  != 0) 
		{
			HorizontalCollisions(ref deltaMove);
		}
		if(deltaMove.y != 0)
		{
			VerticalCollisions(ref deltaMove);
		}

    	transform.Translate (deltaMove);

		if(standingOnPlatform)
		{
			collisionsInf.below = true;
		}
    }

	void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = Mathf.Sign(velocity.y);
		float rayLenght = Mathf.Abs(velocity.y) + skinWidth;

		for(int i = 0; i <verticalRayCount; i++)
		{
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x) ; 
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin,Vector2.up * directionY,rayLenght,collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY,Color.red);

			if(hit)
			{
				if(hit.collider.tag == "Pass")
				{
					if(directionY == 1 || hit.distance == 0)
					{
						continue;
					}
				}

				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLenght = hit.distance;

				if(collisionsInf.climbingSlope)
				{
					velocity.x = velocity.y/Mathf.Tan(collisionsInf.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}
				
				collisionsInf.below = directionY == -1; // If collided with smth below -> true
				collisionsInf.above = directionY == 1; // If collided with smth above -> true
			}
		}
		if(collisionsInf.climbingSlope)
		{
			float directionX = Mathf.Sign(velocity.x);
			rayLenght = Mathf.Abs(velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin,Vector2.right * directionX,rayLenght,collisionMask);

			if(hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal,Vector2.up);
				if ( slopeAngle != collisionsInf.slopeAngle)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisionsInf.slopeAngle = slopeAngle;
				}
			}
		}
	}
	
	void HorizontalCollisions(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign(velocity.x);
		float rayLenght = Mathf.Abs(velocity.x) + skinWidth;

		if(facingRight && directionX == -1) ChangeDir();
		else if (!facingRight && directionX == 1) ChangeDir();
		
		for(int i = 0; i <horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i) ; 
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin,Vector2.right * directionX,rayLenght,collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX,Color.red);

			if(hit)
			{
				if(hit.distance == 0)
				{
					continue;
				}

				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				if(i == 0 &&  slopeAngle <= maxSlopeAngle)
				{	
					if(collisionsInf.descendingSlope)
					{
						collisionsInf.descendingSlope = false;
						velocity = collisionsInf.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if(slopeAngle != collisionsInf.slopeAngleOld)
					{
						distanceToSlopeStart = hit.distance-skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}
				if(!collisionsInf.climbingSlope || slopeAngle > maxSlopeAngle)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLenght = hit.distance;

					if(collisionsInf.climbingSlope)		
					{
						velocity.y = Mathf.Tan(collisionsInf.slopeAngle * Mathf.Deg2Rad * Mathf.Abs(velocity.x));
					}
					collisionsInf.left = directionX == -1; // If collided with smth on the left -> true
					collisionsInf.right = directionX == 1; // If collided with smth on the right -> true
				}
				if(hit.collider.CompareTag("Push"))
				{
					if(hit.transform.GetComponent<Rigidbody2D>().velocity.x < 0.7f)
						hit.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(7f,0));
				}
			}
		}
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle)
	{
		float moveDistace = Mathf.Abs(velocity.x);
		float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistace;

		if(velocity.y <= climbVelocityY)
		{		
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistace * Mathf.Sign(velocity.x);

			collisionsInf.below = true;
			collisionsInf.climbingSlope = true;
			collisionsInf.slopeAngle = slopeAngle;
		}
	}
	void DescendSlope(ref Vector3 velocity)
	{

		
		float directionX = Mathf.Sign(velocity.x);
		Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomRight:raycastOrigins.bottomLeft);
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin,-Vector2.up,Mathf.Infinity,collisionMask);
		
		if(hit)
		{
			float slopeAngle = Vector2.Angle(hit.normal,Vector2.up);
			if(slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
			{
				if(Mathf.Sign(hit.normal.x) == directionX)
				{
					if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
					{
						float moveDistace = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistace;
						velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistace * Mathf.Sign(velocity.x);
						velocity.y -= descendVelocityY;

						collisionsInf.slopeAngle = slopeAngle;
						collisionsInf.descendingSlope = true;
						collisionsInf.below = true;
					}
				}
			}
		}
	}

	public void ChangeDir()
	{
		facingRight = !facingRight;
	}

	public struct CollisionInfo
	{
		public bool above, below;
		public bool left,right;

		public bool climbingSlope,descendingSlope;
		public float slopeAngle, slopeAngleOld;

		public Vector3 velocityOld;

		public void ResetColl()
		{
			above = below = false;
			left = right = false;

			climbingSlope = false;
			descendingSlope = false;
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

}
