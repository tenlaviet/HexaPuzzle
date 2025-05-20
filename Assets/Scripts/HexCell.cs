using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
    public struct HexData
    {
        public int value { get; private set; }
        public HexCoordinate coordinate { get; private set; }
        public HexCoordinate[] NeighborCoordinates { get; private set; }
        public void InitiateHexData(HexCoordinate coordinate, int value = 0)
        {
            this.value = value;
            this.coordinate = coordinate;
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

        public void SetValue(int value)
        {
            this.value = value;
        }
        public bool CanMerge(HexCell cell)
        {
            
            // value validation
            bool isValidValue = cell.data.value == value;
            //
            
            //neighbor hex check
            bool isNeighbor = false;

            for (int i = 0 ; i < NeighborCoordinates.Length; i++)
            {
                if (cell.data.coordinate.Equals(NeighborCoordinates[i]))
                {
                    isNeighbor = true;
                    break;
                }
            }
            return isNeighbor && isValidValue;
        }

    }
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
    public HexData data;
    [SerializeField] private SpriteRenderer m_BackgroundSR;
    [SerializeField] private Transform m_SpriteTransform;
    [SerializeField] private TextMeshProUGUI m_ValueDisplayText;
    public HexState state { get; private set; }
    
    
    private float _speed;
    private bool _isMoving;
    private Vector3 _destination;
    private float _percentBetweenPoints;

    
    private void Start()
    {
    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveToward(_destination);
        }
    }
    public void SetState(HexState state)
    {
        this.state = state;
        
        this.data.SetValue(state.number);
        m_BackgroundSR.color = state.backgroundColor;
        m_ValueDisplayText.color = state.textColor;
        m_ValueDisplayText.text = state.number.ToString();
    }
    private void MoveToward(Vector3 destination)
    {
        float distanceBetweenWaypoints = Vector3.Distance (transform.position, _destination);
        _percentBetweenPoints += Time.deltaTime * _speed/distanceBetweenWaypoints;
        _percentBetweenPoints = Mathf.Clamp01 (_percentBetweenPoints);
        float easedPercentBetweenPoints = Ease(_percentBetweenPoints);
        Vector3 newPos = Vector3.Lerp (transform.position, _destination, easedPercentBetweenPoints);
        m_SpriteTransform.position = newPos;
        if (_percentBetweenPoints >= 1)
        {
            _isMoving = false;
            Destroy(gameObject);
        }
    }
    public void SetDestination(Vector2 destination, float speed = 5)
    {
        _destination = destination;
        _isMoving = true;
        _speed = speed;
    }

    private float Ease(float percent)
    {
        return Mathf.Sin((percent * Mathf.PI) / 2); 
    }
}   
