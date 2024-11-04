using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour,IEndDragHandler, IDragHandler
{
    [Header("Page Swiper Settings")]
    [SerializeField][Tooltip("Page in order left to right")] private List<GameObject> panels;
    [SerializeField]private int _pageActive;
    [SerializeField] private float sizeThreashold = 0.2f;
    [SerializeField] private UnityEvent<int> onPageSwiped;
    
    [Header("Settings Effects")]
    [SerializeField] private float punchEffect = 10f;
    [SerializeField] private float duration = 0.2f;

    private void Start() => SwapPage();

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if (Mathf.Abs(percentage) > sizeThreashold)
        {
            _pageActive = Math.Clamp(_pageActive + (percentage > 0 ? 1 : -1) , 0, panels.Count-1);
            SwapPage();
            panels[_pageActive].transform.DOPunchPosition(new Vector3((percentage > 0 ? 1 : -1) * punchEffect, 0), duration);
        }
    }
    
    public void OnDrag(PointerEventData eventData){}

    private void SwapPage()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (i == _pageActive) onPageSwiped?.Invoke(i);
            panels[i].SetActive(_pageActive == i);
        }
    }

}
