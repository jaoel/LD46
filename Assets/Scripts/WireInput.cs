using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WireInput : MonoBehaviour
{
    [SerializeField] private UnityEventWire onConnectWire = new UnityEventWire();
    [SerializeField] private UnityEvent onDisconnectWire = new UnityEvent();
    [SerializeField] private Wire connectedWire = null;
    [SerializeField] private AudioClip connectSound;
    [SerializeField] private AudioClip disconnectSound;

    private AudioSource audioSource;
    private static float soundLastPlayedTime = -10f;

    public bool Connected => connectedWire != null;
    public Wire ConnectedWire => connectedWire;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayAudioCLip(AudioClip clip) {
        if (audioSource != null && clip != null && Time.time > soundLastPlayedTime + 0.1f) {
            soundLastPlayedTime = Time.time;
            audioSource.PlayOneShot(clip);
        }
    }

    public bool Connect(Wire wire)
    {
        if (connectedWire == null)
        {
            connectedWire = wire;
            connectedWire.connectedWireInput = this;
            onConnectWire.Invoke(wire);
            PlayAudioCLip(connectSound);
            return true;
        }
        return false;
    }

    public void Disconnect() {
        if (connectedWire != null) {
            connectedWire.connectedWireInput = null;
            connectedWire = null;
            onDisconnectWire.Invoke();
            PlayAudioCLip(disconnectSound);
        }
    }
}
