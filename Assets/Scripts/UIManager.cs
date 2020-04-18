using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private Image _blackScreen;

    [SerializeField]
    private TMPro.TMP_Text _text;

    private Queue<IEnumerator> _queuedCoroutines = new Queue<IEnumerator>();
    private bool _fading = false;

    private void Start()
    {
        StartCoroutine(ProcessCoroutines());  
    }

    public void TogglePause(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0.0f;
            Physics.queriesHitTriggers = false;
        }
        else
        {
            Time.timeScale = 1.0f;
            Physics.queriesHitTriggers = true;
        }

        _blackScreen.color = paused ? Color.black : Color.clear;
        _text.color = paused ? Color.white : Color.clear;
        _text.text = "Paused\nPress Escape to unpause\nPress Enter to return to Main Menu";
    }

    public void Fade(float time, bool fadeIn, string text = "", Action onComplete = null)
    {
         _queuedCoroutines.Enqueue(FadeScene(time, fadeIn, text, onComplete));
    }

    public void FadeText(float time, bool fadeIn, string text = "", Action onComplete = null)
    {
        _queuedCoroutines.Enqueue(FadeTextOnly(time, fadeIn, text, onComplete));
    }

    private IEnumerator FadeTextOnly(float time, bool fadeIn, string text, Action onComplete)
    {
        _fading = true;
        float timePassed = 0.0f;
        float sign = fadeIn ? 1.0f : -1.0f;

        if (!string.IsNullOrEmpty(text))
        {
            _text.text = text;
        }

        while (timePassed < time)
        {
            Color textColor = sign * Color.white * Time.unscaledDeltaTime / time;
            _text.color += textColor;

            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }
        _text.color = fadeIn ? Color.white : Color.clear;

        onComplete?.Invoke();

        _fading = false;
        yield break;
    }

    private IEnumerator FadeScene(float time, bool fadeIn, string text, Action onComplete)
    {
        _fading = true;
        float timePassed = 0.0f;
        float sign = fadeIn ? -1.0f : 1.0f;

        if (!string.IsNullOrEmpty(text))
        {
            _text.text = text;
        }

        while (timePassed < time)
        {
            Color blackScreenColor = sign * Color.black * Time.unscaledDeltaTime / time;
            _blackScreen.color += blackScreenColor;

            if (!string.IsNullOrEmpty(text))
            {
                Color textColor = sign * Color.white * Time.unscaledDeltaTime / time;
                _text.color += textColor;
            }

            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }

        _blackScreen.color = fadeIn ? Color.clear : Color.black;
        _text.color = fadeIn ? Color.clear : Color.white;

        onComplete?.Invoke();

        _fading = false;
        yield break;
    }

  

    private void Update()
    {
    }

    private IEnumerator ProcessCoroutines()
    {
        while (true)
        {
            if (_queuedCoroutines != null)
            {
                while (_queuedCoroutines.Count > 0)
                {
                    while (_fading)
                    {
                        yield return new WaitForSeconds(0.01f);
                    }

                    yield return StartCoroutine(_queuedCoroutines.Dequeue());
                }

            }
            yield return null;
        }
    }
}
