using System;
using System.Collections;
using System.Collections.Generic;
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


    public float speed;

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

    }

    private void FillBoard()
    {
        for (int col = 0, i = 0; col < _grid.Width; col++)
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

    // public bool CanMerge(HexCell cell)
    // {
    //         
    //     // value validation
    //     bool isValidValue = cell.data.value == value || (int)Math.Log(cell.data.value, 2) == (int)Math.Log(this.value, 2) - 1;
    //     //
    //         
    //     //neighbor hex check
    //     bool isNeighbor = false;
    //
    //     for (int i = 0 ; i < NeighborCoordinates.Length; i++)
    //     {
    //         if (cell.data.coordinate.Equals(NeighborCoordinates[i]))
    //         {
    //             isNeighbor = true;
    //             break;
    //         }
    //     }
    //     return isNeighbor && isValidValue;
    // }
}
