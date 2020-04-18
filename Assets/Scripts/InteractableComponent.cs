using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableComponent : MonoBehaviour
{
    //[0, 1]
    protected float _state;
    public float State => _state;

    [SerializeField]
    UnityEventFloat _onStateChanged;
}
