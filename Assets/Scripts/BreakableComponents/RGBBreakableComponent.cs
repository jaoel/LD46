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

    private List<string> _errorCodes = new List<string>() { "100", "010", "001", "101", "011" };
    private int _currentError = int.MaxValue;
    private string _currentCode;

    private float _glowStrength = 5.0f;
    private Vector3 _defaultColor = new Vector3(1.0f, 1.0f, 1.0f);

    private void Start()
    {
        _toggler.Toggle(true);
        _colorControl.SetColor(_defaultColor * _glowStrength);
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
            _colorControl.SetColor(new Vector3((int)Char.GetNumericValue(errorCode[0]), 
                (int)Char.GetNumericValue(errorCode[1]), (int)Char.GetNumericValue(errorCode[2])) * _glowStrength);
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            _colorControl.SetColor(_defaultColor * _glowStrength);
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
                    //Play success sound
                }
            }

            if (succes)
            {
                SetState(BreakableState.FUNCTIONAL);
            }
            else
            {

            }

            _currentCode = "";
        }
    }
}
