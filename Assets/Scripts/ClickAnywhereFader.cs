using UnityEngine;

public class ClickAnywhereFader : MonoBehaviour {
    [SerializeField]
    private TMPro.TMP_Text _clickAnywhereText;

    private bool _shouldBeVisible = false;
    private float _visibleStartTime = 0f;
    private float _fadeInDuration = 4f;
    private float _fadeOutDuration = 2f;
    private bool done = true;

    public void SetVisible(bool visible) {
        _shouldBeVisible = visible;
        _visibleStartTime = Time.unscaledTime;
        done = false;
    }

    private void Update() {
        if (!done) {
            if (_shouldBeVisible) {
                float t = Mathf.Clamp01((Time.unscaledTime - _visibleStartTime) / _fadeInDuration);
                _clickAnywhereText.color = Color.Lerp(Color.clear, Color.white, t);
                if (t == 1f) {
                    done = true;
                }
            } else {
                float t = Mathf.Clamp01((Time.unscaledTime - _visibleStartTime) / _fadeOutDuration);
                _clickAnywhereText.color = Color.Lerp(Color.white, Color.clear, t);
                if (t == 1f) {
                    done = true;
                }
            }
        }
    }
}
