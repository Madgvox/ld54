using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

[Serializable]
public struct GemGenerationTable {
    public int maxLayers;
    public AnimationCurve layerCountProbability;
}

[Serializable]
public struct GemLayerGenerationTable {
    public int maxColors;
    public AnimationCurve colorCountProbability;
    public AnimationCurve absoluteColorSelectionProbability;
    public AnimationCurve relativeColorSelectionProbability;
    public int[] availableColors;
}

public struct HoveredCel {
    public int x;
    public int y;
}

public class MatchChain {
    public List<Gem> gems = new();
    public int gemType;
}

public class Game : MonoBehaviour {
    public static Game instance { get; private set; }

    public Gem gemPrefab;

    public GemTypes gemTypes;
    public ProbabilityTables probabilityTables;

    public int width;
    public int height;

    public GemGenerationTable defaultGenerationTable;
    public GemLayerGenerationTable defaultLayerGenerationTable;

    public Gem thrownGem;

    bool canThrow;
    Vector2Int hoveredTile;
    Vector2Int targetedTile;
    Vector2Int deltaDirection;
    Vector2Int sendDirection;

    public List<Gem> gems;

    bool animating;

    public AnimationCurve shiftCurve;

    void Awake () {
        instance = this;

        width = 2;
        height = 2;

        defaultGenerationTable = probabilityTables.tables[ 2 ].gemGenerationTable;
        defaultLayerGenerationTable = probabilityTables.tables[ 2 ].gemLayerGenerationTable;

        PopulateBoardRandomly();

        thrownGem = GenerateRandomGem( defaultGenerationTable, defaultLayerGenerationTable );

        Application.targetFrameRate = 60;
    }



    void Update () {
        var mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        mousePos.z = 0;

        var boardCenter = new Vector3( width / 2f, height / 2f );

        var delta = mousePos - boardCenter;

        deltaDirection = delta.DeltaToDirection();
        sendDirection = -deltaDirection;

        hoveredTile = new Vector2Int( Mathf.FloorToInt( mousePos.x ), Mathf.FloorToInt( mousePos.y ) );

        if( deltaDirection.x != 0 ) {
            var y = Mathf.Clamp( hoveredTile.y, 0, height - 1 );
            var x = deltaDirection.x > 0 ? width - 1 : 0;

            targetedTile = new Vector2Int( x, y );
        } else {
            var x = Mathf.Clamp( hoveredTile.x, 0, width - 1 );
            var y = deltaDirection.y > 0 ? height - 1 : 0;

            targetedTile = new Vector2Int( x, y );
        }

        Draw.Point( (Vector2)targetedTile );
        Draw.Arrow( (Vector2)targetedTile, (Vector2)sendDirection );

        canThrow = !InGridBounds( hoveredTile );

        if( canThrow ) {
            if( !InGridBoundsX( hoveredTile.x ) && !InGridBoundsY( hoveredTile.y ) ) {
                canThrow = false;
            }
        }

        if( !animating ) {
            UpdateThrownVisualPosition();

            if( canThrow ) {
                ShowThrowGem();
                if( Input.GetMouseButtonDown( 0 ) ) {
                    StartCoroutine( ThrowGemAnimated( targetedTile, sendDirection ) );
                    // ThrowGem( targetedTile, sendDirection );
                }
            } else {
                HideThrowGem();
            }
        }


#if UNITY_EDITOR
        if( Input.GetKeyDown( KeyCode.Space ) ) {
            var allMatched = GatherMatches();
            foreach (var match in allMatched)
            {
                Debug.Log( match.gems.Count + " " + match.gemType );
                foreach (var gem in match.gems)
                {
                    Debug.Log( gem.gridPosition );
                }
            }
        }
#endif
    }

    private void UpdateThrownVisualPosition () {
        var throwPreviewPos = targetedTile + deltaDirection * 2;
        thrownGem.transform.position = new Vector3( throwPreviewPos.x, throwPreviewPos.y );
    }

    private void HideThrowGem () {
        if( thrownGem == null ) return;
        thrownGem.gameObject.SetActive( false );
    }

    private void ShowThrowGem () {
        if( thrownGem == null ) return;
        thrownGem.gameObject.SetActive( true );
    }

