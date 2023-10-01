using System;
using UnityEngine;

[Serializable]
public struct GemLayer {
    public int top;
    public int right;
    public int bottom;
    public int left;

    public int this[ int index ] {
        readonly get {
            return index switch {
                0 => top,
                1 => right,
                2 => bottom,
                3 => left,
                _ => throw new IndexOutOfRangeException(),
            };
        }
        set {
            switch( index ) {
                case 0:
                    top = value;
                    break;
                case 1:
                    right = value;
                    break;
                case 2:
                    bottom = value;
                    break;
                case 3:
                    left = value;
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public int this[ Vector2Int index ] {
        readonly get {
            return index switch {
                { x: 0, y: 1 } => top,
                { x: 1, y: 0 } => right,
                { x: 0, y: -1 } => bottom,
                { x: -1, y: 0 } => left,
                _ => throw new ArgumentException(),
            };
        }
        set {
            if( index.x == 0 ) {
                if( index.y > 0 ) {
                    top = value;
                } else {
                    bottom = value;
                }
            } else {
                if( index.x > 0 ) {
                    right = value;
                } else {
                    left = value;
                }
            }
        }
    }
}
