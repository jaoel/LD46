using UnityEngine;

public class ColorControl : MonoBehaviour
{
    [SerializeField]
    private Renderer _renderer = null;

    private MaterialPropertyBlock mpb = null;

    // Start is called before the first frame update
    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
    }

    public void SetEmissiveColor(Color color) {
        SetEmissiveColor(new Vector3(color.r, color.g, color.b));
    }

    public void SetEmissiveColor(Vector3 color) {
        _renderer.GetPropertyBlock(mpb);
        mpb.SetVector("_EmissionColor", ColorPalette.GetLEDEmissiveColor(color));
        _renderer.SetPropertyBlock(mpb);
    }

    public void SetAlbedoColor(Vector3 color) {
        _renderer.GetPropertyBlock(mpb);
        mpb.SetVector("_EmissionColor", color);
        _renderer.SetPropertyBlock(mpb);
    }
}
