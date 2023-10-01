using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;

public static class Gizmo {
	
	public class DrawingScope : IDisposable {
		Color color;
		Matrix4x4 matrix;

		public DrawingScope ( Color color ) {
			this.color = Gizmos.color;
			this.matrix = Gizmos.matrix;
			Gizmos.color = color;
		}

		public DrawingScope ( Matrix4x4 matrix ) {
			this.color = Gizmos.color;
			this.matrix = Gizmos.matrix;
			Gizmos.matrix = matrix;
		}

		public DrawingScope ( Color color, Matrix4x4 matrix ) {
			this.color = Gizmos.color;
			Gizmos.color = color;
			this.matrix = Gizmos.matrix;
			Gizmos.matrix = matrix;
		}

		public void Dispose () {
			Gizmos.color = color;
			Gizmos.matrix = matrix;
		}
	}

	// RAY METHODS //
	public static void Ray ( Vector3 origin, Vector3 direction ) {
		Gizmos.DrawRay( origin, direction );
	}
	public static void Ray ( Vector3 origin, Vector3 direction, Color color ) {
		using( new DrawingScope( color ) )  Gizmos.DrawRay( origin, direction );
	}


	// LINE METHODS //
	public static void Line ( Vector3 start, Vector3 end ) {
		Gizmos.DrawLine( start, end );
	}
	public static void Line ( Vector3 start, Vector3 end, Color color ) {
		using( new DrawingScope( color ) )  Gizmos.DrawLine( start, end );
	}


	// POINT METHODS //
	public static void Point ( Vector3 position, Color color ) {
		Point( position, 0.5f );
	}
	public static void Point ( Vector3 position, float radius, Color color ) {
		using( new DrawingScope( color ) )  Point( position, radius );
	}
	public static void Point ( Vector3 position ) {
		Point( position, 0.5f );
	}
	public static void Point ( Vector3 position, float radius ) {
		var up = Vector3.up * radius;
		var right = Vector3.right * radius;
		var forward = Vector3.forward * radius;

		Line( position - up, position + up );
		Line( position - right, position + right );
		Line( position - forward, position + forward );
	}


	// BOUNDS METHODS //
	public static void Bounds ( Bounds bounds, Color color ) {
		using( new DrawingScope( color ) )  Bounds( bounds );
	}
	public static void Bounds ( Bounds bounds ) {
		var center = bounds.center;

		var x = bounds.extents.x;
		var y = bounds.extents.y;
		var z = bounds.extents.z;

		var p1 = new Vector3( center.x + x, center.y + y, center.z + z );
		var p8 = new Vector3( center.x - x, center.y - y, center.z - z );

		var p2 = new Vector3( p1.x, p1.y, p8.z );
		var p3 = new Vector3( p8.x, p1.y, p1.z );
		var p4 = new Vector3( p8.x, p1.y, p8.z );

		var p5 = new Vector3( p1.x, p8.y, p1.z );
		var p6 = new Vector3( p1.x, p8.y, p8.z );
		var p7 = new Vector3( p8.x, p8.y, p1.z );

		Gizmos.DrawLine( p1, p3 );
		Gizmos.DrawLine( p1, p2 );
		Gizmos.DrawLine( p3, p4 );
		Gizmos.DrawLine( p2, p4 );

		Gizmos.DrawLine( p1, p5 );
		Gizmos.DrawLine( p2, p6 );
		Gizmos.DrawLine( p3, p7 );
		Gizmos.DrawLine( p4, p8 );

		Gizmos.DrawLine( p5, p7 );
		Gizmos.DrawLine( p5, p6 );
		Gizmos.DrawLine( p7, p8 );
		Gizmos.DrawLine( p8, p6 );
	}

	public static void Bounds ( Vector3 center, Vector3 size, Color color ) {
		Bounds( center, size );
	}
	public static void Bounds ( Vector3 center, Vector3 size, Color color, float duration ) {
		Bounds( center, size );
	}
	public static void Bounds ( Vector3 center, Vector3 size ) {
		float x = size.x * 0.5f;
		float y = size.y * 0.5f;
		float z = size.z * 0.5f;

		var p1 = new Vector3( center.x + x, center.y + y, center.z + z );
		var p8 = new Vector3( center.x - x, center.y - y, center.z - z );

		var p2 = new Vector3( p1.x, p1.y, p8.z );
		var p3 = new Vector3( p8.x, p1.y, p1.z );
		var p4 = new Vector3( p8.x, p1.y, p8.z );

		var p5 = new Vector3( p1.x, p8.z, p1.z );
		var p6 = new Vector3( p1.x, p8.z, p8.z );
		var p7 = new Vector3( p8.x, p8.z, p1.z );

		Gizmos.DrawLine( p1, p3 );
		Gizmos.DrawLine( p1, p2 );
		Gizmos.DrawLine( p3, p4 );
		Gizmos.DrawLine( p2, p4 );

		Gizmos.DrawLine( p1, p5 );
		Gizmos.DrawLine( p2, p6 );
		Gizmos.DrawLine( p3, p7 );
		Gizmos.DrawLine( p4, p8 );

		Gizmos.DrawLine( p5, p7 );
		Gizmos.DrawLine( p5, p6 );
		Gizmos.DrawLine( p7, p8 );
		Gizmos.DrawLine( p8, p6 );
	}



