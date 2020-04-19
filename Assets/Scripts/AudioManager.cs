using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public class MixerChannelVolume {
        private string _parameterName;
        private AudioMixer _audioMixer;

        public float Volume { get; private set; } = 0f;

        public MixerChannelVolume(string parameterName, AudioMixer audioMixer) {
            _parameterName = parameterName;
            _audioMixer = audioMixer;

            Volume = PlayerPrefs.GetFloat("MixerChannelVolume" + _parameterName, 0f);
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

    public MixerChannelVolume MasterVolume { get; private set; } = null;
    public MixerChannelVolume MusicVolume { get; private set; } = null;
    public MixerChannelVolume SfxVolume { get; private set; } = null;

    private void Awake() {
        if (_instance != null) {
            DestroyImmediate(this);
        } else {
            _instance = this;
            DontDestroyOnLoad(this);

            MasterVolume = new MixerChannelVolume("MasterVolume", _audioMixer);
            MusicVolume = new MixerChannelVolume("MusicVolume", _audioMixer);
            SfxVolume = new MixerChannelVolume("SfxVolume", _audioMixer);
        }
    }

    private void UpdateVolumes() {

    }
}
