using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HexBoard : MonoBehaviour
{
    [SerializeField] private HexState[] m_hexVariations;
    [SerializeField] private Hex M_hexPrefab;
    private HexGrid _grid;
    public List<Hex> hexes;
    private List<HexData[]> BoardHistory;
    private float waitTime;
    public bool wait;
    public bool IsMerging {get; private set;} 
    public bool IsCollapsing {get; private set;} 
    public bool IsRemoving {get; private set;} 
    public static float MOVING_TIME
    {
        get
        {
            return 0.8f;
        }
        private set
        {
            
        }
    }
    private void Awake()
    {
        _grid = GetComponent<HexGrid>();

        waitTime = MOVING_TIME;
        hexes = new List<Hex>();
        BoardHistory = new List<HexData[]>();
    }
    private void Start()
    {
       FillBoard(_grid.GetAllEmptyCells());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Refill(_grid.GetAllEmptyCells());
            Restart();
        }if(Input.GetKeyDown(KeyCode.A))
        {
            Undo();
            //Hint();
            //TurnOnRemoveHex();
        }
        //if merge --> merge with time --> after merge is done --> collapse --> refill and collapse again
        if (IsMerging)
        {
            wait = true;
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                waitTime = MOVING_TIME;
                IsMerging = false;
                Collapse();
                FillBoard(_grid.GetAllEmptyCells());
            }
        }
        if (IsCollapsing)
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                wait = false;
                waitTime = MOVING_TIME;
                IsCollapsing = false;
            }
        }

    }
    private Hex CreateHex()
    {
        int randomState = Random.Range(0, 3);
        Hex hex = Instantiate(M_hexPrefab, _grid.transform);
        hex.SetState(m_hexVariations[randomState]);
        hexes.Add(hex);
        return hex;
    }
    public bool CanMerge(HexCell lastCell, HexCell mergeInto)
    {
        //neighbor hex check
        bool isNeighbor = false;
        
        for (int i = 0 ; i < mergeInto.neighborCoordinates.Length; i++)
        {
            if (lastCell.coordinate.Equals(mergeInto.neighborCoordinates[i]))
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
        mergedCell.hex.SetState(m_hexVariations[(int)Math.Log(currentBase * Math.Pow(2,power), 2) - 1]);
        selectedCells.Remove(mergedCell);

        
        foreach (HexCell cell in selectedCells)
        {
            cell.hex.MergeInto(mergedCell);
        }

        IsMerging = true;
        
        GameManager.Instance.UpdateScore(score);
    }
    private void FillBoard(List<HexCell> emptyCells)
    {
        hexes.RemoveAll(hex => hex == null);
        for (int i = 0; i < emptyCells.Count; i++)
        {
            CreateHex().Spawn(_grid.spawningPoints[emptyCells[i].coordinate.column]).MoveToCell(emptyCells[i]);
        }
        UpdateBoardHistory();
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

        IsCollapsing = true;
    }
    public void Restart()
    {
        foreach (Hex hex in hexes)
        {
            if (hex != null)
            {
                Destroy(hex.gameObject);
            }
        }
        hexes.Clear();
        FillBoard(_grid.GetAllEmptyCells());
    }
    public void ToggleRemoveHex()
    {
        foreach (HexCell hex in _grid.cells)
        {
            hex.ToggleRemoveIcon();
        }

        IsRemoving = !IsRemoving;
    }
    public void RemoveHex(HexCell cell)
    {
        hexes.Remove(cell.hex);
        cell.hex.Remove();
        Collapse();
        FillBoard(_grid.GetAllEmptyCells());
        ToggleRemoveHex();
    }
    public void Hint()
    {
        List<HexCell> hintCellsList = new List<HexCell>();
        foreach (HexCell cell in _grid.cells)
        {
            List<HexCell> neighborCells = _grid.GetSameNumberNeighborCells(cell);
            foreach (HexCell neighborCell in neighborCells)
            {
                if (!hintCellsList.Contains(neighborCell))
                {
                    hintCellsList.Add(neighborCell);
                    //run hint anim
                    //neighborCell.hex.transform.localScale *= 1.5f;
                }
            }
        }
    }
    public void Undo()
    {
        // if (GameManager.Instance.isPaused)
        // {
        //     return;
        // }
        if (BoardHistory.Count <= 1)
        {
            return;
        }
        for (int i = 0; i < _grid.cells.Length; i++)
        {
            HexData hexData = BoardHistory[1][i];
            HexCell cell = _grid.GetCell(hexData.coordinate);
            cell.hex.SetState(hexData.state); 
        }
        // foreach (Hex hex in hexes)
        // {
        //     int previousState = BoardHistory[1][position].state;
        //     hex.SetState(m_hexVariations[previousState]); 
        //     position++;
        // }
        BoardHistory.RemoveAt(0);
    }

    private void UpdateBoardHistory()
    {
        int position = 0;
        HexData[] boardData = new HexData[_grid.Width*_grid.Height];
        foreach (HexCell currentCell in _grid.cells)
        {
            HexData data = new HexData(currentCell.coordinate, currentCell.hex.state);
            boardData[position] = data;
            position++;
        }
        BoardHistory.Insert(0,boardData);
    }
}
