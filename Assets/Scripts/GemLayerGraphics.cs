using System;
using UnityEngine;
using UnityEngine.Rendering;

public class GemLayerGraphics : MonoBehaviour {
    public SpriteRenderer top;
    public SpriteRenderer right;
    public SpriteRenderer bottom;
    public SpriteRenderer left;
    public SortingGroup group;
    
    internal void SetLayer ( GemLayer gemLayer, int sortingOrder ) {
        top.color = Game.instance.gemTypes.GetColor( gemLayer.top );
        right.color = Game.instance.gemTypes.GetColor( gemLayer.right );
        bottom.color = Game.instance.gemTypes.GetColor( gemLayer.bottom );
        left.color = Game.instance.gemTypes.GetColor( gemLayer.left );
        group.sortingOrder = sortingOrder;
    }
}
