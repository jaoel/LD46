using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField]
    private Vector2 _sensitivity = new Vector2(5.0f, 5.0f);

    [SerializeField]
    private Vector2 _smoothing = new Vector2(2.0f, 2.0f);

    [SerializeField]
    private Vector2 _viewDirectionClamp = new Vector2(360.0f, 180.0f);

    private Vector2 _targetDirection;
    private Vector2 _position;
    private Vector2 _smoothedPosition;
    private Vector3 _initialLocalPos;
    private float _shakeDuration = 0.0f;

    private float _shakeStrength = 0.7f;
    private float _damping = 1.0f;

    private void Start()
    {
        _targetDirection = transform.localRotation.eulerAngles;
        //
        Cursor.visible = true;
        _initialLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false;

            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")) * _sensitivity * _smoothing;

            _smoothedPosition.x = Mathf.Lerp(_smoothedPosition.x, mouseDelta.x, 1f / _smoothing.x);
            _smoothedPosition.y = Mathf.Lerp(_smoothedPosition.y, mouseDelta.y, 1f / _smoothing.y);
            _position += _smoothedPosition;

            if (_viewDirectionClamp.x < 360)
            {
                _position.x = Mathf.Clamp(_position.x, -_viewDirectionClamp.x * 0.5f, _viewDirectionClamp.x * 0.5f);
            }

            if (_viewDirectionClamp.y < 360)
            {
                _position.y = Mathf.Clamp(_position.y, -_viewDirectionClamp.y * 0.5f, _viewDirectionClamp.y * 0.5f);
            }

            Quaternion targetOrientation = Quaternion.Euler(_targetDirection);
            transform.localRotation = Quaternion.AngleAxis(-_position.y, Vector3.right) * targetOrientation;

            Quaternion yRotation = Quaternion.AngleAxis(_position.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
        else
        {
            Cursor.visible = true;
        }

        if (_shakeDuration > 0)
        {
            transform.localPosition = _initialLocalPos + Random.insideUnitSphere * _shakeStrength;

            _shakeDuration -= Time.deltaTime * _damping;
        }
        else
        {
            _shakeDuration = 0f;
            transform.localPosition = _initialLocalPos;
        }
    }

    public void Shake(float duration, float strength = 0.2f)
    {
        _shakeDuration = duration;
        _shakeStrength = strength;
    }
}
