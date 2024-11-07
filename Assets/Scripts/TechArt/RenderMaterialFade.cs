using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class RenderMaterialFade : MonoBehaviour
{ 
    [Header("Parameters")]
    [SerializeField] private new Renderer renderer;
    [SerializeField] private float fadeDuration = 0.5f;
    
    [Header("Render Blends")]
    [SerializeField] UnityEngine.Rendering.BlendMode scrBlendMode = UnityEngine.Rendering.BlendMode.SrcAlpha;
    [SerializeField] UnityEngine.Rendering.BlendMode dstBlendMode = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;

    public void FadeMaterial()
    {
        StartCoroutine(Fade());
        transform.DOMoveY(8f, fadeDuration).SetEase(Ease.InQuart);
    }

    private IEnumerator Fade()
    {
        float time = 0;
        float alpha;
        while (time < fadeDuration)
        {
            alpha = Mathf.Lerp(1f, 0f, time/fadeDuration);
            renderer.material.color =  new Color(renderer.material.color.r,renderer.material.color.g,renderer.material.color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }
    }
    
    public void ResetFade()
    {
        StopCoroutine(Fade());
        transform.position = Vector3.zero;
        renderer.material.color =  new Color(renderer.material.color.r,renderer.material.color.g,renderer.material.color.b, 1f);
    }
}