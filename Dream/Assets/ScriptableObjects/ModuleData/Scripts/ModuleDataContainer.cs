using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "newModuleData", menuName = "Bang's Things/ScriptObjects/ModuleData/ModuleData")]
public class ModuleDataContainer : ScriptableObject
{
    public List<ModuleDataContainer> moduleDatas = new List<ModuleDataContainer>();
}

[Serializable]
public class ModuleData
{
    public int id;
    public string displayName;
    public int maxLevel;
}