    // void ThrowGem ( Vector2Int targetPosition, Vector2Int targetDirection ) {
    //     Gem bumpedGem;
    //     if( targetDirection.x != 0 ) {
    //         bumpedGem = ShiftRow( targetPosition.y, targetDirection.x );
    //     } else {
    //         bumpedGem = ShiftColumn( targetPosition.x, targetDirection.y );
    //     }

    //     if( bumpedGem != null ) {
    //         Destroy( bumpedGem.gameObject );
    //     }

    //     thrownGem.SetPositionImmediate( targetPosition.x, targetPosition.y );
    //     gems.Add( thrownGem );

    //     List<MatchChain> chains;


    //     do {
    //         chains = GatherMatches();
    //         ConsumeMatches( chains );
    //     } while( chains.Count > 0 );

    //     thrownGem = GenerateRandomGem( defaultGenerationTable, defaultLayerGenerationTable );
    //     UpdateThrownVisualPosition();
    // }

    IEnumerator ThrowGemAnimated ( Vector2Int targetPosition, Vector2Int targetDirection ) {
        animating = true;

        var from = thrownGem.transform.position;
        Vector3 to = (Vector2)targetPosition;

        var time = 0f;
        var duration = 0.5f;

        List<Gem> shiftedGems;
        if( targetDirection.x != 0 ) {
            shiftedGems = GrabContiguousRow( targetPosition.y, targetDirection.x );
        } else {
            shiftedGems = GrabContiguousColumn( targetPosition.x, targetDirection.y );
        }
        List<Vector3> startPositions = shiftedGems.Select( s => (Vector3)(Vector2)s.gridPosition ).ToList();

        foreach( var gem in shiftedGems ) {
            gem.gridPosition += targetDirection;
        }

        while( time < duration ) {
            time += Time.deltaTime;

            var p = time / duration;
            p = shiftCurve.Evaluate( p );

            thrownGem.transform.position = Vector3.Lerp( from, to, p );

            if( p > 0.5f ) {
                var shiftP = ( p - 0.5f ) * 2f;
                for( int i = 0; i < shiftedGems.Count; i++ ) {
                    Gem gem = shiftedGems[ i ];
                    Vector3 startPosition = startPositions[ i ];
                    gem.transform.position = Vector3.Lerp( startPosition, (Vector2)gem.gridPosition, shiftP );
                }
            }

            yield return null;
        }

        var bumpedGem = false;

        thrownGem.SetPositionImmediate( targetPosition.x, targetPosition.y );
        foreach( var gem in shiftedGems ) {
            gem.SetPositionImmediate( gem.gridPosition.x, gem.gridPosition.y );
            if( !InGridBounds( gem.gridPosition ) ) {
                gems.Remove( gem );
                gem.layerContainer.transform.DOScale( 0f, 0.2f );
                gem.layerContainer.transform.DORotate(new Vector3( 30f, 30f, 30f ), 0.2f, RotateMode.Fast );

                Destroy( gem.gameObject, 0.2f );

                bumpedGem = true;
            }
        }
        gems.Add( thrownGem );


        if( bumpedGem ) {
            yield return new WaitForSeconds( 0.2f );
        }

        thrownGem = GenerateRandomGem( defaultGenerationTable, defaultLayerGenerationTable );
        UpdateThrownVisualPosition();

        animating = false;

        List<MatchChain> chains;

        do {
            chains = GatherMatches();
            yield return StartCoroutine( ConsumeMatchesAnimated( chains ) );
            yield return new WaitForSeconds( 0.5f );
        } while( chains.Count > 0 );
    }

    IEnumerator ConsumeMatchesAnimated ( List<MatchChain> chains ) {
        // gems can only be popped once per chain!
        var poppedGems = new HashSet<Gem>();
        
        foreach( var chain in chains ) {
            Debug.Log( "Starting chain size: " + chain.gems.Count + " Type: " + chain.gemType );
            for( int i = 0; i < chain.gems.Count; i++ ) {
                Gem gem = chain.gems[ i ];
                if( poppedGems.Add( gem ) ) {
                    Debug.Log( "  " + gem.gridPosition );
                    if( gem.PopLayerAnimated( chain.gemType ) ) {
                        gems.Remove( gem );
                        Destroy( gem.gameObject, 0.5f );
                    }
                }
            }
        }

        yield break;
    }

