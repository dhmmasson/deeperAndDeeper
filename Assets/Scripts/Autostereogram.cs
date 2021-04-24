using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autostereogram : MonoBehaviour
{
    public Shader autostereogramShader;
    public Material main; 
    private void Start()
    {
       // Camera.main.depthTextureMode = DepthTextureMode.Depth;
       // Camera.main.SetReplacementShader(autostereogramShader, "Opaque");
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, main);
        
    }

}
