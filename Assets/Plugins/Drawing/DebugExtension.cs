using UnityEngine;
using System.Collections.Generic;
using System;

public static class Log {
	public static void Array<T> ( T[] array ) {
		if( array == null ) return;
		Debug.Log( string.Join( ", ", array ) );
	}
	public static string ToArrayString<T> ( T[] array ) {
		if( array == null ) return "";
		return string.Join( ", ", array );
	}
}

public static class Draw {

	struct DrawParameters {
		public Color color;
		public float duration;
		public bool depthTest;

		public DrawParameters ( Color color, float duration, bool depthTest ) {
			this.color = color;
			this.duration = duration;
			this.depthTest = depthTest;
		}
	}

	public static Color color = Color.red;
	public static float duration = 0;
	public static bool depthTest = false;
	public static Matrix4x4 matrix = Matrix4x4.identity;

	public static void Reset () {
		color = Color.red;
		duration = 0;
		depthTest = false;
		matrix = Matrix4x4.identity;
	}



	// RAY METHODS //
	public static void Ray ( Vector3 origin, Vector3 direction ) {
		Debug.DrawRay( matrix.MultiplyPoint( origin ), matrix.MultiplyVector( direction ), color, duration, depthTest );
	}
	public static void Ray ( Vector3 origin, Vector3 direction, Color color ) {
		var c = Draw.color;
		Draw.color = color;
		Ray( origin, direction );
		Draw.color = c;
	}
	public static void Ray ( Vector3 origin, Vector3 direction, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Ray( origin, direction );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Ray ( Vector3 origin, Vector3 direction, float duration ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Ray( origin, direction );

		Draw.duration = d;
	}


	// LINE METHODS //
	public static void Line ( Vector3 start, Vector3 end ) {
		Debug.DrawLine( matrix.MultiplyPoint( start ), matrix.MultiplyPoint( end ), color, duration, depthTest );
	}
	public static void Line ( Vector3 start, Vector3 end, Color color ) {
		var c = Draw.color;
		Draw.color = color;

		Line( start, end );
	}
	public static void Line ( Vector3 start, Vector3 end, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Line( start, end );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Line ( Vector3 start, Vector3 end, float duration ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Line( start, end );

		Draw.duration = d;
	}


	// POINT METHODS //
	public static void Point ( Vector3 position, Color color ) {
		var c = Draw.color;
		Draw.color = color;

		Point( position, 0.5f );

		Draw.color = c;
	}
	public static void Point ( Vector3 position, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Point( position, 0.5f );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Point ( Vector3 position, float radius, Color color ) {
		var c = Draw.color;
		Draw.color = color;

		Point( position, radius );

		Draw.color = c;
	}
	public static void Point ( Vector3 position, float radius, float duration ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Point( position, radius );

		Draw.duration = d;
	}
	public static void Point ( Vector3 position, float radius, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Point( position, radius );

		Draw.color = c;
		Draw.duration = d;
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
		var c = Draw.color;
		Draw.color = color;

		Bounds( bounds );

		Draw.color = c;
	}
	public static void Bounds ( Bounds bounds, float duration ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Bounds( bounds );

		Draw.duration = d;
	}
	public static void Bounds ( Bounds bounds, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Bounds( bounds );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Bounds ( Bounds bounds ) {
		Vector3 center = bounds.center;

		float x = bounds.extents.x;
		float y = bounds.extents.y;
		float z = bounds.extents.z;

		Vector3 p1 = center + new Vector3( x, y, z );
		Vector3 p2 = center + new Vector3( x, y, -z );
		Vector3 p3 = center + new Vector3( -x, y, z );
		Vector3 p4 = center + new Vector3( -x, y, -z );

		Vector3 p5 = center + new Vector3( x, -y, z );
		Vector3 p6 = center + new Vector3( x, -y, -z );
		Vector3 p7 = center + new Vector3( -x, -y, z );
		Vector3 p8 = center + new Vector3( -x, -y, -z );

		Line( p1, p3 );
		Line( p1, p2 );
		Line( p3, p4 );
		Line( p2, p4 );

		Line( p1, p5 );
		Line( p2, p6 );
		Line( p3, p7 );
		Line( p4, p8 );

		Line( p5, p7 );
		Line( p5, p6 );
		Line( p7, p8 );
		Line( p8, p6 );
	}

	public static void Bounds ( Vector3 center, Vector3 size, Color color ) {
		var c = Draw.color;
		Draw.color = color;

		Bounds( center, size );

		Draw.color = c;
	}
	public static void Bounds ( Vector3 center, Vector3 size, float duration ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Bounds( center, size );

		Draw.duration = d;
	}
	public static void Bounds ( Vector3 center, Vector3 size, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Bounds( center, size );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Bounds ( Vector3 center, Vector3 size ) {
		float x = size.x * 0.5f;
		float y = size.y * 0.5f;
		float z = size.z * 0.5f;

		Vector3 p1 = center + new Vector3( x, y, z );
		Vector3 p2 = center + new Vector3( x, y, -z );
		Vector3 p3 = center + new Vector3( -x, y, z );
		Vector3 p4 = center + new Vector3( -x, y, -z );

		Vector3 p5 = center + new Vector3( x, -y, z );
		Vector3 p6 = center + new Vector3( x, -y, -z );
		Vector3 p7 = center + new Vector3( -x, -y, z );
		Vector3 p8 = center + new Vector3( -x, -y, -z );

		Line( p1, p3 );
		Line( p1, p2 );
		Line( p3, p4 );
		Line( p2, p4 );

		Line( p1, p5 );
		Line( p2, p6 );
		Line( p3, p7 );
		Line( p4, p8 );

		Line( p5, p7 );
		Line( p5, p6 );
		Line( p7, p8 );
		Line( p8, p6 );
	}



	// RECT METHODS //
	public static void Rect ( Vector3 center, Vector2 size, Color color ) {
		var c = Draw.color;
		Draw.color = color;

		Rect( center, size, Vector3.forward, Vector3.up );

		Draw.color = c;
	}
	public static void Rect ( Vector3 center, Vector2 size, float duration ) {
		var d = Draw.duration;
		Draw.duration = duration;
		Rect( center, size, Vector3.forward, Vector3.up );

		Draw.duration = d;
	}
	public static void Rect ( Vector3 center, Vector2 size, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Rect( center, size, Vector3.forward, Vector3.up );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward, Color color ) {
		var c = Draw.color;
		Draw.color = color;

		Rect( center, size, forward, Vector3.up );

		Draw.color = c;
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward, float duration ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Rect( center, size, forward, Vector3.up );

		Draw.duration = d;
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Rect( center, size, forward, Vector3.up );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward, Vector3 up, Color color ) {
		var c = Draw.color;
		Draw.color = color;

		Rect( center, size, forward, up );

		Draw.color = c;
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward, Vector3 up, float duration ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Rect( center, size, forward, up );

		Draw.duration = d;
	}
	public static void Rect ( Vector3 center, Vector2 size, Vector3 forward, Vector3 up, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Rect( center, size, forward, up );

		Draw.color = c;
		Draw.duration = d;
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

		var rightExtent = size.x * 0.5f;
		var upExtent = size.y * 0.5f;
		var p1 = rotation * new Vector3( center.x - rightExtent, center.y - upExtent, center.z );
		var p2 = rotation * new Vector3( center.x - rightExtent, center.y + upExtent, center.z );
		var p3 = rotation * new Vector3( center.x + rightExtent, center.y + upExtent, center.z );
		var p4 = rotation * new Vector3( center.x + rightExtent, center.y - upExtent, center.z );

		Line( p1, p2 );
		Line( p2, p3 );
		Line( p3, p4 );
		Line( p4, p1 );
	}

	public static void Rect ( Rect rect, Color color ) {
		var c = Draw.color;
		Draw.color = color;

		Rect( rect.center, rect.size, Vector3.forward, Vector3.up );

		Draw.color = c;
	}
	public static void Rect ( Rect rect, float duration ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Rect( rect.center, rect.size, Vector3.forward, Vector3.up );

		Draw.duration = d;
	}
	public static void Rect ( Rect rect, Color color, float duration ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Rect( rect.center, rect.size, Vector3.forward, Vector3.up );

		Draw.color = c;
		Draw.duration = d;
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
		var c = Draw.color;
		Draw.color = color;

		Circle( center, 0.5f, Vector3.forward, numSegments );

		Draw.color = c;
	}
	public static void Circle ( Vector3 center, Color color, float duration, int numSegments = defaultCircleSegmentCount ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Circle( center, 0.5f, Vector3.forward, numSegments );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Circle ( Vector3 center, float radius, Color color, int numSegments = defaultCircleSegmentCount ) {
		var c = Draw.color;
		Draw.color = color;

		Circle( center, radius, Vector3.forward, numSegments );

		Draw.color = c;
	}
	public static void Circle ( Vector3 center, float radius, float duration, int numSegments = defaultCircleSegmentCount ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Circle( center, radius, Vector3.forward, numSegments );

		Draw.duration = d;
	}
	public static void Circle ( Vector3 center, float radius, Color color, float duration, int numSegments = defaultCircleSegmentCount ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Circle( center, radius, Vector3.forward, numSegments );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Circle ( Vector3 center, int numSegments = defaultCircleSegmentCount ) {
		Circle( center, 0.5f, Vector3.forward, numSegments );
	}
	public static void Circle ( Vector3 center, float radius, int numSegments = defaultCircleSegmentCount ) {
		Circle( center, radius, Vector3.forward, numSegments );
	}
	public static void Circle ( Vector3 center, float radius, Vector3 forward, int numSegments = defaultCircleSegmentCount ) {
		if( numSegments == 0 ) throw new ArgumentException( "numSegments cannot be 0.", "numSegments" );

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
			Line( lastPoint, point );
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
		if( numSegments == 0 ) throw new ArgumentException( "numSegments cannot be 0.", "numSegments" );

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
			Line( lastPoint, point );
		}
	}



	// SPHERE METHODS //
	public static void Sphere ( Vector3 center, Color color, int numSegments = defaultCircleSegmentCount ) {
		var c = Draw.color;
		Draw.color = color;

		Sphere( center, 0.5f, numSegments );

		Draw.color = c;
	}
	public static void Sphere ( Vector3 center, Color color, float duration, int numSegments = defaultCircleSegmentCount ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Sphere( center, 0.5f, numSegments );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Sphere ( Vector3 center, float radius, Color color, int numSegments = defaultCircleSegmentCount ) {
		var c = Draw.color;
		Draw.color = color;

		Sphere( center, radius, numSegments );

		Draw.color = c;
	}
	public static void Sphere ( Vector3 center, float radius, float duration, int numSegments = defaultCircleSegmentCount ) {
		var d = Draw.duration;
		Draw.duration = duration;

		Sphere( center, radius, numSegments );

		Draw.duration = d;
	}
	public static void Sphere ( Vector3 center, float radius, Color color, float duration, int numSegments = defaultCircleSegmentCount ) {
		var c = Draw.color;
		Draw.color = color;
		var d = Draw.duration;
		Draw.duration = duration;

		Sphere( center, radius, numSegments );

		Draw.color = c;
		Draw.duration = d;
	}
	public static void Sphere ( Vector3 center, int numSegments = defaultCircleSegmentCount ) {
		Sphere( center, 0.5f, numSegments );
	}
	public static void Sphere ( Vector3 center, float radius, int numSegments = defaultCircleSegmentCount ) {
		if( numSegments == 0 ) throw new ArgumentException( "numSegments cannot be 0.", "numSegments" );

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

			Line( lX, pX );
			Line( lY, pY );
			Line( lZ, pZ );
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

		Ray( position, slerpedVector.normalized * dist );
		Ray( position, Vector3.Slerp( _forward, -_up, angle / 90.0f ).normalized * dist );
		Ray( position, Vector3.Slerp( _forward, _right, angle / 90.0f ).normalized * dist );
		Ray( position, Vector3.Slerp( _forward, -_right, angle / 90.0f ).normalized * dist );

		Circle( position + _forward, ( _forward - ( slerpedVector.normalized * dist ) ).magnitude, direction );
		Circle( position + ( _forward * 0.5f ), ( ( _forward * 0.5f ) - ( slerpedVector.normalized * ( dist * 0.5f ) ) ).magnitude, direction );
	}



	// ARROW METHODS //
	// -- incomplete
	public static void Arrow ( Vector3 position, Vector3 direction, float angle = 45 ) {
		Ray( position, direction );
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
