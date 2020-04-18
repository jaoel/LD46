using UnityEngine;
using System.Collections;

public class LCDMonitor : MonoBehaviour {
    public int width = 128;
    public int height = 128;
    public Camera renderCamera = null;
    public Renderer renderObject = null;

    private RenderTexture renderTexture = null;
    private MaterialPropertyBlock renderObjectMPB = null;

    private void Start() {
        Init();
    }

    private void OnDestroy() {
        Cleanup();
    }

    private void Init() {
        if (renderCamera != null) {
            if (renderTexture == null) {
                renderTexture = new RenderTexture(width, height, 0);
                renderTexture.filterMode = FilterMode.Point;
            }
            renderCamera.targetTexture = renderTexture;
        }

        renderObjectMPB = new MaterialPropertyBlock();

        if (renderObject != null) {
            renderObjectMPB.SetTexture("_MainTex", renderTexture);
            renderObject.SetPropertyBlock(renderObjectMPB);
        }
    }

    private void Cleanup() {
        if (renderCamera != null) {
            renderCamera.targetTexture = null;
            renderCamera.Render();
        }
        if (renderTexture != null) {
            renderTexture.Release();
            renderTexture = null;
        }
    }

}