	// RECT METHODS //
	public static void Rect ( Vector3 center, Vector2 size, Color color ) {
		using( new DrawingScope( color ) )  Rect( center, size, Vector3.forward, Vector3.up );
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward, Color color ) {
		using( new DrawingScope( color ) )  Rect( center, size, forward, Vector3.up );
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward, Vector3 up, Color color ) {
		using( new DrawingScope( color ) )  Rect( center, size, forward, up );
	}
	public static void Rect ( Vector3 center, Vector2 size ) {
		Rect( center, size, Vector3.forward, Vector3.up );
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward ) {
		Rect( center, size, forward, Vector3.up );
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward, Vector3 up ) {
		if( up.Equals( Vector3.zero ) ) {
			up = Vector3.up;
		}

		var rotation = Quaternion.LookRotation( forward, up );

		var rightExtent = rotation * new Vector3( size.x * 0.5f, 0, 0 );
		var upExtent    = rotation * new Vector3( 0, size.y * 0.5f, 0 );
		var p1 = new Vector3( center.x - rightExtent.x - upExtent.x, center.y - rightExtent.y - upExtent.y, center.z - rightExtent.z - upExtent.z );
		var p2 = new Vector3( center.x - rightExtent.x + upExtent.x, center.y - rightExtent.y + upExtent.y, center.z - rightExtent.z + upExtent.z );
		var p3 = new Vector3( center.x + rightExtent.x + upExtent.x, center.y + rightExtent.y + upExtent.y, center.z + rightExtent.z + upExtent.z );
		var p4 = new Vector3( center.x + rightExtent.x - upExtent.x, center.y + rightExtent.y - upExtent.y, center.z + rightExtent.z - upExtent.z );

		Gizmos.DrawLine( p1, p2 );
		Gizmos.DrawLine( p2, p3 );
		Gizmos.DrawLine( p3, p4 );
		Gizmos.DrawLine( p4, p1 );
	}

	public static void Rect ( Rect rect, Color color ) {
		using( new DrawingScope( color ) ) Rect( rect.center, rect.size, Vector3.forward, Vector3.up );
	}
	public static void Rect ( Rect rect ) {
		Rect( rect.center, rect.size, Vector3.forward, Vector3.up );
	}
	public static void Rect ( Rect rect, Vector3 forward ) {
		Rect( rect.center, rect.size, forward, Vector3.up );
	}
	public static void Rect ( Rect rect, Vector3 forward, Vector3 up ) {
		Rect( rect.center, rect.size, forward, up );
	}


	// CIRCLE METHODS //
	const int defaultCircleSegmentCount = 32;
	public static void Circle ( Vector3 center, Color color, int numSegments = defaultCircleSegmentCount ) {
		using( new DrawingScope( color ) )  Circle( center, 0.5f, Vector3.forward, numSegments );
	}
	public static void Circle ( Vector3 center, float radius, Color color, int numSegments = defaultCircleSegmentCount ) {
		using( new DrawingScope( color ) )  Circle( center, radius, Vector3.forward, numSegments );
	}
	public static void Circle ( Vector3 center, int numSegments = defaultCircleSegmentCount ) {
		Circle( center, 0.5f, Vector3.forward, numSegments );
	}
	public static void Circle ( Vector3 center, float radius, int numSegments = defaultCircleSegmentCount ) {
		Circle( center, radius, Vector3.forward, numSegments );
	}
	public static void Circle ( Vector3 center, float radius, Vector3 forward, Color color, int numSegments = defaultCircleSegmentCount ) {
		using( new DrawingScope( color ) )  Circle( center, radius, forward, numSegments );
	}
	public static void Circle ( Vector3 center, float radius, Vector3 forward, int numSegments = defaultCircleSegmentCount ) {
		if( numSegments <= 1 ) return;

		var cross = Vector3.Cross( forward, Vector3.up ).normalized;
		if( cross.Equals( Vector3.zero ) ) {
			cross = Vector3.Cross( forward, Vector3.right ).normalized;
		}

		var rotateBy = Quaternion.AngleAxis( 360f / numSegments, forward );
		var d = cross * radius;

		var lastPoint = center + d;
		for( int i = 0; i < numSegments; i++ ) {
			d = rotateBy * d;
			var point = center + d;
			Gizmos.DrawLine( lastPoint, point );
			lastPoint = point;
		}
	}



