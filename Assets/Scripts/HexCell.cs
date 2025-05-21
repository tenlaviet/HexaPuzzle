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

public class HexCell : MonoBehaviour
{
    public Hex hex;
    public HexCoordinate coordinate;
    public HexCoordinate[] NeighborCoordinates { get; private set; }

    public bool Empty => hex == null;
    public bool Occupied => hex != null;


    private void Awake()
    {
        bool isEven = coordinate.column % 2 == 0;
        if (isEven)
        {
            NeighborCoordinates = new HexCoordinate[6]
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
            NeighborCoordinates = new HexCoordinate[6]
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
}   
