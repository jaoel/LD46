using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableComponent : MonoBehaviour
{
    private float _state;

    [SerializeField]
    private UnityEventFloat _onStateChanged;
    public float State => _state;


    protected void Awake()
    {
        _state = 0.0f;
    }

    protected void UpdateState(float state)
    {
        _state = Mathf.Clamp(state, 0.0f, 1.0f);
        _onStateChanged.Invoke(_state);
    }
}
