using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableComponent : MonoBehaviour
{
    //[0, 1]
    private float _state;
    public float State => _state;

    [SerializeField]
    UnityEventFloat _onStateChanged;

    protected void Awake()
    {
        _state = 0.0f;
    }

    protected void UpdateState(float state)
    {
        _state = Mathf.Clamp(state, 0.0f, 1.0f);
    }
}
