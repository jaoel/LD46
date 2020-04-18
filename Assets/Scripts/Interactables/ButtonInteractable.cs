using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonState
{
    HOVER,
    PRESSING,
    RELEASED
}

public class ButtonInteractable : InteractableComponent
{
    private ButtonState _buttonState;

    [SerializeField]
    private float _pushSpeed;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        switch (_buttonState)
        {
            case ButtonState.HOVER:
                break;
            case ButtonState.PRESSING:
                UpdateState(State + _pushSpeed * Time.deltaTime);
                break;
            case ButtonState.RELEASED:
                UpdateState(State - _pushSpeed * Time.deltaTime);
                break;
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            _buttonState = ButtonState.PRESSING;
        }
        else
        {
            _buttonState = ButtonState.HOVER;
        }
    }

    private void OnMouseExit()
    {
        _buttonState = ButtonState.RELEASED;
    }
}
