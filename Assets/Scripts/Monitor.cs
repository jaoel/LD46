using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour {
    public enum MonitorMode {
        Status,
        Warning
    }

    public RectTransform clockMinute;
    public RectTransform clockHour;

    [SerializeField] private TheToggler powerToggler = null;
    private MonitorMode mode = MonitorMode.Status;

    private GameManager gameManager;

    public void TogglePower() {
        powerToggler.Toggle(!powerToggler.Toggled);
    }

    public void SetMode(MonitorMode newMode) {
        if(newMode != mode) {
            switch (newMode) {
                case MonitorMode.Status:
                    break;
                case MonitorMode.Warning:
                    break;
            }
            mode = newMode;
        }
    }

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        powerToggler.Toggle(true);
    }

    private void Update() {
        float t;
        if (gameManager != null) {
            t = gameManager.DayCompleteAmount;
        } else {
            t = Mathf.Repeat(Time.time * 0.1f, 1f);
        }

        float hour = t * 30f * 8f;
        float minute = hour * 12;

        clockHour.localRotation = Quaternion.Euler(0f, 0f, 90f-hour);
        clockMinute.localRotation = Quaternion.Euler(0f, 0f, -minute);
    }
}
