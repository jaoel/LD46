using System.Collections.Generic;
using UnityEngine;

public class TheToggler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _onObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> _offObjects = new List<GameObject>();

    public bool Toggled { get; private set; } = false;

    public void Toggle(bool toggled) {
        if (Toggled != toggled) {
            foreach(GameObject go in _onObjects) {
                go.SetActive(toggled);
            }
            foreach (GameObject go in _offObjects) {
                go.SetActive(!toggled);
            }
        }
        Toggled = toggled;
    }
}
