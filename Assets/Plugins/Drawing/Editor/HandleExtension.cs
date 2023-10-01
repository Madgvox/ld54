using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

public static class HandleExtension {
	static Vector3[] testingLevelBoundsCache = new Vector3[ 16 ];
	static Vector3[] testingLevelFilledBoundsCache = new Vector3[ 12 ];

	public static void DrawBoundCorners ( Bounds bounds, float size = 0.2f, bool fillSides = false ) {
		var handleSize = size * HandleUtility.GetHandleSize( bounds.center );
		var offsetX = new Vector3( handleSize, 0 );
		var offsetY = new Vector3( 0, handleSize );

		var tl = new Vector3( bounds.min.x, bounds.max.y, bounds.min.z );
		var br = new Vector3( bounds.max.x, bounds.min.y, bounds.min.z );
		var tr = new Vector3( bounds.max.x, bounds.max.y, bounds.min.z );
		var bl = bounds.min;

		Vector3[] cache;

		if( fillSides ) {
			cache = testingLevelFilledBoundsCache;
			cache[ 0 ] = bl;
			cache[ 1 ] = bl + offsetX;

			cache[ 4 ] = tr;
			cache[ 5 ] = tr - offsetX;

			cache[ 8 ] = tl;
			cache[ 9 ] = tl + offsetX;

			cache[ 10 ] = br;
			cache[ 11 ] = br - offsetX;

			cache[ 6 ] = tr;
			cache[ 7 ] = br;

			cache[ 2 ] = bl;
			cache[ 3 ] = tl;
		} else {
			cache = testingLevelBoundsCache;
			cache[ 0 ] = bl;
			cache[ 1 ] = bl + offsetX;

			cache[ 2 ] = bl;
			cache[ 3 ] = bl + offsetY;

			cache[ 4 ] = tr;
			cache[ 5 ] = tr - offsetX;

			cache[ 6 ] = tr;
			cache[ 7 ] = tr - offsetY;


			cache[ 8 ] = tl;
			cache[ 9 ] = tl + offsetX;

			cache[ 10 ] = tl;
			cache[ 11 ] = tl - offsetY;

			cache[ 12 ] = br;
			cache[ 13 ] = br - offsetX;

			cache[ 14 ] = br;
			cache[ 15 ] = br + offsetY;
		}

		Handles.DrawLines( cache );
	}
}