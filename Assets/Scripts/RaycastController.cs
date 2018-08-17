using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {


	public LayerMask collisionMask;
	internal const float skinWidth = 0.015f;
	internal const float distBetweenRays = .25f;
	internal int horizontalRayCount;
	internal int verticalRayCount;

	internal float horizontalRaySpacing;
	internal float verticalRaySpacing;

	internal BoxCollider2D coll2D;
	internal RaycastOrigins raycastOrigins;	

    public virtual void OnEnable()
	{
		coll2D = GetComponent<BoxCollider2D>();		
		CalculateRaySpacing();
	}

	public void UpdateRaycastOrigins()
	{
		Bounds bounds = coll2D.bounds;
		bounds.Expand (skinWidth * -2f);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x,bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x,bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x,bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x,bounds.max.y);		
	}

	public void CalculateRaySpacing() {
		Bounds bounds = coll2D.bounds;
		bounds.Expand (skinWidth * -2);

		float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;



		horizontalRayCount = Mathf.Clamp (Mathf.RoundToInt(boundsHeight/distBetweenRays), 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (Mathf.RoundToInt(boundsWidth/distBetweenRays), 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	[System.Serializable]
	public struct RaycastOrigins
	{
		public Vector2 topLeft,topRight;
		public Vector2 bottomLeft,bottomRight;
	}
}
