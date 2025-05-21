using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HexGrid : MonoBehaviour
{
    [Header("References")]
    public HexCell m_HexCellPrefab;
    [Space]

    [Header("Grid size")]
    public int Height;
    public int Width;
    public float offset;
    [HideInInspector]private float _innerRadius;
    [HideInInspector]private float _outerRadius;
    public HexCell[] cells { get; private set; }
    private void Awake()
    {
        _outerRadius = 0.5f;
        _innerRadius = _outerRadius * 0.866025404f;
        //innerRadius = (outerRadius * Mathf.Sqrt(3))/2f;
        
        GenerateGrid();
    }

    private void Update()
    {
        
    }

    private void GenerateGrid()
    {
        cells = new HexCell[Height * Width];
    
        for (int x = 0, i = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
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
         cell.transform.SetParent(transform, true);
         cell.transform.localPosition = position;
         cell.coordinate = coordinate;
         cell.gameObject.name = "col: " + x + " " + "row: " + y;
    }

    public HexCell GetCell(HexCoordinate coordinate)
    {
        return GetCell(coordinate.column, coordinate.row);
    }
    public HexCell GetCell(int col, int row)
    {
        foreach (HexCell cell in cells)
        {
            if (cell.coordinate.column == col && cell.coordinate.row == row)
            {
                return cell;
            }
        }
        return null;
    }


    private void Restart()
    {
        // foreach (HexCell cell in cells)
        // {
        //     if (cell != null)
        //     {
        //         Destroy(cell.gameObject);
        //     }
        // }
        //
        // GenerateGrid();
    }
    // private void SpawnCell(HexCell cell)
    // {
    //     int randomIndex = Random.Range(0, 3);
    //     cell.SetState(m_HexVariations[randomIndex]);
    // }
    // public void MergeCells(List<HexCell> selectedCells)
    // {
    //     int score = 0;
    //     int currentBase = 0;
    //     int power = 0;// power of base 2
    //     int count = 0;
    //
    //     for (int i = 0 ; i < selectedCells.Count; i++)
    //     {
    //         if (currentBase == 0)
    //         {
    //             currentBase = selectedCells[i].data.value;
    //         }
    //
    //         if (selectedCells[i].data.value == currentBase)
    //         {
    //             count++;
    //         }
    //         else
    //         {
    //             currentBase = selectedCells[i].data.value;
    //             count = (int)(count/2) + 1;
    //         }
    //         score += currentBase;
    //     } 
    //     
    //     power = (int)(Math.Log(count, 2));
    //
    //
    //     HexCell mergedCell = selectedCells.Last();
    //     mergedCell.SetState(m_HexVariations[(int)Math.Log(currentBase * Math.Pow(2,power), 2) - 1]);
    //     selectedCells.Remove(mergedCell);
    //     // Debug.Log("currentBase:"+currentBase);
    //     // Debug.Log("count:"+count);
    //     // Debug.Log("power:"+power);
    //     // Debug.Log((int)Math.Log(currentBase * Math.Pow(2,power)));
    //
    //     
    //     
    //     foreach (HexCell cell in selectedCells)
    //     {
    //         cell.SetDestination(mergedCell.transform.position, speed);
    //     }
    //     PushDownCells(selectedCells);
    // }
    //
    // public void PushDownCells(List<HexCell> emptyCells)
    // {
    //     // HexCell[] deepestEmptyHexInEachCol = new HexCell[Width];
    //     // //Array.Fill(deepestEmptyHexInEachCol, 6);
    //     // foreach (HexCell cell in emptyCells)
    //     // {
    //     //     HexCoordinate currentHexCoordinate = cell.data.coordinate;
    //     //     if (deepestEmptyHexInEachCol[currentHexCoordinate.column] == null)
    //     //     {
    //     //         deepestEmptyHexInEachCol[currentHexCoordinate.column] = cell;
    //     //     }
    //     //     if (currentHexCoordinate.row < deepestEmptyHexInEachCol[currentHexCoordinate.column].data.coordinate.row)
    //     //     {
    //     //         deepestEmptyHexInEachCol[cell.data.coordinate.column] = cell;
    //     //     }
    //     // }
    //     // foreach (HexCell cell in cells)
    //     // {
    //     //     if (deepestEmptyHexInEachCol[cell.data.coordinate.column] != null)
    //     //     {
    //     //         if (cell.data.coordinate.row > deepestEmptyHexInEachCol[cell.data.coordinate.column].data.coordinate.row)
    //     //         {
    //     //             cell.SetDestination(deepestEmptyHexInEachCol[cell.data.coordinate.column].transform.position, speed);
    //     //         } 
    //     //     }
    //     // }
    // }
    // public void RefillCells(List<HexCell> selectedCells)
    // {
    //     
    // }
}
