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
            x.SetColor(new Vector3(0.0f, 1.0f, 0.0f) * 3.0f);
        });

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
