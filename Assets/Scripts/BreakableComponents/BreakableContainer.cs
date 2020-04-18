using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableContainer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _largeBreakables = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _mediumBreakables = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _smallBreakables = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _mediumLandscapeBreakables = new List<GameObject>();

    public List<GameObject> Largebreakables => _largeBreakables;
    public List<GameObject> MediumBreakables => _mediumBreakables;
    public List<GameObject> SmallBreakables => _smallBreakables;
    public List<GameObject> MediumLandscapeBreakables => _mediumLandscapeBreakables;

    public GameObject GetRandomLargeBreakable()
    {
        return _largeBreakables[UnityEngine.Random.Range(0, _largeBreakables.Count)];
    }
    public GameObject GetRandomMediumBreakable()
    {
        return _mediumBreakables[UnityEngine.Random.Range(0, _mediumBreakables.Count)];
    }

    public GameObject GetRandomSmallBreakable()
    {
        return _smallBreakables[UnityEngine.Random.Range(0, _smallBreakables.Count)];
    }

    public GameObject GetRandomMediumLandscapeBreakable()
    {
        return _mediumLandscapeBreakables[UnityEngine.Random.Range(0, _mediumLandscapeBreakables.Count)];
    }
}
