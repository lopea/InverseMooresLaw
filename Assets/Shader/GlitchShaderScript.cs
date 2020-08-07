using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchShaderScript : MonoBehaviour
{
    Material _material;
    

    // Start is called before the first frame update
    void Start()
    {
        _material= new Material(Shader.Find("Hidden/GlitchEffect"));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source,destination, _material);
    }
}
