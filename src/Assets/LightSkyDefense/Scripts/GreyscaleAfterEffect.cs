using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GreyscaleAfterEffect : MonoBehaviour
{
    private Material _afterEffectMaterial;

    public float Intensity = 1f;
    public bool Active;

    public void Start()
    {
        _afterEffectMaterial = new Material(Shader.Find("Hidden/Grey"));
        _afterEffectMaterial.SetFloat("_bwBlend", Intensity);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (Active)
        {
            Graphics.Blit(source, destination, _afterEffectMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

}

