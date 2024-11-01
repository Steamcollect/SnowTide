using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class RenderMaterialFade : MonoBehaviour
{ 
    [Header("Parameters")]
    [SerializeField] private Renderer renderer;
    [SerializeField] private float fadeDuration = 0.5f;
    
    [Header("Render Blends")]
    [SerializeField] UnityEngine.Rendering.BlendMode scrBlendMode = UnityEngine.Rendering.BlendMode.SrcAlpha;
    [SerializeField] UnityEngine.Rendering.BlendMode dstBlendMode = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
   
    public void FadeMaterial()
    {
        renderer.material.SetInt("_SrcBlend", (int)scrBlendMode);
        renderer.material.SetInt("_DstBlend", (int)dstBlendMode);
        renderer.material.SetInt("_ZWrite", 0);
        renderer.material.DisableKeyword("_ALPHATEST_ON");
        renderer.material.DisableKeyword("_ALPHABLEND_ON");
        renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = 3000;
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
        transform.position = Vector3.zero;
        StopCoroutine(Fade());
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        renderer.material.SetInt("_ZWrite", 1);
        renderer.material.DisableKeyword("_ALPHATEST_ON");
        renderer.material.DisableKeyword("_ALPHABLEND_ON");
        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = -1;
    }
}