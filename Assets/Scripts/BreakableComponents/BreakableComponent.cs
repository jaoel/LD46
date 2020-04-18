using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableComponent : MonoBehaviour
{
    [SerializeField]
    private float _lifetime = 15.0f;
    private float _timeBroken = 0.0f;

    private BreakableState _state;

    public BreakableState State => _state;

    public virtual void SetState(BreakableState state)
    {
        _state = state;

        if (_state == BreakableState.BROKEN)
        {
            _timeBroken = 0.0f;
        }
    }

    protected virtual void Update()
    {
        if (_state == BreakableState.BROKEN)
        {
            _timeBroken += Time.deltaTime;

            if (_timeBroken >= _lifetime)
            {
                SetState(BreakableState.DEAD);
            }
        }
    }


}
