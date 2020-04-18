using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloLightBreakable : BreakableComponent
{
    [SerializeField]
    Light _light;

    [SerializeField]
    private Color _workingColor;

    [SerializeField]
    private Color _brokenColor;

    [SerializeField]
    ButtonInteractable _button;

    private void Awake()
    {
        _light.color = _workingColor;
    }

    private void Update()
    {
        if (_state == BreakableState.BROKEN)
        {
            _light.color = _brokenColor;

            if (_button.State >= 1.0f)
            {
                _state = BreakableState.FUNCTIONAL;
            }
        }
        else
        {
            _light.color = _workingColor;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _state = BreakableState.BROKEN;
        }
    }
}
