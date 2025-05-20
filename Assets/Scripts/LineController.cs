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
    
    public Camera camera;

    public List<Vector2> Points;
    public List<HexCell> SelectedHexCells;
    private int _pointCount;
    private bool _firstTouchIsOnHex;

    private int _mergeValue;
    private int _highestMergeLevel;
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
    private void MouseInput()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                if (hit.transform.gameObject.CompareTag("Hex"))
                {
                    if (hit.transform.TryGetComponent(out HexCell cell))
                    {
                        _firstTouchIsOnHex = true;
                        SelectedHexCells.Add(cell);
                        Vector2 hexPos = hit.transform.position;
                        Points.Add(hexPos);

                    }

                }
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
                if (hit)
                {
                    if (hit.transform.gameObject.CompareTag("Hex"))
                    {
                        if (hit.transform.TryGetComponent(out HexCell cell))
                        {
                            if (!SelectedHexCells.Contains(cell))
                            {
                                if (cell.data.CanMerge(SelectedHexCells[SelectedHexCells.Count - 1]))
                                {
                                    SelectedHexCells.Add(cell);
                                    Vector2 hexPos = hit.transform.position;
                                    Points[Points.Count - 1] = hexPos;
                                }
                            }
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
                m_Grid.MergeCells(SelectedHexCells);
            }
            
            _firstTouchIsOnHex = false;
            SelectedHexCells.Clear();
            Points.Clear();
        }
    }

}
