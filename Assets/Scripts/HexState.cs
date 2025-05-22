using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Hex State")]

public class HexState : ScriptableObject
{
    public int number;
    public Color32 backgroundColor;
    public Color32 textColor;

}
