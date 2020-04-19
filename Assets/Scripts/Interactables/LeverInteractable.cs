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
            float moveT = Mathf.Clamp01((Time.time - _moveStartTime) / _moveDuration);
            if (moveT < 1f)
            {
                UpdateState(Mathf.SmoothStep(_moveStartState, _moveTargetState, moveT));
            }
            else
            {
                _moving = false;
                _moveStartTime = 0.0f;
                UpdateState(_moveTargetState);
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
