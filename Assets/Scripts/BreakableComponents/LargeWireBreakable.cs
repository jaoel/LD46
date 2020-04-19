using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeWireBreakable : BreakableComponent
{
    [SerializeField]
    private List<TheToggler> _lightbulbs = new List<TheToggler>();

    [SerializeField]
    private List<WireInput> _inputs = new List<WireInput>();

    [SerializeField]
    private List<Wire> _wires = new List<Wire>();

    private void Start()
    {
        _lightbulbs.ForEach(x =>
        {
            x.Toggle(true);
        });

        for(int i = 0; i < _inputs.Count; i++)
        {
            _inputs[i].Connect(_wires[i]);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetState(BreakableState.BROKEN);
        }

        if (State != BreakableState.FUNCTIONAL)
        {
            bool allConnected = true;
            for(int i = 0; i < _inputs.Count; i++)
            {
                if (_inputs[i].Connected)
                {
                    if (!_lightbulbs[i].Toggled)
                    {
                        _lightbulbs[i].Toggle(true);
                    }
                }
                else
                {
                    if (_lightbulbs[i].Toggled)
                    {
                        _lightbulbs[i].Toggle(false);
                    }
                    allConnected = false;
                }
            }

            if (allConnected)
            {
                SetState(BreakableState.FUNCTIONAL);
            }
        }
    }

    public override void SetState(BreakableState state)
    {
        base.SetState(state);

        if (state == BreakableState.BROKEN)
        {
            _inputs.ForEach(x =>
            {
                x.Disconnect();
            });

            _lightbulbs.ForEach(x =>
            {
                x.Toggle(false);
            });
        }
        else if (state == BreakableState.FUNCTIONAL)
        {

        }
    }
}