    void WalkMatches ( Gem gem, Vector2Int facing, int gemType, HashSet<(Vector2Int, Gem)> visited, MatchChain currentChain ) {
        WalkMatch( gem, gemType, visited, facing, currentChain );

        var cwFacing = facing.RotateCW();
        if( gem.gemLayers[ 0 ][ cwFacing ] == gemType ) {
            WalkMatch( gem, gemType, visited, cwFacing, currentChain );

            cwFacing = cwFacing.RotateCW();
            if( gem.gemLayers[ 0 ][ cwFacing ] == gemType ) {
                WalkMatch( gem, gemType, visited, cwFacing, currentChain );
            }
        }

        var ccwFacing = facing.RotateCCW();
        if( gem.gemLayers[ 0 ][ ccwFacing ] == gemType ) {
            WalkMatch( gem, gemType, visited, ccwFacing, currentChain );

            ccwFacing = ccwFacing.RotateCCW();
            if( gem.gemLayers[ 0 ][ ccwFacing ] == gemType ) {
                WalkMatch( gem, gemType, visited, ccwFacing, currentChain );
            }
        }
    }

    private void WalkMatch ( Gem gem, int gemType, HashSet<(Vector2Int, Gem)> visited, Vector2Int facing, MatchChain currentChain ) {
        if( visited.Add( (facing, gem) ) ) {
            var nextPos = gem.gridPosition + facing;
            var next = TryGetGem( nextPos );
            if( next != null ) {
                visited.Add( (-facing, next) );
                if( next.gemLayers[ 0 ][ -facing ] == gemType ) {
                    currentChain.gems.Add( next );
                    WalkMatches( next, -facing, gemType, visited, currentChain );
                }
            }
        }
    }

    private Gem TryGetGem ( Vector2Int pos ) {
        if( InGridBounds( pos ) ) {
            foreach( var gem in gems ) {
                if( gem.gridPosition.x == pos.x && gem.gridPosition.y == pos.y ) {
                    return gem;
                }
            }

            return null;
        } else {
            return null;
        }
    }

    private Gem TryGetGem ( int x, int y ) {
        if( InGridBounds( x, y ) ) {
            foreach( var gem in gems ) {
                if( gem.gridPosition.x == x && gem.gridPosition.y == y ) {
                    return gem;
                }
            }

            return null;
        } else {
            return null;
        }
    }

    private List<MatchChain> GatherMatches () {
        var visited = new HashSet<(Vector2Int, Gem)>();
        var chains = new List<MatchChain>();
        for( int y = 0; y < height; y++ ) {
            for( int x = 0; x < width; x++ ) {
                var gem = TryGetGem( x, y );

                if( gem == null ) continue;

                var chain = new MatchChain() {
                    gemType = gem.gemLayers[ 0 ][ Vector2Int.left ]
                };
                chain.gems.Add( gem );
                WalkMatches( gem, Vector2Int.left, gem.gemLayers[ 0 ][ Vector2Int.left ], visited, chain );

                if( chain.gems.Count > 1 ) {
                    chains.Add( chain );
                }

                chain = new MatchChain() {
                    gemType = gem.gemLayers[ 0 ][ Vector2Int.right ]
                };
                chain.gems.Add( gem );
                WalkMatches( gem, Vector2Int.right, gem.gemLayers[ 0 ][ Vector2Int.right ], visited, chain );

                if( chain.gems.Count > 1 ) {
                    chains.Add( chain );
                }

                chain = new MatchChain() {
                    gemType = gem.gemLayers[ 0 ][ Vector2Int.up ]
                };
                chain.gems.Add( gem );
                WalkMatches( gem, Vector2Int.up, gem.gemLayers[ 0 ][ Vector2Int.up ], visited, chain );

                if( chain.gems.Count > 1 ) {
                    chains.Add( chain );
                }

                chain = new MatchChain() {
                    gemType = gem.gemLayers[ 0 ][ Vector2Int.down ]
                };
                chain.gems.Add( gem );
                WalkMatches( gem, Vector2Int.down, gem.gemLayers[ 0 ][ Vector2Int.down ], visited, chain );

                if( chain.gems.Count > 1 ) {
                    chains.Add( chain );
                }
            }
        }

        return chains;

        // Debug.Break();
    }

