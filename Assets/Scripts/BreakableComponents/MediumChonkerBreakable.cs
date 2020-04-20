using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumChonkerBreakable : BreakableComponent
{
    [SerializeField]
    private List<TheToggler> _bulbs = new List<TheToggler>();

    private void Start()
    {
        _bulbs.ForEach(x =>
        {
            x.Toggle(true);
            x.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(0.0f, 360.0f));
        });
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (State != BreakableState.FUNCTIONAL)
        {
            bool allToggled = true;
            _bulbs.ForEach(x =>
            {
                if (!x.Toggled)
                {
                    allToggled = false;
                }
            });

            if (allToggled)
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
            _bulbs.ForEach(x =>
            {
                x.Toggle(false);
            });
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            _bulbs.ForEach(x =>
            {
                x.Toggle(true);
            });
        }
    }

    public void ButtonPressed(int button)
    {
        _bulbs[button].Toggle(true);
    }
}
