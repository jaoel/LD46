using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningLight : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private List<GameObject> _lights;

    [SerializeField]
    private float _rotationSpeed = 15.0f;

    private bool _active;
    private float _angle = 0.0f;

    private void Start()
    {
        _lights.ForEach(x =>
        {
            x.SetActive(false);
        });
        _active = false;
    }

    private void Update()
    {
        if (_gameManager.Dying && !_active)
        {
            _active = true;
            _lights.ForEach(x =>
            {
                x.SetActive(true);
            });
        }
        
        if (!_gameManager.Dying && _active)
        {
            _active = false;
            _lights.ForEach(x =>
            {
                x.SetActive(false);
            });
        }

        if (_active)
        {
            transform.rotation = transform.rotation 
                * Quaternion.Euler(new Vector3(0.0f, _rotationSpeed * Time.deltaTime, 0.0f));
        }
    }
}