	// ARC METHODS //
	public static void Arc ( Vector3 center, float radius, float angle, int numSegments = defaultCircleSegmentCount ) {
		Arc( center, radius, angle, 0, Vector3.forward, numSegments );
	}
	public static void Arc ( Vector3 center, float radius, float angle, float offset, int numSegments = defaultCircleSegmentCount ) {
		Arc( center, radius, angle, offset, Vector3.forward, numSegments );
	}
	public static void Arc ( Vector3 center, float radius, float angle, float offset, Vector3 forward, int numSegments = defaultCircleSegmentCount ) {
		if( numSegments <= 1 ) return;

		var cross = Vector3.Cross( forward, Vector3.up ).normalized;
		if( cross.Equals( Vector3.zero ) ) {
			cross = Vector3.Cross( forward, Vector3.right ).normalized;
		}

		var rotateBy = Quaternion.AngleAxis( angle / numSegments, forward );
		var beginningOffset = Quaternion.AngleAxis( offset, forward );
		var d = cross * radius;
		d = beginningOffset * d;

		var lastPoint = center + d;
		for( int i = 0; i < numSegments; i++ ) {
			d = rotateBy * d;
			var point = center + d;
			Gizmos.DrawLine( lastPoint, point );
		}
	}



	// SPHERE METHODS //
	public static void Sphere ( Vector3 center, Color color, int numSegments = defaultCircleSegmentCount ) {
		using( new DrawingScope( color ) )  Sphere( center, 0.5f, numSegments );
	}
	public static void Sphere ( Vector3 center, float radius, Color color, int numSegments = defaultCircleSegmentCount ) {
		using( new DrawingScope( color ) )  Sphere( center, radius, numSegments );
	}
	public static void Sphere ( Vector3 center, int numSegments = defaultCircleSegmentCount ) {
		Sphere( center, 0.5f, numSegments );
	}
	public static void Sphere ( Vector3 center, float radius, int numSegments = defaultCircleSegmentCount ) {
		if( numSegments <= 1 ) return;

		var step = 360f / numSegments;
		var rotateX = Quaternion.AngleAxis( step, Vector3.right );
		var rotateY = Quaternion.AngleAxis( step, Vector3.up );
		var rotateZ = Quaternion.AngleAxis( step, Vector3.forward );
		var dX = Vector3.up * radius;
		var dY = Vector3.forward * radius;
		var dZ = Vector3.up * radius;

		var lX = center + dX;
		var lY = center + dY;
		var lZ = center + dZ;
		for( int i = 0; i < numSegments; i++ ) {
			dX = rotateX * dX;
			dY = rotateY * dY;
			dZ = rotateZ * dZ;

			var pX = center + dX;
			var pY = center + dY;
			var pZ = center + dZ;

			Gizmos.DrawLine( lX, pX );
			Gizmos.DrawLine( lY, pY );
			Gizmos.DrawLine( lZ, pZ );
		}
	}



	// CYLINDER METHODS //
	// -- incomplete

	//public static void Cylinder ( Vector3 start, Vector3 end, Color color, float radius = 1, float duration = 0, bool depthTest = true ) {
	//	Vector3 up = ( end - start ).normalized * radius;
	//	Vector3 forward = Vector3.Slerp( up, -up, 0.5f );
	//	Vector3 right = Vector3.Cross( up, forward ).normalized * radius;

	//	//Radial circles
	//	Draw.Circle( start, up, color, radius, duration, depthTest );
	//	Draw.Circle( end, -up, color, radius, duration, depthTest );
	//	Draw.Circle( ( start + end ) * 0.5f, up, color, radius, duration, depthTest );

	//	//Side lines
	//	Debug.DrawLine( start + right, end + right, color, duration, depthTest );
	//	Debug.DrawLine( start - right, end - right, color, duration, depthTest );

	//	Debug.DrawLine( start + forward, end + forward, color, duration, depthTest );
	//	Debug.DrawLine( start - forward, end - forward, color, duration, depthTest );

	//	//Start endcap
	//	Debug.DrawLine( start - right, start + right, color, duration, depthTest );
	//	Debug.DrawLine( start - forward, start + forward, color, duration, depthTest );

