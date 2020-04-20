using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnobInteractable : InteractableComponent {

    public Collider knobCollider = null;
    public Transform knobTransform = null;
    public float maxRotation = 180f;
    public bool snapping = true;

    private bool _isGrabbed = false;
    private Vector3 _grabbedMousePos = Vector3.zero;
    private float _grabbedState = 0f;
    private float _targetState = 0f;
    private int _lastIntegerValue = 0;

    private bool _moving = false;
    private float _moveStartTime = 0.0f;
    private float _moveDuration = 0.0f;
    private float _moveStartState = 0.0f;
    private float _moveTargetState = 0.0f;
    public void MoveTo(float time, float state)
    {
        _moving = true;
        _moveStartTime = Time.time;
        _moveDuration = time;
        _moveTargetState = state;
        _moveStartState = State;
    }

    protected override void Update() {
        base.Update();
        if (_moving)
        {
            if (_moveDuration == 0f) {
                _moving = false;
                _moveStartTime = 0.0f;
                _targetState = _moveTargetState;
            } else {
                float moveT = Mathf.Clamp01((Time.time - _moveStartTime) / _moveDuration);
                if (moveT < 1f) {
                    _targetState = Mathf.SmoothStep(_moveStartState, _moveTargetState, moveT);
                } else {
                    _moving = false;
                    _moveStartTime = 0.0f;
                    _targetState = _moveTargetState;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (knobCollider.Raycast(ray, out RaycastHit hit, 100f))
                {
                    _grabbedMousePos = Input.mousePosition;
                    _isGrabbed = true;
                    _grabbedState = State;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isGrabbed = false;
            }

            if (_isGrabbed)
            {
                Vector3 delta = Input.mousePosition - _grabbedMousePos;

                float dist = delta.x;
                dist /= Screen.height;
                dist *= 6f;
                dist = Mathf.Clamp(dist, -1f, 1f);

                _targetState = _grabbedState + dist;
                int integerValue = Mathf.Clamp(Mathf.FloorToInt(_targetState * 9f), 0, 9);
                if (integerValue != _lastIntegerValue) {
                    PlayAudioCLip(_pressAudioClip, 0.01f);
                }
                _lastIntegerValue = integerValue;

            }
        }

        if (snapping) {
            int integerValue = Mathf.Clamp(Mathf.FloorToInt(_targetState * 9f), 0, 9);
            UpdateState(integerValue / 9f);
        } else {
            UpdateState(_targetState);
        }

        knobTransform.localRotation = Quaternion.RotateTowards(knobTransform.localRotation, Quaternion.Euler(Vector3.forward * maxRotation * State), 700f * Time.deltaTime);
    }
}
