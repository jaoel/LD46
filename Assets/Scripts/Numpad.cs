using UnityEngine;

public class Numpad : MonoBehaviour {
    [SerializeField]
    private UnityEventInt _onButtonPressed = new UnityEventInt();


    [SerializeField]
    private AudioSource _pressedAudio = null;

    public void OnPressButton(int number) {
        if (_pressedAudio != null) {
            _pressedAudio.Play();
        }

        _onButtonPressed.Invoke(number);
    }
}
