using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WireInput : MonoBehaviour
{
    [SerializeField] private UnityEventWire onConnectWire = new UnityEventWire();
    [SerializeField] private UnityEvent onDisconnectWire = new UnityEvent();
    [SerializeField] private Wire connectedWire = null;

    public bool Connected => connectedWire != null;
    public Wire ConnectedWire => connectedWire;

    public bool Connect(Wire wire) {
        if (connectedWire == null) {
            connectedWire = wire;
            onConnectWire.Invoke(wire);
            return true;
        }
        return false;
    }

    public void Disconnect() {
        if (connectedWire != null) {
            connectedWire = null;
            onDisconnectWire.Invoke();
        }
    }
}
