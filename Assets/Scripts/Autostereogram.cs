using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autostereogram : MonoBehaviour
{
    
    public Material autoStereogram;
    private bool _easyMode = false; 
    public bool EasyMode
    {
        get { return _easyMode; }
        set
        {
            _easyMode = value;
            autoStereogram.SetFloat("_blend", _easyMode ? .5f : 0);
        }
    }

    private void Awake()
    {
        EasyMode = false; 
    }
    private void Start()
    {
       Camera.main.depthTextureMode = DepthTextureMode.Depth;
       
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Apply the autostereogram shader
        Graphics.Blit(source, destination, autoStereogram);
    }
   
}
