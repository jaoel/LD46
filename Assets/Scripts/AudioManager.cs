using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour {
    public class MixerChannelVolume {
        private string _parameterName;
        private AudioMixer _audioMixer;

        public float Volume { get; private set; } = 1f;

        public MixerChannelVolume(string parameterName, AudioMixer audioMixer) {
            _parameterName = parameterName;
            _audioMixer = audioMixer;

            Volume = PlayerPrefs.GetFloat("MixerChannelVolume" + _parameterName, 1f);
            SetVolume(Volume);
        }

        public void SetVolume(float volume) {
            Volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("MixerChannelVolume" + _parameterName, Volume);

            float logVolume = Mathf.Clamp(Mathf.Log(Mathf.Lerp(0.001f, 1f, Volume)) * 20f, -80f, 0f);
            _audioMixer.SetFloat(_parameterName, logVolume);
        }
    }

    private static AudioManager _instance = null;
    public static AudioManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<AudioManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private AudioMixer _audioMixer = null;
    [SerializeField] private AudioSource _musicSource = null;

    [SerializeField] private AudioClip _mainMenuTheme = null;
    [SerializeField] private AudioClip _calmGameplay = null;
    [SerializeField] private AudioClip _angryGameplay = null;


    public MixerChannelVolume MasterVolume { get; private set; } = null;
    public MixerChannelVolume MusicVolume { get; private set; } = null;
    public MixerChannelVolume SfxVolume { get; private set; } = null;

    private Queue<IEnumerator> _queuedCoroutines;
    private bool _fadingMusic = false;

    private void Awake() {
        if (_instance != null) {
            DestroyImmediate(this);
        } else {
            _instance = this;
            DontDestroyOnLoad(this);

            MasterVolume = new MixerChannelVolume("MasterVolume", _audioMixer);
            MusicVolume = new MixerChannelVolume("MusicVolume", _audioMixer);
            SfxVolume = new MixerChannelVolume("SfxVolume", _audioMixer);

            _instance._queuedCoroutines = new Queue<IEnumerator>();
            _instance.StartCoroutine(_instance.ProcessCoroutines());
        }
    }

    private IEnumerator ProcessCoroutines()
    {
        while (true)
        {
            if (_queuedCoroutines != null)
            {
                while (_queuedCoroutines.Count > 0)
                {
                    while (_fadingMusic)
                    {
                        yield return new WaitForSeconds(0.01f);
                    }

                    yield return StartCoroutine(_queuedCoroutines.Dequeue());
                }

            }
            yield return null;
        }
    }

    public void PlayMusic(string key, bool loop = true, float fadeTime = 0.2f)
    {
        if (key == "MainMenu")
        {
            _queuedCoroutines.Enqueue(PlayMusicFade(_musicSource, _mainMenuTheme, loop, fadeTime));
        }
        else if (key == "CalmGameplay")
        {
            _queuedCoroutines.Enqueue(PlayMusicFade(_musicSource, _calmGameplay, loop, fadeTime));
        }
        else if (key == "AlertGameplay")
        {
            _queuedCoroutines.Enqueue(PlayMusicFade(_musicSource, _angryGameplay, loop, fadeTime));
        }
        else if (key == "GameOver")
        {

        }
        else if (key == "Win")
        {

        }
    }

    private IEnumerator PlayMusicFade(AudioSource source, AudioClip clip, bool loop, float fadeTime, float time = 0.0f, bool gameplayMusic = false)
    {
        _fadingMusic = true;
        float fadeHalfTime = fadeTime / 2.0f;

        IEnumerator fadeOut = null;
        if (source.isPlaying)
        {
            fadeOut = FadeOut(source, fadeHalfTime);
            StartCoroutine(fadeOut);
            yield return new WaitForSeconds(fadeHalfTime);
        }

        if (fadeOut != null)
        {
            while (fadeOut.MoveNext())
            {
                yield return new WaitForSeconds(0.01f);
            }
        }

        source.clip = clip;
        source.loop = loop;
        source.volume = 0.2f;
        source.time = time;
        source.Play();

        StartCoroutine(FadeIn(source, fadeHalfTime));
        yield break;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        float timePassed = 0.0f;
        while (audioSource.volume > 0 || timePassed <= fadeTime)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            timePassed += Time.deltaTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = Math.Min(0.2f, MusicVolume.Volume);

        audioSource.volume = 0;
        audioSource.Play();

        float timePassed = 0.0f;
        while (audioSource.volume < MusicVolume.Volume || timePassed <= fadeTime)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            timePassed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = MusicVolume.Volume;
        _fadingMusic = false;
    }
}
