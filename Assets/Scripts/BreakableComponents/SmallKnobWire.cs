using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallKnobWire : BreakableComponent
{
    [SerializeField]
    private ColorControl _LED = null;

    [SerializeField]
    private Wire _wire = null;

    [SerializeField]
    private WireInput _input = null;

    [SerializeField]
    private PressureGauge _gauge = null;
    

    private void Start()
    {
        _LED.SetEmissiveColor(ColorPalette.OK);
        _LED.GetComponent<TheToggler>().Toggle(true);
        _input.Connect(_wire);
        _gauge.SetState(UnityEngine.Random.Range(0.0f, 0.4f));
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
                _LED.SetEmissiveColor(ColorPalette.WARNING);
            }
        }
    }

    public override void SetState(BreakableState state)
    {
        base.SetState(state);

        if (state == BreakableState.BROKEN)
        {
            _input.Disconnect();
            _gauge.SetState(1.0f);
            _LED.SetEmissiveColor(ColorPalette.BAD);
        }
        else if(state == BreakableState.FUNCTIONAL)
        {
            _LED.SetEmissiveColor(ColorPalette.OK);
            _gauge.SetState(UnityEngine.Random.Range(0.0f, 0.4f));
        }
    }

    public void WireConnected()
    {
        SetState(BreakableState.FUNCTIONAL);
    }

    public void WireDisconnected()
    {
        SetState(BreakableState.BROKEN);
    }
}
