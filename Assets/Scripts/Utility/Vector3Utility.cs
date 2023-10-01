using UnityEngine;

public static class Vector3Utility {
	public static Vector2Int DeltaToDirection ( this Vector3 delta ) {
		Vector2Int dir;
		if( delta.y > 0 ) {
			if( delta.y > Mathf.Abs( delta.x ) ) {
				dir = Vector2Int.up;
			} else {
				if( delta.x > 0 ) {
					dir = Vector2Int.right;
				} else {
					dir = Vector2Int.left;
				}
			}
		} else {
			if( delta.y < -Mathf.Abs( delta.x ) ) {
				dir = Vector2Int.down;
			} else {
				if( delta.x > 0 ) {
					dir = Vector2Int.right;
				} else {
					dir = Vector2Int.left;
				}
			}
		}

		return dir;
	}

	public static Vector2Int RotateCCW ( this Vector2Int pos ) {
		if( pos == Vector2Int.up ) {
			return Vector2Int.left;
		} else if( pos == Vector2Int.right ) {
			return Vector2Int.up;
		} else if( pos == Vector2Int.down ) {
			return Vector2Int.right;
		} else if( pos == Vector2Int.left ) {
			return Vector2Int.down;
		} else {
			return pos;
		}
	}

	public static Vector2Int RotateCW ( this Vector2Int pos ) {
		if( pos == Vector2Int.up ) {
			return Vector2Int.right;
		} else if( pos == Vector2Int.right ) {
			return Vector2Int.down;
		} else if( pos == Vector2Int.down ) {
			return Vector2Int.left;
		} else if( pos == Vector2Int.left ) {
			return Vector2Int.up;
		} else {
			return pos;
		}
	}
}