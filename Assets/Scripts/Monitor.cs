using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour {
    public enum MonitorMode {
        Status,
        Error
    }

    public GameObject statusObject;
    public GameObject ErrorObject;

    public RectTransform clockMinute;
    public RectTransform clockHour;

    public TMPro.TextMeshProUGUI statusText;
    public ScrollingText scrollingText;

    public TMPro.TextMeshProUGUI countdownText;

    [SerializeField] private TheToggler powerToggler = null;
    private MonitorMode mode = MonitorMode.Status;

    private GameManager gameManager;

    private int prevFunctionalCount = 0;
    private int prevBrokenCount = 0;

    public void TogglePower() {
        powerToggler.Toggle(!powerToggler.Toggled);
    }

    public void SetMode(MonitorMode newMode) {
        if(newMode != mode) {
            switch (newMode) {
                case MonitorMode.Status:
                    break;
                case MonitorMode.Error:
                    break;
            }
            statusObject.SetActive(newMode == MonitorMode.Status);
            ErrorObject.SetActive(newMode == MonitorMode.Error);
            mode = newMode;
        }
    }

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        powerToggler.Toggle(true);
        statusObject.SetActive(mode == MonitorMode.Status);
        ErrorObject.SetActive(mode == MonitorMode.Error);
    }

    private void Update() {
        if (mode == MonitorMode.Status) {
            float t;
            if (gameManager != null) {
                t = gameManager.DayCompleteAmount;
            } else {
                t = Mathf.Repeat(Time.time * 0.1f, 1f);
            }

            float hour = t * 30f * 8f;
            float minute = hour * 12;

            clockHour.localRotation = Quaternion.Euler(0f, 0f, 90f - hour);
            clockMinute.localRotation = Quaternion.Euler(0f, 0f, -minute);

            if (gameManager != null) {
                int brokenCount = gameManager.BrokenComponentCount;
                int functionalCount = gameManager.FunctionalComponentCount;

                if (brokenCount != prevBrokenCount || functionalCount != prevFunctionalCount) {
                    prevBrokenCount = brokenCount;
                    prevFunctionalCount = functionalCount;

                    statusText.text =
                        "OK: " + functionalCount + "\n" +
                        "ERROR: " + brokenCount + "\n";
                }
            }

            if (gameManager?.Dying == true) {
                SetMode(MonitorMode.Error);
            }
        } else if (mode == MonitorMode.Error) {
            countdownText.text = gameManager.DeadTimer.ToString("0.00");
            if (!gameManager.Dying && !gameManager.Dead) {
                SetMode(MonitorMode.Status);
            }
        }

    }
}
