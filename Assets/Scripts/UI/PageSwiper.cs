using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour,IDragHandler,IEndDragHandler
{
    private Vector3 panelLocation;
    public float sizeThreashold;

    private void Start() => panelLocation = transform.position;

    public void OnDrag(PointerEventData eventData)
    {
        float difference = eventData.pressPosition.x - eventData.position.x;
        transform.position = panelLocation + new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if (Mathf.Abs(percentage) > sizeThreashold)
        {
            Vector3 newPos = panelLocation;
            if (percentage > 0) newPos += new Vector3(-Screen.width,0,0);
            else if (percentage < 0) newPos += new Vector3(Screen.width,0,0);
            transform.position = newPos;
            panelLocation = newPos;
        }
        else
        {
            transform.position = panelLocation;
        }
    }
}
