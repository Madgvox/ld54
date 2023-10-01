using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public struct GemLayerComponents {
    public GemLayerGraphics gfx;
    public SpriteRenderer separator;
}

public class Gem : MonoBehaviour {

    public Vector2Int gridPosition;

    public GemLayerGraphics graphicPrefab;
    public SpriteRenderer blockerPrefab;
    public Transform layerContainer;
    public Color blockerColor;

    public float maxScale = 1f;
    public float minScale = 0.4f;

    public float separatorScale = 0.05f;

    public List<GemLayer> gemLayers;
    List<GemLayerComponents> gemLayerGraphics = new();

    internal void InitializeLayers () {
        foreach (var gfx in gemLayerGraphics) {
            Destroy( gfx.gfx.gameObject );
            if( gfx.separator ) {
                Destroy( gfx.separator.gameObject );
            }
        }
        gemLayerGraphics.Clear();

        for( int i = 0; i < gemLayers.Count; i++ ) {
            var gemLayer = gemLayers[ i ];
            var inst = Instantiate( graphicPrefab, layerContainer );
            inst.transform.localPosition = Vector2.zero;
            float scale = Mathf.Lerp( maxScale, minScale, i / (float)gemLayers.Count );
            inst.transform.localScale = new Vector3( scale, scale, 1 );
            inst.SetLayer( gemLayer, i * 2 );
            var components = new GemLayerComponents() {
                gfx = inst
            };
            if( i > 0 ) {
                var separator = Instantiate( blockerPrefab, layerContainer );
                separator.transform.localPosition = Vector2.zero;
                separator.transform.localScale = new Vector3( scale + separatorScale, scale + separatorScale, 1);
                var color = new Color( 0, 0, 0, 0.15f );
                separator.color = color;
                separator.sortingOrder = i * 2 - 1;
                components.separator = separator;
            }
            gemLayerGraphics.Add( components );
        }

        var blocker = Instantiate( blockerPrefab, layerContainer );
        blocker.transform.localPosition = Vector2.zero;
        float blockerScale = minScale;
        blocker.transform.localScale = new Vector3( blockerScale, blockerScale, 1 );
        blocker.color = blockerColor;
        blocker.sortingOrder = gemLayers.Count * 2 + 1;
    }

    public bool PopLayer () {
        gemLayers.RemoveAt( 0 );

        if( gemLayers.Count > 0 ) {
            InitializeLayers();
            return false;
        } else {
            Destroy( gameObject );
            return true;
        }
    }

    internal void SetPositionImmediate ( int x, int y ) {
        gridPosition = new Vector2Int( x, y );
        transform.position = new Vector3( x, y );
    }

    internal bool PopLayerAnimated ( int triggeringGemType ) {
        var layerToRemove = gemLayerGraphics[ 0 ];
        DestroyLayer( layerToRemove, triggeringGemType );
        gemLayerGraphics.RemoveAt( 0 );
        
        gemLayers.RemoveAt( 0 );

        if( gemLayers.Count > 0 ) {
            PopLayerAnimation();
            // InitializeLayers();
            return false;
        } else {
            // destroy animation
            
            Destroy( gameObject, 0.2f );
            return true;
        }
    }

    void DestroyLayer ( GemLayerComponents components, int triggeringGemType ) {
        var gemColor = Game.instance.gemTypes.GetColor( triggeringGemType );
        
        var clear = new Color( 1, 1, 1, 0 );

        var topSeq = DOTween.Sequence();
        topSeq.Append( components.gfx.top.DOColor( gemColor, 0.05f ) );
        topSeq.Append( components.gfx.top.DOColor( clear, 0.1f ) );

        var leftSeq = DOTween.Sequence();
        leftSeq.Append( components.gfx.left.DOColor( gemColor, 0.05f ) );
        leftSeq.Append( components.gfx.left.DOColor( clear, 0.1f ) );

        var rightSeq = DOTween.Sequence();
        rightSeq.Append( components.gfx.right.DOColor( gemColor, 0.05f ) );
        rightSeq.Append( components.gfx.right.DOColor( clear, 0.1f ) );

        var bottomSeq = DOTween.Sequence();
        bottomSeq.Append( components.gfx.bottom.DOColor( gemColor, 0.05f ) );
        bottomSeq.Append( components.gfx.bottom.DOColor( clear, 0.1f ) );
        
        components.gfx.transform.DOScale( 2f, 0.2f );
        Destroy( components.gfx.gameObject, 0.2f );
        if( components.separator ) {
            Destroy( components.separator.gameObject, 0.2f );
        }
    }

    void PopLayerAnimation () {
        var duration = 0.5f;

        for( int i = 0; i < gemLayers.Count; i++ ) {
            float scale = Mathf.Lerp( maxScale, minScale, i / (float)gemLayers.Count );

            var components = gemLayerGraphics[ i ];

            components.gfx.transform.DOScale(scale, duration);

            if( components.separator ) {
                if( i != 0 ) {
                    components.separator.transform.DOScale(scale + separatorScale, duration);
                } else {
                    Destroy( components.separator.gameObject, duration );
                }
            }
        }
    }
}
