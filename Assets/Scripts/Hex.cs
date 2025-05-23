using System;
using TMPro;
using UnityEngine;


public class Hex : MonoBehaviour
{
    
    public HexState state { get; private set; }
    public HexCell cell { get; private set; }

    [SerializeField] private TextMeshProUGUI m_ValueDisplayText;
    [SerializeField] private SpriteRenderer m_BackgroundSR;

    private Vector2 _oldPos;
    private Vector2 _newPos;
    
    private bool _isMoving;
    private float _percentBetweenPoints;
    private bool _isMerging;

    private void Awake()
    {

    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveTo(_oldPos, _newPos);
        }
    }

    private void MoveTo(Vector2 start, Vector2 finish)
    {
        float distanceBetweenWaypoints = Vector3.Distance (start, finish);
        float speed = distanceBetweenWaypoints / HexBoard.MOVING_TIME;
        _percentBetweenPoints += Time.deltaTime * speed/distanceBetweenWaypoints;
        _percentBetweenPoints = Mathf.Clamp01 (_percentBetweenPoints);
        float easedPercentBetweenPoints = Ease(_percentBetweenPoints);
        transform.position = Vector3.Lerp (start, finish, easedPercentBetweenPoints);
        if (_percentBetweenPoints >= 1)
        {
            _percentBetweenPoints = 0;
            _isMoving = false;
            if (_isMerging)
            {
                _isMerging = false;
                Destroy(gameObject);
            }
        }
    }
    public void MergeInto(HexCell mergeInto)
    {
        _isMerging = true;
        if (this.cell != null) {
            this.cell.hex = null;
        }
        this.cell.hex = null;
        _oldPos = transform.position;
        _newPos = mergeInto.transform.position;
        _isMoving = true;
    }
    public void MoveToCell(HexCell cell)
    {
        if (this.cell != null) {
            this.cell.hex = null;
        }
        
        this.cell = cell;
        this.cell.hex = this;
        _oldPos = transform.position;
        _newPos = cell.transform.position;
        _isMoving = true;

    }
    public void Remove()
    {
        if (this.cell != null) {
            this.cell.hex = null;
        }

        Destroy(this.gameObject);
    }
    public void SetState(HexState state)
    {
        this.state = state;
        m_BackgroundSR.color = this.state.backgroundColor;
        m_ValueDisplayText.color = this.state.textColor;
        m_ValueDisplayText.text = this.state.number.ToString();
    }
    public Hex Spawn(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    private float Ease(float percent)
    {
        return Mathf.Sin((percent * Mathf.PI) / 2); 
    }
}
