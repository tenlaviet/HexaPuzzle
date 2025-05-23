using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HexBoard : MonoBehaviour
{
    [SerializeField] private HexState[] m_HexVariations;
    [SerializeField] private Hex M_HexPrefab;
    private HexGrid _grid;
    public List<Hex> hexes;
    private void Awake()
    {
        _grid = GetComponent<HexGrid>();
        
    }

    private void Start()
    {
       FillBoard();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Refill(_grid.GetAllEmptyCells());
            //Restart();
        }
    }

    private void FillBoard()
    {
        for (int col = 0; col < _grid.Width; col++)
        {
            for (int row = 0; row < _grid.Height; row++)
            {
                CreateHex().Spawn(_grid.GetCell(col, row));
            }
        }
    }

    private Hex CreateHex()
    {
        int randomState = Random.Range(0, 3);
        Hex hex = Instantiate(M_HexPrefab, _grid.transform);
        hex.SetState(m_HexVariations[randomState]);
        hexes.Add(hex);
        return hex;
    }

    public bool CanMerge(HexCell lastCell, HexCell mergeInto)
    {
        //neighbor hex check
        bool isNeighbor = false;
        
        for (int i = 0 ; i < mergeInto.NeighborCoordinates.Length; i++)
        {
            if (lastCell.coordinate.Equals(mergeInto.NeighborCoordinates[i]))
            {
                isNeighbor = true;
                break;
            }
        } 
        // value validation
        bool isValidValue = lastCell.hex.state.number == mergeInto.hex.state.number || (int)Math.Log(lastCell.hex.state.number, 2) == (int)Math.Log(mergeInto.hex.state.number, 2) - 1;
        //
            
        return isNeighbor && isValidValue;
    }
    
    public void MergeHexes(List<HexCell> selectedCells)
    {
        int score = 0;
        int currentBase = 0;
        int power = 0;// power of base 2
        int count = 0;
        
        for (int i = 0 ; i < selectedCells.Count; i++)
        {
            if (currentBase == 0)
            {
                currentBase = selectedCells[i].hex.state.number;
            }
        
            if (selectedCells[i].hex.state.number == currentBase)
            {
                count++;
            }
            else
            {
                currentBase = selectedCells[i].hex.state.number;
                count = (int)(count/2) + 1;
            }
            score += currentBase;
        } 
        
        power = (int)(Math.Log(count, 2));
        
        
        HexCell mergedCell = selectedCells.Last();
        mergedCell.hex.SetState(m_HexVariations[(int)Math.Log(currentBase * Math.Pow(2,power), 2) - 1]);
        selectedCells.Remove(mergedCell);
        // Debug.Log("currentBase:"+currentBase);
        // Debug.Log("count:"+count);
        // Debug.Log("power:"+power);
        // Debug.Log((int)Math.Log(currentBase * Math.Pow(2,power)));
        
        foreach (HexCell cell in selectedCells)
        {
            cell.hex.MergeInto(mergedCell);
        }
        Collapse();
    }

    private void Collapse()
    {
        for (int x = 0; x < _grid.Width; x++)
        {
            int emptyCellsCount = 0;
            for (int y = 0; y < _grid.Height; y++)
            {
                if (_grid.cells[x,y].hex == null)
                {
                    emptyCellsCount++;
                }
                else
                {
                    if (emptyCellsCount <= 0)
                    {
                        continue;
                    }
                    _grid.cells[x,y].hex.MoveToCell(_grid.cells[x,y - emptyCellsCount]);
                }
            }
        }
    }

    private void Refill(List<HexCell> emptyCells)
    {
        for (int i = 0; i < emptyCells.Count; i++)
        {
            CreateHex().Spawn(emptyCells[i]);
        }
    }

    public void Restart()
    {
        // foreach (HexCell cell in _grid.cells)
        // {
        //     if (cell.hex != null)
        //     {
        //         Destroy(cell.hex.gameObject);
        //     }
        // } 
        foreach (Hex hex in hexes)
        {
            if (hex != null)
            {
                Destroy(hex.gameObject);
            }
        }
        hexes.Clear();
        FillBoard();
    }
}
