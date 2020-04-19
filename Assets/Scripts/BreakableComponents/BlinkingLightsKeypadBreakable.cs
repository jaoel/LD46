using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLightsKeypadBreakable : BreakableComponent
{
    [SerializeField]
    private Numpad _numpad;

    [SerializeField]
    private List<TheToggler> _lightBulbs = new List<TheToggler>();

    private string _currentCode = string.Empty;
    private string _inputCode = string.Empty;
    private int _currentCharacter = 0;
    private float _timePassed = 0.0f;
    private float _timePerCharacter = 1.0f;

    private void Start()
    {
        _lightBulbs.ForEach(x =>
        {
            x.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(0.0f, 360.0f));
        });

        Reset();
    }

    public override void SetState(BreakableState state)
    {
        base.SetState(state);

        if (state == BreakableState.BROKEN)
        {
            GenerateCode();
        }
        else if (state == BreakableState.FUNCTIONAL)
        {
            Reset();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (State != BreakableState.FUNCTIONAL)
        {
            _timePassed += Time.deltaTime;

            if (_timePassed >= _timePerCharacter)
            {
                _currentCharacter++;

                if (_currentCharacter == _currentCode.Length)
                {
                    ToggleLightBulbs(0);
                    _timePassed = -_timePerCharacter;
                    _currentCharacter = -1;
                }
                else
                {
                    ToggleLightBulbs((int)Char.GetNumericValue(_currentCode[_currentCharacter]));
                    _timePassed = 0.0f;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetState(BreakableState.BROKEN);
        }
    }

    private void GenerateCode()
    {
        _currentCode = string.Empty;
        _currentCharacter = 0;

        for (int i = 0; i < 4; i++)
        {
            int random = UnityEngine.Random.Range(1, 7);

            while (_currentCode.Length > 0 && random == (int)Char.GetNumericValue(_currentCode[_currentCode.Length - 1]))
            {
                random = UnityEngine.Random.Range(1, 7);
            }

            _currentCode += random.ToString();
        }

        ToggleLightBulbs((int)Char.GetNumericValue(_currentCode[_currentCharacter]));
    }

    private void Reset()
    {
        _currentCode = string.Empty;
        _inputCode = string.Empty;
        _currentCharacter = 0;
        _timePassed = 0.0f;
        ToggleLightBulbs(_lightBulbs.Count);
    }

    private void ToggleLightBulbs(int range)
    {
        for (int i = 0; i < _lightBulbs.Count; i++)
        {
            if (i < range)
            {
                _lightBulbs[i].Toggle(true);
            }
            else
            {
                _lightBulbs[i].Toggle(false);
            }
        }
    }

    public void OnButtonPressed(int code)
    {
        _inputCode += code.ToString();

        if (string.Compare(_inputCode, 0, _currentCode, 0, _inputCode.Length) != 0)
        {
            _inputCode = code.ToString();
        }

        bool succes = false;
        if (_inputCode.Length == _currentCode.Length)
        {
            if (_currentCode != string.Empty)
            {
                if (_currentCode.Equals(_inputCode))
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
        }
    }
}
