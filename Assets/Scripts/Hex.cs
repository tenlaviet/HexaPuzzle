using System;
using TMPro;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public HexState state { get; private set; }
    public HexCell cell { get; private set; }
    //public bool locked { get; set; }

    [SerializeField] private TextMeshProUGUI m_ValueDisplayText;
    [SerializeField] private SpriteRenderer m_BackgroundSR;
    
    [SerializeField] private Transform m_SpriteTransform;
    private float _speed;
    private bool _isMoving;
    private Vector3 _destination;
    private float _percentBetweenPoints;
    
    private void Update()
    {
        if (_isMoving)
        {
            //MoveToward(_destination);
        }
    }
    // public bool CanMerge(Hex hex)
    // {
    //         
    //     // value validation
    //     bool isValidValue = hex.state.value == this.state.value || (int)Math.Log(hex.state.value, 2) == (int)Math.Log(this.state.value, 2) - 1;
    //     //
    //         
    //     //neighbor hex check
    //     bool isNeighbor = false;
    //
    //     for (int i = 0 ; i < cell.NeighborCoordinates.Length; i++)
    //     {
    //         if (hex.cell.coordinate.Equals(this.cell.NeighborCoordinates[i]))
    //         {
    //             isNeighbor = true;
    //             break;
    //         }
    //     }
    //     return isNeighbor && isValidValue;
    // }
    // public void SetDestination(Vector2 destination, float speed = 5)
    // {
    //     _destination = destination;
    //     _isMoving = true;
    //     _speed = speed;
    // }
    //
    // private void MoveToward(Vector3 destination)
    // {
    //     float distanceBetweenWaypoints = Vector3.Distance (transform.position, _destination);
    //     _percentBetweenPoints += Time.deltaTime * _speed/distanceBetweenWaypoints;
    //     _percentBetweenPoints = Mathf.Clamp01 (_percentBetweenPoints);
    //     float easedPercentBetweenPoints = Ease(_percentBetweenPoints);
    //     Vector3 newPos = Vector3.Lerp (transform.position, _destination, easedPercentBetweenPoints);
    //     m_SpriteTransform.position = newPos;
    //     if (_percentBetweenPoints >= 1)
    //     {
    //         _isMoving = false;
    //         //Destroy(m_SpriteTransform.gameObject);
    //     }
    // }
    public void SetState(HexState state)
    {
        this.state = state;
        
        m_BackgroundSR.color = this.state.backgroundColor;
        m_ValueDisplayText.color = this.state.textColor;
        m_ValueDisplayText.text = this.state.value.ToString();
    }
    //
    // private float Ease(float percent)
    // {
    //     return Mathf.Sin((percent * Mathf.PI) / 2); 
    // }
    public void Spawn(HexCell cell)
    {
        if (this.cell != null) {
            this.cell.hex = null;
        }

        this.cell = cell;
        this.cell.hex = this;

        transform.position = cell.transform.position;
    }
}
