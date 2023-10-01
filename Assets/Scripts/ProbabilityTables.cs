using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProbabilityTable {
    public GemGenerationTable gemGenerationTable;
    public GemLayerGenerationTable gemLayerGenerationTable;
}

[CreateAssetMenu]
public class ProbabilityTables : ScriptableObject {
    public List<ProbabilityTable> tables;
}
