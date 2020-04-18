using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverInteractable : InteractableComponent {

    public Collider leverCollider = null;
    public Transform leverTransform = null;
    public Vector3 axis = Vector3.right;
    public float maxRotation = 180f;

    private bool _isGrabbed = false;
    private int _grabbedDirection = 1;
    private Vector3 _grabbedMousePos = Vector3.zero;

    private bool _moving = false;
    private float _timePassed = 0.0f;
    private float _time = 0.0f;
    private float _targetState = 0.0f;
    public void MoveTo(float time, float state)
    {
        _moving = true;
        _timePassed = 0.0f;
        _time = time;
        _targetState = state;
    }

    private void Update() {
        if (_moving)
        {
            _timePassed += Time.deltaTime;

            if (_timePassed <= _time && !Mathf.Approximately(State, _targetState))
            {
                UpdateState(Mathf.SmoothStep(State, _targetState, _timePassed / _time));
            }
            else
            {
                _moving = false;
                _timePassed = 0.0f;
                UpdateState(_targetState);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (leverCollider.Raycast(ray, out RaycastHit hit, 100f))
                {
                    _grabbedMousePos = Input.mousePosition;
                    _isGrabbed = true;
                    _grabbedDirection = State < 0.5f ? 1 : -1;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isGrabbed = false;
            }

            if (_isGrabbed)
            {
                Vector3 delta = Input.mousePosition - _grabbedMousePos;

                float dist;
                if (axis.x != 0f)
                {
                    dist = -delta.y;
                }
                else
                {
                    dist = delta.x;
                }
                dist /= Screen.height;
                dist *= _grabbedDirection * 4f;
                dist = Mathf.Clamp01(dist);


                UpdateState(_grabbedDirection > 0 ? dist : 1f - dist);
            }
            else
            {
                if (State > 0.5f)
                {
                    UpdateState(State + 10f * Time.deltaTime);
                }
                else
                {
                    UpdateState(State - 10f * Time.deltaTime);
                }
            }
        }

      
        leverTransform.localRotation = Quaternion.Euler(axis * maxRotation * State);
    }
}
