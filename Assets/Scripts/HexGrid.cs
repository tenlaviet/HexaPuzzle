using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class HexGrid : MonoBehaviour
{
    public int Height;
    public int Width;
  
    public float offset;

    public HexCell m_HexCellPrefab;
    public TextMeshProUGUI cellLabelPrefab;
    public Canvas gridCanvas;
    
    public HexCell[] cells;
    public Bounds cellBounds;
    [HideInInspector]private float _innerRadius;
    [HideInInspector]private float _outerRadius;
    
    private void Awake()
    {
        cellBounds = m_HexCellPrefab.GetComponent<SpriteRenderer>().bounds;

        _outerRadius = 1f;
        _innerRadius = _outerRadius * 0.866025404f;
        //innerRadius = (outerRadius * Mathf.Sqrt(3))/2f;
        
        cells = new HexCell[Height * Width];

        for (int y = 0; y < Height ; y++) 
        {
            for (int x = 0, i = 0; x < Width; x++)
            {
                CreateCell(x, y, i++);
            }
        }
    }
    private void CreateCell(int x, int y, int i)
    {
        Vector3 position = new Vector3();

        position.x = x * (_outerRadius * 1.5f + offset);
        position.y = (y + x * 0.5f - x/2) * (_innerRadius * 2f + offset);
        
        HexCell cell = cells[i] = Instantiate<HexCell>(m_HexCellPrefab);
        HexCoordinate coordinate = new HexCoordinate(x,y);
        cell.Data.SetHexData(coordinate);
        cell.transform.SetParent(transform, true);
        cell.transform.localPosition = position;
        
        //tmp
        TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.y);
        label.text =  "col:" + x.ToString() + "row:" + y.ToString();
        //
    }

    public void MergeCells(List<HexCell> selectedCells)
    {
        foreach (HexCell cell in selectedCells)
        {
            cell.transform.position = selectedCells[selectedCells.Count - 1].transform.position;
        }
    }
}
