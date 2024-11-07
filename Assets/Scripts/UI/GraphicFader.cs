using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GraphicFader : MonoBehaviour
{
    [SerializeField][Range(0,1)] private float transparencyMin;
    [SerializeField] private float fadeDuration;
    [SerializeField] private Graphic image;

    private Color _color;

    private void Awake()
    {
        if (image) _color = image.color;
    }

    private void OnEnable()
    {
        if (!image) return;
        image.DOFade(transparencyMin, fadeDuration).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        if (!image) return;
        image.DOKill();
        image.color = _color;
    }
}
