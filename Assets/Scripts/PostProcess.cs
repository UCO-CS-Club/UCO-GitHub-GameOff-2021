using UnityEngine;

[ExecuteInEditMode]
public class PostProcess : MonoBehaviour
{

    public Material material;

    public float waveLength;
    public float waveHeight;
    public float effectSpeed;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    void Update()
    {
        material.SetFloat("_waveHeight", waveHeight);
        material.SetFloat("_speed", effectSpeed);
        material.SetFloat("_waveLength", waveLength);
    }
}