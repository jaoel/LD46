using UnityEngine;
using System.Collections;

public class ScrollingText : MonoBehaviour {
    public RectTransform textTransform = null;
    public TMPro.TextMeshProUGUI text = null;
    public float speed = 1f;
    public float space = 1f;

    private void Update() {
        float textWidth = text.renderedWidth;
        textTransform.anchoredPosition = textTransform.anchoredPosition - Vector2.right * speed * Time.deltaTime;
        if (textTransform.anchoredPosition.x < -textWidth) {
            textTransform.anchoredPosition = new Vector2(space, textTransform.anchoredPosition.y);
        }
    }

    public void SetText(string text) {
        this.text.text = text;
        this.text.ForceMeshUpdate();
        textTransform.anchoredPosition = new Vector2(space, textTransform.anchoredPosition.y);
    }
}