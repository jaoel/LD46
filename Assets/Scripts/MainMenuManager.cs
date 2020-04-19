using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private KnobInteractable _masterKnob;
    [SerializeField]
    private KnobInteractable _sfxKnob;
    [SerializeField]
    private KnobInteractable _musicKnob;

    private void Start() {
        _masterKnob.MoveTo(0f, AudioManager.Instance.MasterVolume.Volume);
        _sfxKnob.MoveTo(0f, AudioManager.Instance.MusicVolume.Volume);
        _musicKnob.MoveTo(0f, AudioManager.Instance.SfxVolume.Volume);
    }

    public void OnStartPressed()
    {
        _uiManager.Fade(2.0f, false, "", () =>
        {
            SceneManager.LoadScene("DevScene");
        });
    }

    public void OnExitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void OnMasterVolumeChange(float volume) {
        AudioManager.Instance.MasterVolume.SetVolume(volume);
    }

    public void OnMusicVolumeChange(float volume) {
        AudioManager.Instance.MusicVolume.SetVolume(volume);
    }

    public void OnSfxVolumeChange(float volume) {
        AudioManager.Instance.SfxVolume.SetVolume(volume);
    }
}
