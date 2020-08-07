using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShaderScript : MonoBehaviour
{
    Material mat;
    
    void Awake()
    {
        mat = new Material(Shader.Find("Hidden/Shader"));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(ComputerStateManager.CurrentState == ComputerState._8bit)
            Graphics.Blit(src, dest, mat);
        else
        {
            Graphics.Blit(src,dest);
        }
    }
}
