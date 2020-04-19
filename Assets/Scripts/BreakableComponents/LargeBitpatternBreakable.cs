using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeBitpatternBreakable : BreakableComponent
{
    [SerializeField]
    private List<ColorControl> _LEDs = new List<ColorControl>();

    private string _code;
    private string _currentInput;

    private void Start()
    {
        _LEDs.ForEach(x =>
        {
            x.GetComponent<TheToggler>().Toggle(true);
            x.SetColor(ColorPalette.OK);
        });

        _code = string.Empty;
    }

    public override void SetState(BreakableState state)
    {
        base.SetState(state);

        if (state == BreakableState.BROKEN)
        {
            int number = UnityEngine.Random.Range(1, 16);
            _code = number.ToString();
            string bitpattern = Convert.ToString(number, 2).PadLeft(4, '0');

            for(int i = 0; i < 4; i++)
            {
                if ((int)Char.GetNumericValue(bitpattern[i]) == 1)
                {
                    _LEDs[i].SetColor(ColorPalette.BAD);
                }
                else
                {
                    _LEDs[i].GetComponent<TheToggler>().Toggle(false);
                }
            }
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            _LEDs.ForEach(x =>
            {
                x.GetComponent<TheToggler>().Toggle(true);
                x.SetColor(ColorPalette.OK);
            });

            _code = string.Empty;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetState(BreakableState.BROKEN);
        }
    }

    public void OnButtonPressed(int code)
    {
        if (!string.IsNullOrEmpty(_code))
        {
            bool succes = false;
            _currentInput += code.ToString();
            if (_currentInput.Length == _code.Length)
            {
                if (_currentInput.Equals(_code))
                {
                    succes = true;
                }

                if (succes)
                {
                    SetState(BreakableState.FUNCTIONAL);
                }
                else
                {

                }

                _currentInput = "";
            }
        }
    }
}
