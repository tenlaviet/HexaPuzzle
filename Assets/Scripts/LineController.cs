using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class LineController : MonoBehaviour
{
    private LineRenderer _lr;
    [SerializeField] private HexGrid m_Grid;
    [SerializeField] private HexBoard m_Board;
    
    public Camera camera;

    public List<Vector2> Points;
    public List<HexCell> SelectedHexCells;
    private int _pointCount;
    private bool _firstTouchIsOnHex;

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        camera = Camera.main;
        Points = new List<Vector2>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        MouseInput();
        DrawLine();
    }
    
    public void DrawLine()
    {
        _lr.positionCount = Points.Count;
        if (_lr.positionCount != 0)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                _lr.SetPosition(i, Points[i]);
            } 
        }
    }

    private HexCell GetHitCell(RaycastHit2D hit)
    {
        if (hit)
        {
            if (hit.transform.gameObject.CompareTag("Hex"))
            {
                if (hit.transform.TryGetComponent(out HexCell cell))
                {
                    if (cell.hex != null)
                    {
                        return cell;
                    }
                }
            }
        }

        return null;
    }
    private void MouseInput()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        if (m_Board.wait)
        {
            return;
        }
            
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            HexCell foundCell = GetHitCell(hit);
            if (foundCell != null)
            {
                if (m_Board.IsRemoving)
                {
                    m_Board.RemoveHex(foundCell);
                    return;
                }
                _firstTouchIsOnHex = true;
                SelectedHexCells.Add(foundCell);
                Vector2 hexPos = foundCell.transform.position;
                Points.Add(hexPos);
            }
        }
        
        if (Input.GetMouseButton(0))
        {
            if (_firstTouchIsOnHex)
            {
                if (Points.Count < SelectedHexCells.Count + 1)
                {
                    Points.Add(new Vector2());
                }
                
                Points[Points.Count - 1] = mousePos;
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                HexCell foundCell = GetHitCell(hit);
                if (foundCell != null)
                {
                    if (!SelectedHexCells.Contains(foundCell))
                    {
                        if (foundCell.hex.state.number != SelectedHexCells[SelectedHexCells.Count - 1].hex.state.number && SelectedHexCells.Count < 2)
                        {
                            return;
                        }
                        if (m_Board.CanMerge(SelectedHexCells[SelectedHexCells.Count - 1], foundCell))
                        {
                            SelectedHexCells.Add(foundCell);
                            Vector2 hexPos = foundCell.transform.position;
                            Points[Points.Count - 1] = hexPos;
                        }
                    }
                    else
                    {
                        if (foundCell != SelectedHexCells.Last())
                        {
                            int removeFrom = SelectedHexCells.IndexOf(foundCell);
                            int removeTo = SelectedHexCells.Count - 1;
                            Debug.Log(removeFrom + "-->" + removeTo);
                            SelectedHexCells.RemoveRange(removeFrom,removeTo - removeFrom);
                            Points.RemoveRange(removeFrom, removeTo + 1 - removeFrom);
                        }
                    }
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            //merge hexes
            if (SelectedHexCells.Count > 1)
            {
                m_Board.MergeHexes(SelectedHexCells);
            }
            
            _firstTouchIsOnHex = false;
            SelectedHexCells.Clear();
            Points.Clear();
        }
    }

}
