using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeKnobGauge : BreakableComponent
{
    [SerializeField]
    private List<KnobInteractable> _knobs = new List<KnobInteractable>();

    [SerializeField]
    private List<PressureGauge> _gauges = new List<PressureGauge>();

    [SerializeField]
    private LeverInteractable _lever = null;

    [SerializeField]
    private TheToggler _lightbulb = null;

    private void Start()
    {
        _lightbulb.Toggle(true);
        _lever.MoveTo(0.0f, 1.0f);

        for(int i = 0; i < _gauges.Count; i++)
        {
            _gauges[i].SetState(UnityEngine.Random.Range(0.0f, 0.4f));
            _knobs[i].MoveTo(1.0f, UnityEngine.Random.Range(0.4f, 0.6f));
        }
    }

    protected override void Update()
    {
        base.Update();

        if (State == BreakableState.FUNCTIONAL)
        {
            for (int i = 0; i < _knobs.Count; i++)
            {
                _gauges[i].SetState(_gauges[i].State + Random.Range(0.0f, 0.05f) * Time.deltaTime);

                if (_gauges[i].State >= 0.9f)
                {
                    SetState(BreakableState.BROKEN);
                    break;
                }
            }
        }
        else
        {
            bool allGaugesGood = true;
            for (int i = 0; i < _knobs.Count; i++)
            {
                if (_gauges[i].State > 0.1f)
                {
                    allGaugesGood = false;
                }
            }

            if (allGaugesGood && Mathf.Approximately(_lever.State, 1.0f))
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
            _knobs.ForEach(x =>
            {
                x.MoveTo(1.0f, 1.0f);
            });

            _gauges.ForEach(x =>
            {
                x.SetState(1.0f);
            });

            _lightbulb.Toggle(false);
            _lever.MoveTo(1.0f, 0.0f);
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            _lightbulb.Toggle(true);
            for (int i = 0; i < _gauges.Count; i++)
            {
                _gauges[i].SetState(UnityEngine.Random.Range(0.0f, 0.4f));
                _knobs[i].MoveTo(1.0f, UnityEngine.Random.Range(0.4f, 0.6f));
            }
        }
    }

    public void MovingGauge(int id)
    {
        _gauges[id].SetState(_knobs[id].State);
    }
}
