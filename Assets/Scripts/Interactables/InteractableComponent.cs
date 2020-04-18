using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableComponent : MonoBehaviour
{
    private float _state;

    [SerializeField]
    private UnityEventFloat _onStateChanged;

    [SerializeField]
    private UnityEvent _onPress;

    [SerializeField]
    private UnityEvent _onRelease;

    public float State => _state;

    protected void Awake()
    {
        _state = 0.0f;
    }

    protected void UpdateState(float state)
    {
        float prevState = _state;
        _state = Mathf.Clamp01(state);
        

        if (prevState != _state) {
            if (_state == 1f) {
                _onPress.Invoke();
            } else if (_state == 0f) {
                _onRelease.Invoke();
            }

            _onStateChanged.Invoke(_state);
        }
    }
}
