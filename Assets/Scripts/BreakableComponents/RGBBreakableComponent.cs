using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBBreakableComponent : BreakableComponent
{
    [SerializeField]
    private Numpad _numpad;

    [SerializeField]
    private ColorControl _colorControl;

    [SerializeField]
    private TheToggler _toggler;

    private List<string> _errorCodes = new List<string>() { "100", "001", "101", "011", "110", "111" };
    private int _currentError = int.MaxValue;
    private string _currentCode;

    private void Start()
    {
        _toggler.Toggle(true);
        _colorControl.SetEmissiveColor(ColorPalette.OK);
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
            _currentError = UnityEngine.Random.Range(0, _errorCodes.Count);
            _currentCode = string.Empty;

            string errorCode = _errorCodes[_currentError];
            Vector3 color = new Vector3((int)Char.GetNumericValue(errorCode[0]),
                (int)Char.GetNumericValue(errorCode[1]), (int)Char.GetNumericValue(errorCode[2]));
            _colorControl.SetEmissiveColor(color);
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            _colorControl.SetEmissiveColor(ColorPalette.OK);
        }

    }

    public void OnButtonPressed(int code)
    {
        _currentCode += code.ToString();
        bool succes = false;
        if (_currentCode.Length == 3)
        {
            if (_currentError != int.MaxValue)
            {
                if(_currentCode.Equals(_errorCodes[_currentError]))
                {
                    succes = true;
                }
            }

            if (succes)
            {
                SetState(BreakableState.FUNCTIONAL);
                _numpad.PlayConfirm();
            }
            else
            {
                _numpad.PlayDeny();
            }

            _currentCode = "";
        }
    }
}
