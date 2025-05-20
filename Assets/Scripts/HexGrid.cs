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
    public TextMeshProUGUI cellLabelPrefab;
    public Canvas gridCanvas;
    [SerializeField] private HexState[] m_HexVariations;

    [Space]

    [Header("Grid size")]
    public int Height;
    public int Width;
    public float offset;
    [HideInInspector]private float _innerRadius;
    [HideInInspector]private float _outerRadius;
    [Space]

    public HexCell[] cells;
    public float speed;
    private void Awake()
    {
        _outerRadius = 0.5f;
        _innerRadius = _outerRadius * 0.866025404f;
        //innerRadius = (outerRadius * Mathf.Sqrt(3))/2f;
        
        cells = new HexCell[Height * Width];

        GenerateGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
    }

    private void GenerateGrid()
    {
        for (int y = 0, i = 0; y < Height ; y++) 
        {
            for (int x = 0; x < Width; x++)
            {
                CreateCellHolder(x, y, i++);
            }
        }
    }
    private void CreateCellHolder(int x, int y, int i)
    {
        Vector3 position = new Vector3();

        position.x = x * (_outerRadius * 1.5f + offset);
        position.y = (y + x * 0.5f - x/2) * (_innerRadius * 2f + offset);
        HexCell cell = cells[i] = Instantiate<HexCell>(m_HexCellPrefab);
        HexCoordinate coordinate = new HexCoordinate(x,y);
        cell.data.InitiateHexData(coordinate);
        cell.transform.SetParent(transform, true);
        cell.transform.localPosition = position;

        cell.gameObject.name = "col: " + x + " " + "row: " + y;
        
        SpawnCell(cell);
        // hex coordinate UI
        // TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cellLabelPrefab);
        // label.rectTransform.SetParent(gridCanvas.transform, false);
        // label.rectTransform.anchoredPosition = new Vector2(position.x, position.y);
        // label.text =  "col:" + x.ToString() + "row:" + y.ToString();
        //
    }

    private void Restart()
    {
        foreach (HexCell cell in cells)
        {
            if (cell != null)
            {
                Destroy(cell.gameObject);
            }
        }

        GenerateGrid();
    }
    private void SpawnCell(HexCell cell)
    {
        int randomIndex = Random.Range(0, 3);
        cell.SetState(m_HexVariations[randomIndex]);
    }
    public void MergeCells(List<HexCell> selectedCells)
    {
        int count = selectedCells.Count;
        int logValue = (int)(Math.Log(count, 2));
        double newValue = (int)(Math.Log(selectedCells[0].data.value, 2)) + logValue;
        foreach (HexCell cell in selectedCells)
        {
            if (cell == selectedCells.Last())
            {
                cell.SetState(m_HexVariations[(int)newValue - 1]);
                return;
            }
            cell.SetDestination(selectedCells.Last().transform.position, speed);
        }
    }
}
