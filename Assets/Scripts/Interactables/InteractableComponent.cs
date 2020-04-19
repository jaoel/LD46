using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableComponent : MonoBehaviour
{
    private float _state;

    [SerializeField]
    private UnityEventFloat _onStateChanged;

    [SerializeField]
    private UnityEvent _onPress;

    [SerializeField]
    private UnityEvent _onRelease;

    [SerializeField]
    private AudioClip _pressAudioClip;

    [SerializeField]
    private AudioClip _releaseAudioClip;

    [SerializeField]
    private AudioClip _upAudioClip;

    private AudioSource _audioSource;
    private static float _soundLastPlayedTime = -10f;

    public float State => _state;

    protected bool _mouseOver = false;
    protected bool _mouseOverPrev = false;

    int _layerMask = 0;
    protected void Awake()
    {
        _state = 0.0f;
        _layerMask = 1 << LayerMask.NameToLayer("Default");
        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        _mouseOverPrev = _mouseOver;
        _mouseOver = false;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f, _layerMask))
        {
            if (GameObject.ReferenceEquals(gameObject, hit.transform.gameObject))
            {
                _mouseOver = true;
            }
        }
    }

    private void PlayAudioCLip(AudioClip clip) {
        if (_audioSource != null && clip != null && Time.time > _soundLastPlayedTime + 0.1f && Time.time > 0.5f) {
            _soundLastPlayedTime = Time.time;
            _audioSource.PlayOneShot(clip);
        }
    }

    protected void UpdateState(float state)
    {
        float prevState = _state;
        _state = Mathf.Clamp01(state);
        

        if (prevState != _state) {
            if (_state == 1f) {
                _onPress.Invoke();
                PlayAudioCLip(_pressAudioClip);
            } else if (prevState == 1f && _state != 1f) {
                _onRelease.Invoke();
                PlayAudioCLip(_releaseAudioClip);
            } else if (_state == 0f) {
                PlayAudioCLip(_upAudioClip);
            }

            _onStateChanged.Invoke(_state);
        }
    }
}
