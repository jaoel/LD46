using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumKeypadLED : BreakableComponent
{
    [SerializeField]
    private List<ColorControl> _LEDs = new List<ColorControl>();

    [SerializeField]
    private Numpad _numpad;

    private string _code;
    private int _currentIndex;

    private void Start()
    {
        _code = string.Empty;
        _currentIndex = 0;
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
            int codeLength = UnityEngine.Random.Range(3, 7);
            for(int i = 0; i < codeLength; i++)
            {
                int number = UnityEngine.Random.Range(0, 10);
                if (_code.Length > 0)
                {
                    while(number == (int)Char.GetNumericValue(_code[_code.Length - 1]))
                    {
                        number = UnityEngine.Random.Range(0, 10);
                    }
                }
                _code += number.ToString();
            }

            List<ColorControl> shuffled = _LEDs.Shuffle();
            for(int i = 0; i < shuffled.Count; i++)
            {
                if (i < (int)Char.GetNumericValue(_code[_currentIndex]))
                {
                    shuffled[i].GetComponent<TheToggler>().Toggle(true);
                    shuffled[i].SetEmissiveColor(ColorPalette.BAD);
                }
                else
                {
                    shuffled[i].GetComponent<TheToggler>().Toggle(false);
                }
            }
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            _code = string.Empty;
            _currentIndex = 0;

            _LEDs.ForEach(x =>
            {
                x.GetComponent<TheToggler>().Toggle(true);
                x.SetEmissiveColor(ColorPalette.OK);
            });
        }
    }

    public void OnKeypadInput(int val)
    {
        if (State != BreakableState.FUNCTIONAL)
        {
            int currentNumber = (int)Char.GetNumericValue(_code[_currentIndex]);

            if (val == currentNumber)
            {
                _currentIndex++;

                if (_currentIndex >= _code.Length)
                {
                    SetState(BreakableState.FUNCTIONAL);
                    _numpad.PlayConfirm();
                }
                else
                {
                    List<ColorControl> shuffled = _LEDs.Shuffle();
                    for (int i = 0; i < shuffled.Count; i++)
                    {
                        if (i < (int)Char.GetNumericValue(_code[_currentIndex]))
                        {
                            shuffled[i].GetComponent<TheToggler>().Toggle(true);
                            shuffled[i].SetEmissiveColor(ColorPalette.BAD);
                        }
                        else
                        {
                            shuffled[i].GetComponent<TheToggler>().Toggle(false);
                        }
                    }
                }
            }
            else
            {
                _numpad.PlayDeny();
            }
        }
    }
}