	//	//End endcap
	//	Debug.DrawLine( end - right, end + right, color, duration, depthTest );
	//	Debug.DrawLine( end - forward, end + forward, color, duration, depthTest );
	//}

	

	
	// CONE METHODS //
	// -- incomplete
	public static void Cone ( Vector3 position, Vector3 direction, float angle = 45 ) {
		float length = direction.magnitude;

		Vector3 _forward = direction;
		Vector3 _up = Vector3.Slerp( _forward, -_forward, 0.5f );
		Vector3 _right = Vector3.Cross( _forward, _up ).normalized * length;

		direction.Normalize();

		Vector3 slerpedVector = Vector3.Slerp( _forward, _up, angle / 90.0f );

		float dist;
		var farPlane = new Plane( -direction, position + _forward );
		var distRay = new Ray( position, slerpedVector );

		farPlane.Raycast( distRay, out dist );

		Gizmos.DrawRay( position, slerpedVector.normalized * dist );
		Gizmos.DrawRay( position, Vector3.Slerp( _forward, -_up, angle / 90.0f ).normalized * dist );
		Gizmos.DrawRay( position, Vector3.Slerp( _forward, _right, angle / 90.0f ).normalized * dist );
		Gizmos.DrawRay( position, Vector3.Slerp( _forward, -_right, angle / 90.0f ).normalized * dist );

		Circle( position + _forward, ( _forward - ( slerpedVector.normalized * dist ) ).magnitude, direction );
		Circle( position + ( _forward * 0.5f ), ( ( _forward * 0.5f ) - ( slerpedVector.normalized * ( dist * 0.5f ) ) ).magnitude, direction );
	}



	// ARROW METHODS //
	// -- incomplete
	public static void Arrow ( Vector3 position, Vector3 direction, float angle = 30 ) {
		Gizmos.DrawRay( position, direction );
		Cone( position + direction, Vector3.ClampMagnitude( -direction * 0.333f, 0.5f ), angle );
	}



	// CAPSULE METHODS //
	// -- incomplete

	//public static void Capsule ( Vector3 start, Vector3 end, Color color, float radius = 1, float duration = 0, bool depthTest = true ) {
	//	Vector3 up = ( end - start ).normalized * radius;
	//	Vector3 forward = Vector3.Slerp( up, -up, 0.5f );
	//	Vector3 right = Vector3.Cross( up, forward ).normalized * radius;

	//	float height = ( start - end ).magnitude;
	//	float sideLength = Mathf.Max( 0, ( height * 0.5f ) - radius );
	//	Vector3 middle = ( end + start ) * 0.5f;

	//	start = middle + ( ( start - middle ).normalized * sideLength );
	//	end = middle + ( ( end - middle ).normalized * sideLength );

	//	//Radial circles
	//	Draw.Circle( start, up, color, radius, duration, depthTest );
	//	Draw.Circle( end, -up, color, radius, duration, depthTest );

	//	//Side lines
	//	Debug.DrawLine( start + right, end + right, color, duration, depthTest );
	//	Debug.DrawLine( start - right, end - right, color, duration, depthTest );

	//	Debug.DrawLine( start + forward, end + forward, color, duration, depthTest );
	//	Debug.DrawLine( start - forward, end - forward, color, duration, depthTest );

	//	for( int i = 1; i < 26; i++ ) {

	//		//Start endcap
	//		Debug.DrawLine( Vector3.Slerp( right, -up, i / 25.0f ) + start, Vector3.Slerp( right, -up, ( i - 1 ) / 25.0f ) + start, color, duration, depthTest );
	//		Debug.DrawLine( Vector3.Slerp( -right, -up, i / 25.0f ) + start, Vector3.Slerp( -right, -up, ( i - 1 ) / 25.0f ) + start, color, duration, depthTest );
	//		Debug.DrawLine( Vector3.Slerp( forward, -up, i / 25.0f ) + start, Vector3.Slerp( forward, -up, ( i - 1 ) / 25.0f ) + start, color, duration, depthTest );
	//		Debug.DrawLine( Vector3.Slerp( -forward, -up, i / 25.0f ) + start, Vector3.Slerp( -forward, -up, ( i - 1 ) / 25.0f ) + start, color, duration, depthTest );

	//		//End endcap
	//		Debug.DrawLine( Vector3.Slerp( right, up, i / 25.0f ) + end, Vector3.Slerp( right, up, ( i - 1 ) / 25.0f ) + end, color, duration, depthTest );
	//		Debug.DrawLine( Vector3.Slerp( -right, up, i / 25.0f ) + end, Vector3.Slerp( -right, up, ( i - 1 ) / 25.0f ) + end, color, duration, depthTest );
	//		Debug.DrawLine( Vector3.Slerp( forward, up, i / 25.0f ) + end, Vector3.Slerp( forward, up, ( i - 1 ) / 25.0f ) + end, color, duration, depthTest );
	//		Debug.DrawLine( Vector3.Slerp( -forward, up, i / 25.0f ) + end, Vector3.Slerp( -forward, up, ( i - 1 ) / 25.0f ) + end, color, duration, depthTest );
	//	}
	//}
}
