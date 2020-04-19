using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableComponent : MonoBehaviour
{
    [SerializeField]
    private AudioSource _breakSound;

    [SerializeField]
    private AudioSource _deadSound;

    [SerializeField]
    private List<AudioClip> _breakClips;

    [SerializeField]
    private float _lifetime = 15.0f;
    private float _timeBroken = 0.0f;

    private float _sparksTimeNext = 0f;

    private BreakableState _state;

    public BreakableState State => _state;

    public virtual void SetState(BreakableState state)
    {
        if (_state != state && state == BreakableState.BROKEN) {
            GameManager.Instance.InstantiateSparks(transform.position, transform.rotation);
            _sparksTimeNext = Time.time + Random.Range(4f, 7f);
        }
        _state = state;

        if (_state == BreakableState.BROKEN)
        {
            _timeBroken = 0.0f;

            if (_breakSound != null)
            {
                _breakSound.clip = _breakClips[UnityEngine.Random.Range(0, _breakClips.Count)];
                _breakSound.Play();
            }
        }

        if (_state == BreakableState.DEAD)
        {
            _deadSound?.Play();
        }
    }

    protected virtual void Update()
    {
        if (_state == BreakableState.BROKEN)
        {
            _timeBroken += Time.deltaTime;

            if (_timeBroken >= _lifetime)
            {
                SetState(BreakableState.DEAD);
            }

            if (Time.time > _sparksTimeNext) {
                _sparksTimeNext = Time.time + Random.Range(4f, 7f);
                GameManager.Instance.InstantiateSparks(transform.position, transform.rotation);
                _breakSound.clip = _breakClips[UnityEngine.Random.Range(0, _breakClips.Count)];
                _breakSound.Play();
            }
        }
    }


}
