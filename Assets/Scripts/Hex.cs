using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public HexState state { get; private set; }
    public HexCell cell { get; private set; }
    public bool locked { get; set; }

    [SerializeField] private TextMeshProUGUI m_ValueDisplayText;
    [SerializeField] private SpriteRenderer m_BackgroundSR;
}
