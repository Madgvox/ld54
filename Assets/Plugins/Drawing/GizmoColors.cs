using UnityEngine;

public static class GizmoColors {
	public static readonly Color defaultColor = Color.red;
	public static readonly Color xAxis = new Color32( 255, 77, 32, 255 );
	public static readonly Color xAxisMuted = new Color32( 178, 55, 24, 127 );
	public static readonly Color yAxis = new Color32( 176, 242, 83, 255 );
	public static readonly Color yAxisMuted = new Color32( 123, 178, 45, 127 );
	public static readonly Color zAxis = new Color32( 82, 160, 255, 255 );
	public static readonly Color zAxisMuted = new Color32( 50, 108, 178, 127 );
	public static readonly Color outline = new Color( 0.8f, 0.8f, 0.8f, 0.8f );
	public static readonly Color wireframe = new Color32( 206, 135, 255, 255 );
	public static readonly Color selected = new Color32( 248, 254, 55, 255 );

	public static Color HexToColor ( uint hex ) {
		var hexString = hex.ToString( "X6" );
		if( hexString.Length != 6 && hexString.Length != 8 ) {
			Debug.LogError( "Malformed hex color: " + hexString );
			return new Color32( 255, 0, 255, 255 ); // error pink

		}
		byte a = 255;
		if( hexString.Length == 8 ) { // use alpha
			a = (byte)( hex & 255 );
			hex >>= 8;
		}
		byte r = (byte)( ( hex >> 16 ) & 255 );
		byte g = (byte)( ( hex >> 8 ) & 255 );
		byte b = (byte)( hex & 255 );

		return new Color32( r, g, b, a );
	}
}
