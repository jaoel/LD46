using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTickerBreakable : BreakableComponent
{
    [SerializeField]
    private PressureGauge _gauge;

    [SerializeField]
    private ColorControl _LED;

    private void Start()
    {
        _LED.GetComponent<TheToggler>().Toggle(true);
        _LED.SetColor(ColorPalette.OK);
        _gauge.SetState(0.0f);
    }

    protected override void Update()
    {
        base.Update();

        if (State == BreakableState.FUNCTIONAL)
        {
            _gauge.SetState(_gauge.State + 0.05f * Time.deltaTime);

            if (_gauge.State >= 0.9f)
            {
                SetState(BreakableState.BROKEN);
            }
            else if (_gauge.State >= 0.6f)
            {
                _LED.SetColor(ColorPalette.WARNING);
            }
        }
    }

    public override void SetState(BreakableState state)
    {
        base.SetState(state);

        if (state == BreakableState.BROKEN)
        {
            if (_gauge.State < 0.9f)
            {
                _gauge.SetState(1.0f);
            }

            _LED.SetColor(ColorPalette.BAD);
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            _gauge.SetState(0.0f);
            _LED.SetColor(ColorPalette.OK);
        }
    }

    public void OnButtonPress()
    {
        SetState(BreakableState.FUNCTIONAL);
    }
}
