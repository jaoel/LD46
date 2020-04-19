using UnityEngine;

public class PressureGauge : MonoBehaviour {
    public GameObject needle;

    public float State { get; private set; }

    public void SetState(float newState) {
        State = Mathf.Clamp01(newState);
    }

    private void Update() {
        float vibratoState = Mathf.Clamp01((State - 0.7f) / 0.2f);
        float vibrato = Mathf.Lerp(0f, Mathf.Sin(Time.time * 75f) * 5f, vibratoState);
        needle.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-75f, 75f, State) + vibrato);
    }
}
