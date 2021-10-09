using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Stats Data", menuName = "Stats Menu")]
public class Stats : SerializedScriptableObject {
    public Dictionary<string, float> stats
                = new Dictionary<string, float>();
}
