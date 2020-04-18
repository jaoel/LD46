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

    public void SetColor(Vector3 color) {
        _renderer.GetPropertyBlock(mpb);
        mpb.SetVector("_EmissionColor", color);
        _renderer.SetPropertyBlock(mpb);
    }
}
