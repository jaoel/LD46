using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallWireBreakable : BreakableComponent
{
    [SerializeField]
    private List<ColorControl> _LEDs = new List<ColorControl>();

    [SerializeField]
    private List<Wire> _wires = new List<Wire>();

    [SerializeField]
    private List<WireInput> _wireInputs = new List<WireInput>();

    private void Start()
    {
        _LEDs.ForEach(x =>
        {
            x.GetComponent<TheToggler>().Toggle(true);
            x.SetColor(ColorPalette.GetLEDEmissiveColor(ColorPalette.OK));
        });

        for(int i = 0; i < _wireInputs.Count; i++)
        {
            _wireInputs[i].Connect(_wires[i]);
        }
    }

    public override void SetState(BreakableState state)
    {
        base.SetState(state);

        if (state == BreakableState.BROKEN)
        {
            List<int> indices = new List<int>();
            while(indices.Count == 0)
            {
                for (int i = 0; i < _LEDs.Count; i++)
                {
                    if (indices.Contains(i))
                    {
                        continue;
                    }

                    if (UnityEngine.Random.Range(0.0f, 1.0f) <= 0.5f)
                    {
                        indices.Add(i);
                    }
                }
            }

            foreach(int i in indices)
            {
                _wireInputs[i].Disconnect();
            }
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            for(int i = 0; i < 2; i++)
            {
                _LEDs[i].SetColor(ColorPalette.GetLEDEmissiveColor(ColorPalette.OK));
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetState(BreakableState.BROKEN);
        }
    }

    public void UpdateButtonState()
    {
        if (State == BreakableState.FUNCTIONAL)
        {
            return;
        }

        bool allConnected = true;

        for(int i = 0; i < 2; i++)
        {
            if (!_wireInputs[i].Connected || _wireInputs[i].ConnectedWire != _wires[i])
            {
                allConnected = false;
            }
        }

        if (allConnected)
        {
            SetState(BreakableState.FUNCTIONAL);
        }
        else
        {
            SetState(BreakableState.BROKEN);
        }
    }

    public void OnConnect()
    {
        if (State != BreakableState.FUNCTIONAL)
        {
            for (int i = 0; i < 2; i++)
            {
                if (_wireInputs[i].Connected)
                {
                    _LEDs[i].SetColor(ColorPalette.GetLEDEmissiveColor(ColorPalette.WARNING));
                }
            }
        }
    }

    public void OnDisconnect()
    {
        if (State == BreakableState.FUNCTIONAL)
        {
            SetState(BreakableState.BROKEN);
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                if (!_wireInputs[i].Connected)
                {
                    _LEDs[i].SetColor(ColorPalette.GetLEDEmissiveColor(ColorPalette.BAD));
                }
                else
                {
                    _LEDs[i].SetColor(ColorPalette.GetLEDEmissiveColor(ColorPalette.WARNING));
                }
            }
        }
    }
}
