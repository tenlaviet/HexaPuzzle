using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public struct HexData
    {
        public int value { get; private set; }
        public HexCoordinate coordinate { get; private set; }
        public HexCoordinate[] NeighborCoordinates;
        public void SetHexData(HexCoordinate coordinate, int value = 0)
        {
            this.value = value;
            this.coordinate = coordinate;
            bool isEven = coordinate.Column % 2 == 0;
            if (isEven)
            {
                NeighborCoordinates = new HexCoordinate[6]
                {
                    new HexCoordinate(coordinate.Column,coordinate.row - 1), 
                    new HexCoordinate(coordinate.Column,coordinate.row + 1), 
                    new HexCoordinate(coordinate.Column - 1,coordinate.row), 
                    new HexCoordinate(coordinate.Column - 1,coordinate.row - 1), 
                    new HexCoordinate(coordinate.Column + 1,coordinate.row), 
                    new HexCoordinate(coordinate.Column + 1,coordinate.row - 1) 
                };
            }
            else
            {
                NeighborCoordinates = new HexCoordinate[6]
                {
                    new HexCoordinate(coordinate.Column,coordinate.row - 1), 
                    new HexCoordinate(coordinate.Column,coordinate.row + 1), 
                    new HexCoordinate(coordinate.Column - 1,coordinate.row), 
                    new HexCoordinate(coordinate.Column - 1,coordinate.row + 1), 
                    new HexCoordinate(coordinate.Column + 1,coordinate.row), 
                    new HexCoordinate(coordinate.Column + 1,coordinate.row + 1) 
                };
            }
        }
        public bool IsNeighBorTo(HexCell cell)
        {
            for (int i = 0 ; i < NeighborCoordinates.Length; i++)
            {
                if (cell.Data.coordinate.Equals(NeighborCoordinates[i]))
                {
                    return true;
                }
            }
            return false;
        }

    }
    public struct HexCoordinate
    {
        public int row;
        public int Column;
        public HexCoordinate(int column, int row)
        {
            this.Column = column;
            this.row = row;
        }
    }

public class HexCell : MonoBehaviour
{
    public HexData Data;
}
