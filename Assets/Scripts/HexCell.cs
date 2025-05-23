using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct HexCoordinate
{
    public int row { get; private set; }
    public int column { get; private set; }

    public HexCoordinate(int column, int row)
    {
        this.column = column;
        this.row = row;
    }
}

public struct HexData
{
    public HexCoordinate coordinate { get; private set; }
    public HexState state { get; private set; }

    public HexData(HexCoordinate coordinate, HexState state)
    {
        this.coordinate = coordinate;
        this.state = state;
    }
}
public class HexCell : MonoBehaviour
{
    public Hex hex;
    public HexCoordinate coordinate { get; private set; }
    public HexCoordinate[] neighborCoordinates { get; private set; }
    private SpriteRenderer _removeIcon;


    private void Awake()
    {
        _removeIcon = GetComponentInChildren<SpriteRenderer>();
    }
    public void SetCellCoordinate(HexCoordinate coordinate)
    {
        this.coordinate = coordinate;
        
        bool isEven = coordinate.column % 2 == 0;
        if (isEven)
        {
            neighborCoordinates = new HexCoordinate[6]
            {
                new HexCoordinate(coordinate.column,coordinate.row - 1), 
                new HexCoordinate(coordinate.column,coordinate.row + 1), 
                new HexCoordinate(coordinate.column - 1,coordinate.row), 
                new HexCoordinate(coordinate.column - 1,coordinate.row - 1), 
                new HexCoordinate(coordinate.column + 1,coordinate.row), 
                new HexCoordinate(coordinate.column + 1,coordinate.row - 1) 
            };
        }
        else
        {
            neighborCoordinates = new HexCoordinate[6]
            {
                new HexCoordinate(coordinate.column,coordinate.row - 1), 
                new HexCoordinate(coordinate.column,coordinate.row + 1), 
                new HexCoordinate(coordinate.column - 1,coordinate.row), 
                new HexCoordinate(coordinate.column - 1,coordinate.row + 1), 
                new HexCoordinate(coordinate.column + 1,coordinate.row), 
                new HexCoordinate(coordinate.column + 1,coordinate.row + 1) 
            };
        }
    }

    public void ToggleRemoveIcon()
    {
        _removeIcon.enabled = !_removeIcon.enabled;
    }
}   