    List<Gem> GrabContiguousRow ( int y, int direction ) {

        var gems = new List<Gem>();

        if( direction > 0 ) {
            var x = 0;
            while( x < width ) {
                var gem = TryGetGem( x, y );
                if( gem == null ) break;
                gems.Add( gem );
                x += 1;
            }
        } else {
            var x = width - 1;
            while( x >= 0 ) {
                var gem = TryGetGem( x, y );
                if( gem == null ) break;
                gems.Add( gem );
                x -= 1;
            }
        }

        return gems;
    }

    List<Gem> GrabContiguousColumn ( int x, int direction ) {

        var gems = new List<Gem>();

        if( direction > 0 ) {
            var y = 0;
            while( y < height ) {
                var gem = TryGetGem( x, y );
                if( gem == null ) break;
                gems.Add( gem );
                y += 1;
            }
        } else {
            var y = height - 1;
            while( y >= 0 ) {
                var gem = TryGetGem( x, y );
                if( gem == null ) break;
                gems.Add( gem );
                y -= 1;
            }
        }

        return gems;
    }

    bool InGridBoundsX ( int x ) => x >= 0 && x < width;
    bool InGridBoundsY ( int y ) => y >= 0 && y < height;
    bool InGridBounds ( Vector2Int position ) => position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    bool InGridBounds ( int x, int y ) => x >= 0 && x < width && y >= 0 && y < height;

    void PopulateBoardRandomly () {
        for( int y = 0; y < height; y++ ) {
            for( int x = 0; x < width; x++ ) {
                var gem = GenerateRandomGem( defaultGenerationTable, defaultLayerGenerationTable );
                gem.SetPositionImmediate( x, y );
                gems.Add( gem );
            }
        }
    }

    Gem GenerateRandomGem ( GemGenerationTable gemGenerationTable, GemLayerGenerationTable gemLayerGenerationTable ) {
        var gem = Instantiate( gemPrefab );
        var sampler = new AnimationCurveSampler( gemGenerationTable.layerCountProbability );
        var layerCount = Mathf.CeilToInt( sampler.Sample() * gemGenerationTable.maxLayers );
        if( layerCount == 0 ) layerCount = 1;

        // layerCount = 4;

        for( int i = 0; i < layerCount; i++ ) {
            gem.gemLayers.Add( RandomGemLayer( gemLayerGenerationTable ) );
        }

        gem.InitializeLayers();

        return gem;
    }

    GemLayer RandomGemLayer ( GemLayerGenerationTable gemLayerGenerationTable ) {
        var colorCountSampler = new AnimationCurveSampler( gemLayerGenerationTable.colorCountProbability );
        var colorCount = Mathf.CeilToInt( colorCountSampler.Sample() * gemLayerGenerationTable.maxColors );
        if( colorCount == 0 ) colorCount = 1;

        // colorCount = 4;

        var colorSelector = new AnimationCurveSampler( gemLayerGenerationTable.absoluteColorSelectionProbability );
        var validColors = new List<int>();
        for( int i = 0; i < colorCount; i++ ) {
            var color = Mathf.FloorToInt( colorSelector.Sample() * gemTypes.gemTypes.Count );
            if( !validColors.Contains( color ) ) {
                validColors.Add( color );
            }
        }

        GemLayer gemLayer = default;
        if( validColors.Count > 1 ) {
            var finalColorSampler = new AnimationCurveSampler( gemLayerGenerationTable.relativeColorSelectionProbability );
            for( int i = 0; i < 4; i++ ) {
                var gemType = Mathf.FloorToInt( finalColorSampler.Sample() * validColors.Count );
                gemLayer[ i ] = gemType;
            }
        } else {
            for( int i = 0; i < 4; i++ ) {
                gemLayer[ i ] = validColors[ 0 ];
            }
        }

        return gemLayer;
    }
}
