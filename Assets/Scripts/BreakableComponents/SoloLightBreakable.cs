﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloLightBreakable : BreakableComponent
{
    [SerializeField]
    ButtonInteractable _button;

    [SerializeField]
    TheToggler _toggler;

    private void Awake()
    {
        _toggler.Toggle(true);
    }

    public override void SetState(BreakableState state)
    {
        base.SetState(state);

        if (state == BreakableState.BROKEN)
        {
            _toggler.Toggle(false);
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            _toggler.Toggle(true);
        }
    }

    private void Update()
    {
    }

    public void UpdateButtonState()
    {
        SetState(BreakableState.FUNCTIONAL);
    }
}
