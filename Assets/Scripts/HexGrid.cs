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
    public HexCell[,] cells { get; private set; }
    public Vector2[] spawningPoints { get; private set; }
    private void Awake()
    {
        _outerRadius = 0.5f;
        _innerRadius = _outerRadius * 0.866025404f;
        //innerRadius = (outerRadius * Mathf.Sqrt(3))/2f;
        
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        cells = new HexCell[Width,Height];
        spawningPoints = new Vector2[Width];
        for (int x = 0; x < Width; x++)
        {
            CreateSpawningPoint(x);
            for (int y = 0; y < Height; y++)
            {
                CreateCell(x, y);
            }
        }
        
    }
    private void CreateCell(int x, int y)
    {
         Vector3 position = new Vector3();
        
         position.x = x * (_outerRadius * 1.5f + offset);
         position.y = (y + x * 0.5f - x/2) * (_innerRadius * 2f + offset);
         HexCell cell = cells[x,y] = Instantiate<HexCell>(m_HexCellPrefab);
         HexCoordinate coordinate = new HexCoordinate(x,y);
         cell.SetCellCoordinate(coordinate);
         cell.transform.SetParent(transform, true);
         cell.transform.localPosition = position;
         cell.gameObject.name = "col: " + x + " " + "row: " + y;
    }
    private void CreateSpawningPoint(int x)
    {
         Vector3 position = new Vector3();
         position.x = x * (_outerRadius * 1.5f + offset);
         position.y = 12;
         spawningPoints[x] = position + transform.position;
    }

    public HexCell GetCell(HexCoordinate coordinate)
    {
        return GetCell(coordinate.column, coordinate.row);
    }
    public HexCell GetCell(int col, int row)
    {
        return cells[col,row];
    }
    public List<HexCell> GetAllEmptyCells()
    {
        List<HexCell> emptyCells = new List<HexCell>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (cells[x,y].hex == null)
                {
                    emptyCells.Add(cells[x,y]);
                }
            }
        }
        return emptyCells;
    }

    public List<HexCell> GetCellNeighbors(HexCell cell)
    {
        List<HexCell> neighborCells = new List<HexCell>();
        foreach (HexCoordinate coordinate in cell.neighborCoordinates)
        {
            if (coordinate.column >=0 && coordinate.column < Width && coordinate.row >=0 && coordinate.row < Height)
            {
                neighborCells.Add(cells[coordinate.column, coordinate.row]);
            }
        }
        return neighborCells;
    }
    public List<HexCell> GetSameNumberNeighborCells(HexCell cell)
    {
        List<HexCell> neighborCells = new List<HexCell>();
        foreach (HexCoordinate coordinate in cell.neighborCoordinates)
        {
            if (coordinate.column >=0 && coordinate.column < Width && coordinate.row >=0 && coordinate.row < Height)
            {
                if (cells[coordinate.column, coordinate.row].hex.state.number == cell.hex.state.number)
                {
                    neighborCells.Add(cells[coordinate.column, coordinate.row]);
                }
            }
        }
        return neighborCells;
    }
}
