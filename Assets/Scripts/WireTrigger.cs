using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireTrigger : MonoBehaviour {
    public float radius = 0.1f;
    private List<WireInputTrigger> _inputTriggers = new List<WireInputTrigger>();

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out WireInputTrigger inputTrigger)) {
            if(!_inputTriggers.Contains(inputTrigger)) {
                _inputTriggers.Add(inputTrigger);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent(out WireInputTrigger inputTrigger)) {
            if (_inputTriggers.Contains(inputTrigger)) {
                _inputTriggers.Remove(inputTrigger);
            }
        }
    }

    public bool TryGetClosestInput(Vector3 position, out WireInput closestInput) {
        float closest = float.MaxValue;
        closestInput = null;
        foreach (var inputTrigger in _inputTriggers) {
            float dist = Vector3.Distance(inputTrigger.transform.position, position);
            if (dist < radius && dist < closest) {
                closest = dist;
                closestInput = inputTrigger.owner;
            }
        }

        return closestInput != null;
    }
}
