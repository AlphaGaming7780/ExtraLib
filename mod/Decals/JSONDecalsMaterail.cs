using System.Collections.Generic;
using Game.Prefabs;
using UnityEngine;

namespace ExtraDetailingTools;

public class JSONDecalsMaterail
{
    public Dictionary<string, float> Float = [];
    public Dictionary<string, Vector4> Vector = [];
    public List<PrefabIdentifierInfo> prefabIdentifierInfos = [];
}