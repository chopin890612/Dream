using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContainer : MonoBehaviour
{
    public GridData gridData;

    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(gridData.tileSize.x * gridData.gridSize.x, gridData.tileSize.y * gridData.gridSize.y);
    }
}
