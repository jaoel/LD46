using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelSize
{
    LARGE,
    MEDIUM,
    SMALL
}

public class PanelConfiguration : MonoBehaviour
{
    [SerializeField]
    GameObject _upperPanelContainer;

    [SerializeField]
    GameObject _lowerPanelContainer;

    [SerializeField]
    List<GameObject> _largePanels = new List<GameObject>();

    [SerializeField]
    List<GameObject> _mediumPanels = new List<GameObject>();

    [SerializeField]
    List<GameObject> _smallPanels = new List<GameObject>();

    public List<GameObject> LargePanels => _largePanels;
    public List<GameObject> MediumPanels => _mediumPanels;
    public List<GameObject> SmallPanels => _smallPanels;

    public GameObject UpperPanelContainer => _upperPanelContainer;
    public GameObject LowerPanelContainer => _lowerPanelContainer;
}
