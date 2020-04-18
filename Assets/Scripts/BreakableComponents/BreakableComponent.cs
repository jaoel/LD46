using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableComponent : MonoBehaviour
{
    private BreakableState _state;

    public BreakableState State => _state;

    public virtual void SetState(BreakableState state)
    {
        _state = state;
    }
}
