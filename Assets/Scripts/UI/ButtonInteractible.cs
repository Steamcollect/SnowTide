using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonInteractible : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] [Range(0,1)] private float sizeFade = 0.8f;
    [SerializeField] private float fadeTime = 0.2f;
    private Vector2 startSize;
    
    private void Awake() => startSize = transform.localScale;
    
    public void OnPointerEnter(PointerEventData eventData) => transform.DOScale(startSize*sizeFade, fadeTime);

    public void OnPointerExit(PointerEventData eventData) => gameObject.transform.DOScale(startSize, fadeTime);

    private void OnDisable()
    {
        transform.DOKill();
        transform.localScale = startSize;
    }
}
