using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelConfiguration : MonoBehaviour
{
    [SerializeField]
    List<Transform> _largePanels = new List<Transform>();

    [SerializeField]
    List<Transform> _mediumPanels = new List<Transform>();

    [SerializeField]
    List<Transform> _smallPanels = new List<Transform>();

    public List<Transform> LargePanels => _largePanels;
    public List<Transform> MediumPanels => _mediumPanels;
    public List<Transform> SmallPanels => _smallPanels;
}
