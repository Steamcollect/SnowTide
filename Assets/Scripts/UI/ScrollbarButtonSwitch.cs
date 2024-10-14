using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollbarButtonSwitch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Scrollbar _scrollbar;

    public void OnPointerClick(PointerEventData eventData)
    {
        _scrollbar.value = _scrollbar.value > 0 ? 1 : 0;
    }
}
