using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeKnobBreakable : BreakableComponent
{
    [SerializeField]
    private List<KnobInteractable> _knobs = new List<KnobInteractable>();

    [SerializeField]
    private List<ColorControl> _LEDs = new List<ColorControl>();

    private List<float> _targets = new List<float>();

    private void Start()
    {
        _LEDs.ForEach(x =>
        {
            x.GetComponent<TheToggler>().Toggle(true);
            x.SetEmissiveColor(ColorPalette.OK);
        });

        _knobs.ForEach(x =>
        {
            x.MoveTo(0.0f, UnityEngine.Random.Range(0.0f, 1.0f));
        });
    }

    protected override void Update()
    {
        base.Update();

        if (State != BreakableState.FUNCTIONAL)
        {
            bool allCorrect = true;
            for(int i = 0; i < 3; i++)
            {
                float diff = Mathf.Abs(_knobs[i].State - _targets[i]);
                if (diff <= 0.1f)
                {
                    _LEDs[i].SetEmissiveColor(ColorPalette.OK);
                }
                else
                {
                    allCorrect = false;
                    if (diff <= 0.2f)
                    {
                        _LEDs[i].SetEmissiveColor(ColorPalette.WARNING);
                    }
                    else
                    {
                        _LEDs[i].SetEmissiveColor(ColorPalette.BAD);
                    }
                }
            }

            if (allCorrect)
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
            _targets.Clear();
            for (int i = 0; i < 3; i++)
            {
                float newVal = Random.Range(0.0f, 1.0f);

                while(Mathf.Abs(_knobs[i].State - newVal) < 0.2f)
                {
                    newVal = Random.Range(0.0f, 1.0f);
                }

                _targets.Add((newVal));
                _LEDs[i].SetEmissiveColor(ColorPalette.BAD);
            }
        }
    }
}
