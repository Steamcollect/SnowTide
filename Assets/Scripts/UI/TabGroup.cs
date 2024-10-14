using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler,IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private Image background;
    [SerializeField] private List<GameObject> content;
    
    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.grey;
    [SerializeField] private Color activeColor = Color.grey;

    public Action<PointerHandlerAction, TabGroup> OnActionHandler;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnActionHandler?.Invoke(PointerHandlerAction.PointEnter, this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnActionHandler?.Invoke(PointerHandlerAction.PointClick, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnActionHandler?.Invoke(PointerHandlerAction.PointExit, this);
    }

    public void ImageState(int indexState)
    {
        switch (indexState)
        {
            case 0:
                background.color = normalColor;
                break;
            case 1:
                background.color = hoverColor;
                break;
            case 2:
                background.color = activeColor;
                break;
        }
    }

    public void ChangeStateContent(bool active)
    {
        foreach (var objectContent in content)
        {
            objectContent.SetActive(active);
        }
    }
    
}

public enum PointerHandlerAction
{
    PointEnter,
    PointClick,
    PointExit,
}
