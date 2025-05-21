using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Hex State")]

public class HexState : ScriptableObject
{
    [FormerlySerializedAs("number")] public int value;
    public Color32 backgroundColor;
    public Color32 textColor;

}
