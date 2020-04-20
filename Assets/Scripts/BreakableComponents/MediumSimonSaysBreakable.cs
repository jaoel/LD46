using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumSimonSaysBreakable : BreakableComponent
{
    [SerializeField]
    private List<ColorControl> _LEDs;

    [SerializeField]
    private List<ButtonInteractable> _buttons;

    private string _sequence = string.Empty;
    private int _currentSequenceIndex = 0;
    private void Start()
    {
        _LEDs.ForEach(x =>
        {
            x.GetComponent<TheToggler>().Toggle(true);
            x.SetEmissiveColor(ColorPalette.OK);
        });
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SetState(BreakableState state)
    {
        base.SetState(state);

        if (state == BreakableState.BROKEN)
        {
            _sequence = string.Empty;
            _currentSequenceIndex = 0;

            for (int i = 0; i < 5; i++)
            {
                int random = UnityEngine.Random.Range(0, 4);
                if (_sequence.Length > 0)
                {
                    while((int)Char.GetNumericValue(_sequence[i-1]) == random)
                    {
                        random = UnityEngine.Random.Range(0, 4);
                    }

                }
                _sequence += random.ToString();
            }

            for(int i = 0; i < _LEDs.Count; i++)
            {
                if (i == (int)Char.GetNumericValue(_sequence[_currentSequenceIndex]))
                {
                    _LEDs[i].GetComponent<TheToggler>().Toggle(true);
                }
                else
                {
                    _LEDs[i].GetComponent<TheToggler>().Toggle(false);
                }
                _LEDs[i].SetEmissiveColor(ColorPalette.BAD);
            }
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            _LEDs.ForEach(x =>
            {
                x.GetComponent<TheToggler>().Toggle(true);
                x.SetEmissiveColor(ColorPalette.OK);
            });

            _sequence = string.Empty;
            _currentSequenceIndex = 0;
        }
    }

    public void Button1()
    {
        HandleButtonPress(0);
    }

    public void Button2()
    {
        HandleButtonPress(1);
    }

    public void Button3()
    {
        HandleButtonPress(2);
    }

    public void Button4()
    {
        HandleButtonPress(3);
    }

    private void HandleButtonPress(int index)
    {
        int currentColumn = (int)Char.GetNumericValue(_sequence[_currentSequenceIndex]);
        if (index == currentColumn)
        {
            _LEDs[currentColumn].GetComponent<TheToggler>().Toggle(false);
            _currentSequenceIndex++;
            
            if (_currentSequenceIndex >= _sequence.Length)
            {
                SetState(BreakableState.FUNCTIONAL);
            }
            else
            {
                currentColumn = (int)Char.GetNumericValue(_sequence[_currentSequenceIndex]);
                _LEDs[currentColumn].GetComponent<TheToggler>().Toggle(true);
            }
        }
    }
}
