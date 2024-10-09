using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class UiInteractible : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform target;
    [SerializeField] [Range(0,2)] private float sizeFade = 0.8f;
    [SerializeField] private float fadeTime = 0.2f;
    private Vector2 startSize;
    
    private void Awake()
    {
        if (!target) target = transform;
        startSize = target.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData) => target.DOScale(startSize*sizeFade, fadeTime);

    public void OnPointerExit(PointerEventData eventData) => target.DOScale(startSize, fadeTime);

    private void OnDisable()
    {
        target.DOKill();
        target.localScale = startSize;
    }
}
