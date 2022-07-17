using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGridData", menuName = "Bang's Things/ScriptObjects/GridData")]
public class GridData : ScriptableObject
{
    public Vector2Int tileSize;
    public Vector2Int gridSize;
}
