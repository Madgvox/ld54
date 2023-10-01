using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GemTypes : ScriptableObject {
    public List<GemType> gemTypes;

    internal Color GetColor ( int gemType ) {
        var obj = gemTypes.Find( t => t.typeId == gemType );
        return obj.primaryColor;
    }
}

[Serializable]
public struct GemType {
    public int typeId;
    public Color primaryColor;
}
