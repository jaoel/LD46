using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeFrankensteinBreakable : BreakableComponent
{
    [SerializeField]
    private List<TheToggler> _lightbulbs = new List<TheToggler>();

    [SerializeField]
    private List<LeverInteractable> _levers = new List<LeverInteractable>();

    private void Start()
    {
        _lightbulbs.ForEach(x =>
        {
            x.Toggle(true);
            x.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(0.0f, 360.0f));
        });

        _levers.ForEach(x =>
        {
            x.MoveTo(0f, 1.0f);
        });
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); 
    }

    public override void SetState(BreakableState state)
    {
        base.SetState(state);

        if (state == BreakableState.BROKEN)
        {
            bool broken = false;
            while(!broken)
            {
                for(int i = 0; i < 3; i++)
                {
                    if (UnityEngine.Random.Range(0.0f, 1.0f) <= 0.6f)
                    {
                        broken = true;
                        _levers[i].MoveTo(0.5f, 0.0f);
                        _lightbulbs[i].Toggle(false);
                    }
                }
            }
        }
    }

    public void OnLeverPressed()
    {
        bool allPressed = true;
        for(int i = 0; i < 3; i++)
        {
            if (_levers[i].State == 1.0f)
            {
                _lightbulbs[i].Toggle(true);
            }
            else
            {
                allPressed = false;
            }
        }

        if (allPressed)
        {
            SetState(BreakableState.FUNCTIONAL);
        }
    }
}
