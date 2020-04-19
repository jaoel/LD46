using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WireLEDBreakable : BreakableComponent {
    public Wire[] wires = new Wire[0];
    public WireInput[] wireInputs = new WireInput[0];
    public ColorControl[] wireLEDs = new ColorControl[0];
    public ColorControl[] inputLEDs = new ColorControl[0];

    private List<(Wire wire, WireInput input)> wirePairs = new List<(Wire, WireInput)>();
    private int numConnected = 0;

    private void Start() {
        Randomize();
        wirePairs.ForEach(x =>
        {
            x.input.Connect(x.wire);
        });
        SetState(BreakableState.FUNCTIONAL);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetState(BreakableState.BROKEN);
        }
    }

    public override void SetState(BreakableState state) {
        base.SetState(state);

        if (state == BreakableState.BROKEN) {
            Randomize();
        } else if (state == BreakableState.FUNCTIONAL) {
            SetFunctional();
        }
    }

    public void OnConnectWire(int index) {
        numConnected++;
        if (numConnected == wireInputs.Length) {
            CheckState();
        }
    }

    public void OnDisconnectWire(int index) {
        if (numConnected == wireInputs.Length) {
            CheckState();
        }
        numConnected--;
    }

    private void CheckState() {
        bool allCorrect = true;
        foreach ((Wire wire, WireInput input) in wirePairs) {
            if (input.ConnectedWire != wire) {
                allCorrect = false;
            }
        }

        if (allCorrect && State != BreakableState.FUNCTIONAL) {
            SetState(BreakableState.FUNCTIONAL);
        } else if (!allCorrect && State == BreakableState.FUNCTIONAL) {
            SetState(BreakableState.BROKEN);
        }
    }

    private void SetFunctional() {
        foreach (ColorControl colorControl in wireLEDs) {
            colorControl.SetColor(ColorPalette.GetLEDEmissiveColor(ColorPalette.OK));
        }
        foreach (ColorControl colorControl in inputLEDs) {
            colorControl.SetColor(ColorPalette.GetLEDEmissiveColor(ColorPalette.OK));
        }
    }

    private void Randomize() {
        Color[] colors = new Color[] {
            Color.yellow,
            Color.blue,
            Color.white,
        };

        List<int> wireIndices = Enumerable.Range(0, wires.Length).ToList().Shuffle();
        List<int> inputIndices = Enumerable.Range(0, wireInputs.Length).ToList().Shuffle();

        wirePairs = new List<(Wire wire, WireInput input)>();
        for(int i = 0; i < wireIndices.Count; i++) {
            wirePairs.Add((wires[wireIndices[i]], wireInputs[inputIndices[i]]));
            wireLEDs[wireIndices[i]].SetColor(ColorPalette.GetLEDEmissiveColor(colors[i]));
            wireLEDs[wireIndices[i]].GetComponent<TheToggler>().Toggle(true);
            inputLEDs[inputIndices[i]].SetColor(ColorPalette.GetLEDEmissiveColor(colors[i]));
            inputLEDs[inputIndices[i]].GetComponent<TheToggler>().Toggle(true);
        }
    }
}
