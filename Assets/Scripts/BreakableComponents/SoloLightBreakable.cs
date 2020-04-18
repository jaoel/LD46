using System.Collections;
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
        _toggler.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(0.0f, 360.0f));
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

    protected override void Update()
    {
        base.Update();
    }

    public void UpdateButtonState()
    {
        SetState(BreakableState.FUNCTIONAL);
    }
}
