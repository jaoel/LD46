using UnityEngine;

public class Numpad : MonoBehaviour {
    [SerializeField]
    private UnityEventInt _onButtonPressed = new UnityEventInt();


    [SerializeField]
    private AudioSource _pressedAudio = null;

    [SerializeField]
    private AudioSource _codeConfirmed = null;

    [SerializeField]
    private AudioSource _codeDenied = null;

    public void OnPressButton(int number) {
        if (_pressedAudio != null) {
            _pressedAudio.Play();
        }

        _onButtonPressed.Invoke(number);
    }

    public void PlayConfirm()
    {
        if (_codeConfirmed != null)
        {
            _codeConfirmed.Play();
        }
    }

    public void PlayDeny()
    {
        if (_codeDenied != null)
        {
            _codeDenied.Play();
        }
    }
}
